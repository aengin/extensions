﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Signum.Utilities;
using Signum.Entities.DynamicQuery;
using Signum.Entities.Files;
using System.Xml.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using Signum.Entities.Reflection;
using Signum.Utilities.Reflection;
using System.Reflection;

namespace Signum.Entities.Chart
{
    [Serializable, EntityKind(EntityKind.Main, EntityData.Master)]
    public class ChartScriptEntity : Entity
    {
        [NotNullable, SqlDbType(Size = 100), UniqueIndex]
        string name;
        [StringLengthValidator(AllowNulls = false, Min = 3, Max = 100)]
        public string Name
        {
            get { return name; }
            set { SetToStr(ref name, value); }
        }

        Lite<FileEntity> icon;
        public Lite<FileEntity> Icon
        {
            get { return icon; }
            set { Set(ref icon, value); }
        }

        [NotNullable, SqlDbType(Size = int.MaxValue)]
        string script;
        [StringLengthValidator(AllowNulls = false, Min = 3)]
        public string Script
        {
            get { return script; }
            set { Set(ref script, value); }
        }

        GroupByChart groupBy;
        public GroupByChart GroupBy
        {
            get { return groupBy; }
            set { Set(ref groupBy, value); }
        }

        [NotifyCollectionChanged, ValidateChildProperty, NotNullable, PreserveOrder]
        MList<ChartScriptColumnEntity> columns = new MList<ChartScriptColumnEntity>();
        public MList<ChartScriptColumnEntity> Columns
        {
            get { return columns; }
            set { Set(ref columns, value); }
        }

        [NotNullable, SqlDbType(Size = 100)]
        string columnsStructure;
        [StringLengthValidator(AllowNulls = false, Min = 3, Max = 100)]
        public string ColumnsStructure
        {
            get { return columnsStructure; }
            set { Set(ref columnsStructure, value); }
        }

        static Expression<Func<ChartScriptEntity, string>> ToStringExpression = e => e.name;
        public override string ToString()
        {
            return ToStringExpression.Evaluate(this);
        }

        public string ColumnsToString()
        {
            return Columns.ToString(a => a.ColumnType.ToString(), "|");
        }

        protected override string ChildPropertyValidation(ModifiableEntity sender, System.Reflection.PropertyInfo pi)
        {
            var column = sender as ChartScriptColumnEntity;

            if (column != null && pi.Is(() => column.IsGroupKey))
            {
                if (column.IsGroupKey)
                {
                    if (!ChartUtils.Flag(ChartColumnType.Groupable, column.ColumnType))
                        return "{0} can not be true for {1}".FormatWith(pi.NiceName(), column.ColumnType.NiceToString());
                }
            }

            return base.ChildPropertyValidation(sender, pi);
        }

        protected override string PropertyValidation(System.Reflection.PropertyInfo pi)
        {
            if (pi.Is(() => GroupBy))
            {
                if (GroupBy == GroupByChart.Always || GroupBy == GroupByChart.Optional)
                {
                    if (!Columns.Any(a => a.IsGroupKey))
                        return "{0} {1} requires some key columns".FormatWith(pi.NiceName(), groupBy.NiceToString());
                }
                else
                {
                    if (Columns.Any(a => a.IsGroupKey))
                        return "{0} {1} should not have key".FormatWith(pi.NiceName(), groupBy.NiceToString());
                }
            }

            if (pi.Is(() => Script))
            {
                if (!Regex.IsMatch(Script, @"function\s+DrawChart\s*\(\s*chart\s*,\s*data\s*\)", RegexOptions.Singleline))
                {
                    return "{0} should be a definition of function DrawChart(chart, data)".FormatWith(pi.NiceName());
                }
            }

            return base.PropertyValidation(pi);
        }

        protected override void PreSaving(ref bool graphModified)
        {
            string from = Columns.Where(a => a.IsGroupKey).ToString(c => c.ColumnType.GetCode() + (c.IsOptional ? "?" : ""), ",");
            string to = Columns.Where(a => !a.IsGroupKey).ToString(c => c.ColumnType.GetCode() + (c.IsOptional ? "?" : ""), ",");

            ColumnsStructure = "{0} -> {1}".FormatWith(from, to);

            base.PreSaving(ref graphModified);
        }

        protected override void PostRetrieving()
        {
            base.PostRetrieving();
        }

        public XDocument ExportXml()
        {
            var icon = Icon == null? null: Icon.Entity;

            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("ChartScript",
                    new XAttribute("GroupBy", GroupBy.ToString()),
                    new XElement("Columns",
                        Columns.Select(c => new XElement("Column",
                            new XAttribute("DisplayName", c.DisplayName),
                            new XAttribute("ColumnType", c.ColumnType.ToString()),
                            c.IsGroupKey ? new XAttribute("IsGroupKey", true) : null,
                            c.IsOptional ? new XAttribute("IsOptional", true) : null,
                            c.Parameter1 != null ? c.Parameter1.ExportXml(1): null,  
                            c.Parameter2 != null ? c.Parameter2.ExportXml(2): null,  
                            c.Parameter3 != null ? c.Parameter3.ExportXml(3): null  
                         ))),
                    icon == null ? null :
                    new XElement("Icon",
                        new XAttribute("FileName", icon.FileName),
                        new XCData(Convert.ToBase64String(Icon.Entity.BinaryFile))),
                    new XElement("Script", new XCData(Script))));
                    
        }

        public void ImportXml(XDocument doc, string name, bool force = false)
        {
            XElement script = doc.Root;

            GroupByChart groupBy = script.Attribute("GroupBy").Value.ToEnum<GroupByChart>();

            List<ChartScriptColumnEntity> columns = script.Element("Columns").Elements("Column").Select(c => new ChartScriptColumnEntity
            {
                DisplayName = c.Attribute("DisplayName").Value,
                ColumnType = c.Attribute("ColumnType").Value.ToEnum<ChartColumnType>(),
                IsGroupKey = c.Attribute("IsGroupKey").Let(a => a != null && a.Value == "true"),
                IsOptional = c.Attribute("IsOptional").Let(a => a != null && a.Value == "true"),
                Parameter1 = ChartScriptParameterEntity.ImportXml(c, 1),
                Parameter2 = ChartScriptParameterEntity.ImportXml(c, 2),
                Parameter3 = ChartScriptParameterEntity.ImportXml(c, 3)
            }).ToList();

            if (!IsNew && !force)
                AsssertColumns(columns);

            this.Name = name;
            this.GroupBy = groupBy;

            if (this.Columns.Count == columns.Count)
            {
                this.Columns.ZipForeach(columns, (o, n) =>
                {
                    o.ColumnType = n.ColumnType;
                    o.DisplayName = n.DisplayName;
                    o.IsGroupKey = n.IsGroupKey;
                    o.IsOptional = n.IsOptional;
                    o.Parameter1 = CombineParameters(o.Parameter1, n.Parameter1);
                    o.Parameter2 = CombineParameters(o.Parameter2, n.Parameter2);
                    o.Parameter3 = CombineParameters(o.Parameter3, n.Parameter3);
                }); 
            }
            else
            {
                this.Columns = columns.ToMList();
            }

            this.Script = script.Elements("Script").Nodes().OfType<XCData>().Single().Value;

            var newFile = script.Element("Icon").Try(icon => new FileEntity
            {
                FileName = icon.Attribute("FileName").Value,
                BinaryFile = Convert.FromBase64String(icon.Nodes().OfType<XCData>().Single().Value),
            });

            if (newFile == null)
            {
                Icon = null;
            }
            else
            {
                if (icon == null || icon.Entity.FileName != newFile.FileName || !AreEqual(icon.Entity.BinaryFile, newFile.BinaryFile))
                    Icon = newFile.ToLiteFat();
            }
        }

        private ChartScriptParameterEntity CombineParameters(ChartScriptParameterEntity oldP, ChartScriptParameterEntity newP)
        {
            if (newP == null)
                return null;

            if (oldP == null)
                oldP = new ChartScriptParameterEntity();

            oldP.Name = newP.Name;
            oldP.Type = newP.Type;
            oldP.ValueDefinition = newP.ValueDefinition;

            return newP;
        }

        static bool AreEqual(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                    return false;
            }

            return true;
        }

        private void AsssertColumns(List<ChartScriptColumnEntity> columns)
        {
            string errors = Columns.ZipOrDefault(columns, (o, n) =>
            {
                if (o == null)
                {
                    if (!n.IsOptional)
                        return "Adding non optional column {0}".FormatWith(n.DisplayName);
                }
                else if (n == null)
                {
                    if (o.IsOptional)
                        return "Removing non optional column {0}".FormatWith(o.DisplayName);
                }
                else if (n.ColumnType != o.ColumnType)
                {
                    return "The column type of '{0}' ({1}) does not match with '{2}' ({3})".FormatWith(
                        o.DisplayName, o.ColumnType,
                        n.DisplayName, n.ColumnType);
                }

                return null;
            }).NotNull().ToString("\r\n");

            if (errors.HasText())
                throw new FormatException("The columns doesn't match: \r\n" + errors);
        }

        public bool IsCompatibleWith(IChartBase chartBase)
        {
            if (GroupBy == GroupByChart.Always && !chartBase.GroupResults)
                return false;

            if (GroupBy == GroupByChart.Never && chartBase.GroupResults)
                return false;

            return Columns.ZipOrDefault(chartBase.Columns, (s, c) =>
            {
                if (s == null)
                    return c.Token == null;

                if (c == null || c.Token == null)
                    return s.IsOptional;

                if (!ChartUtils.IsChartColumnType(c.Token.Token, s.ColumnType))
                    return false;

                if (c.Token.Token is AggregateToken)
                    return !s.IsGroupKey;
                else
                    return s.IsGroupKey || !chartBase.GroupResults; 

            }).All(a => a);
        }

        public bool HasChanges()
        {
            var graph = GraphExplorer.FromRoot(this);
            return graph.Any(a => a.Modified == ModifiedState.SelfModified);
        }
    }

    public static class ChartScriptOperation
    {
        public static readonly ExecuteSymbol<ChartScriptEntity> Save = OperationSymbol.Execute<ChartScriptEntity>();
        public static readonly ConstructSymbol<ChartScriptEntity>.From<ChartScriptEntity> Clone = OperationSymbol.Construct<ChartScriptEntity>.From<ChartScriptEntity>();
        public static readonly DeleteSymbol<ChartScriptEntity> Delete = OperationSymbol.Delete<ChartScriptEntity>();
    }

    public enum GroupByChart
    {
        Always,
        Optional,
        Never
    }

    [Serializable]
    public class ChartScriptColumnEntity : EmbeddedEntity       
    {
        [NotNullable, SqlDbType(Size = 80)]
        string displayName;
        [StringLengthValidator(AllowNulls = false, Min = 3, Max = 80)]
        public string DisplayName
        {
            get { return displayName; }
            set { Set(ref displayName, value); }
        }

        bool isOptional;
        public bool IsOptional
        {
            get { return isOptional; }
            set { Set(ref isOptional, value); }
        }
     
        ChartColumnType columnType;
        public ChartColumnType ColumnType
        {
            get { return columnType; }
            set { Set(ref columnType, value); }
        }

        bool isGroupKey;
        public bool IsGroupKey
        {
            get { return isGroupKey; }
            set { Set(ref isGroupKey, value); }
        }

        ChartScriptParameterEntity parameter1;
        public ChartScriptParameterEntity Parameter1
        {
            get { return parameter1; }
            set { Set(ref parameter1, value); }
        }

        ChartScriptParameterEntity parameter2;
        public ChartScriptParameterEntity Parameter2
        {
            get { return parameter2; }
            set { Set(ref parameter2, value); }
        }

        ChartScriptParameterEntity parameter3;
        public ChartScriptParameterEntity Parameter3
        {
            get { return parameter3; }
            set { Set(ref parameter3, value); }
        }

        internal ChartScriptColumnEntity Clone()
        {
            return new ChartScriptColumnEntity
            {
                DisplayName = DisplayName,
                IsGroupKey = IsGroupKey,
                ColumnType = ColumnType,
                IsOptional = IsOptional,
                Parameter1 = Parameter1.Try(p => p.Clone()),
                Parameter2 = Parameter2.Try(p => p.Clone()),
                Parameter3 = Parameter3.Try(p => p.Clone()),
            };
        }
    }

    [Flags]
    public enum ChartColumnType
    {
        [Code("i")] Integer = 1,
        [Code("r")] Real = 2,
        [Code("d")] Date = 4,
        [Code("dt")] DateTime = 8,
        [Code("s")] String = 16, //Guid
        [Code("l")] Lite = 32,
        [Code("e")] Enum = 64, // Boolean 

        [Code("G")] Groupable = ChartColumnTypeUtils.GroupMargin | Integer | Date | String | Lite | Enum,
        [Code("M")] Magnitude = ChartColumnTypeUtils.GroupMargin | Integer | Real,
        [Code("P")] Positionable = ChartColumnTypeUtils.GroupMargin | Integer | Real | Date | DateTime | Enum
    }


    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class CodeAttribute : Attribute
    {
        string code;
        public CodeAttribute(string code)
        {
            this.code = code;
        }

        public string Code
        {
            get { return code; }
        }
    }

    public static class ChartColumnTypeUtils
    {
       public const int GroupMargin = 0x10000000;

       static Dictionary<ChartColumnType, string> codes = EnumFieldCache.Get(typeof(ChartColumnType)).ToDictionary(
           a => (ChartColumnType)a.Key,
           a => a.Value.GetCustomAttribute<CodeAttribute>().Code);

       public static string GetCode(this ChartColumnType columnType)
       {
           return codes[columnType];
       }

       public static string GetComposedCode(this ChartColumnType columnType)
       {
           var result = columnType.GetCode();

           if (result.HasText())
               return result;

           return EnumExtensions.GetValues<ChartColumnType>()
               .Where(a => (int)a < ChartColumnTypeUtils.GroupMargin && columnType.HasFlag(a))
               .ToString(GetCode, ",");
       }

       static Dictionary<string, ChartColumnType> fromCodes = EnumFieldCache.Get(typeof(ChartColumnType)).ToDictionary(
           a => a.Value.GetCustomAttribute<CodeAttribute>().Code,
           a => (ChartColumnType)a.Key);

       public static string TryParse(string code, out ChartColumnType type)
       {
           if(fromCodes.TryGetValue(code, out type))
               return null;
                
           return "{0} is not a valid type code, use {1} instead".FormatWith(code, fromCodes.Keys.CommaOr());
       }

       public static string TryParseComposed(string code, out ChartColumnType type)
       {
           type = default(ChartColumnType);
           foreach (var item in code.Split(','))
	       {
               ChartColumnType temp;
                string error = TryParse(item,   out temp);

               if(error.HasText())
                   return error;

               type |= temp;
           }
           return null;
       }
    }

   
}
