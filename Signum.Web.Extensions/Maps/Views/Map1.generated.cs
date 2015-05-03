﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    
    #line 2 "..\..\Maps\Views\Map.cshtml"
    using System.Reflection;
    
    #line default
    #line hidden
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 3 "..\..\Maps\Views\Map.cshtml"
    using Newtonsoft.Json;
    
    #line default
    #line hidden
    using Signum.Entities;
    using Signum.Utilities;
    using Signum.Web;
    
    #line 1 "..\..\Maps\Views\Map.cshtml"
    using Signum.Web.Maps;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Maps/Views/Map.cshtml")]
    public partial class _Maps_Views_Map_cshtml : System.Web.Mvc.WebViewPage<MapInfo>
    {
        public _Maps_Views_Map_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Maps\Views\Map.cshtml"
Write(Html.ScriptCss("~/Maps/Content/map.css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 6 "..\..\Maps\Views\Map.cshtml"
   var colorProviders = (List<MapColorProvider>)ViewData["colorProviders"]; 
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"form-inline form-sm container\"");

WriteLiteral(" style=\"margin-top: 10px;\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"filter\"");

WriteLiteral(">Filter By</label>\r\n        <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"filter\"");

WriteLiteral(" placeholder=\"type or namespace\"");

WriteLiteral(">\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" style=\"margin-left: 10px;\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"color\"");

WriteLiteral(">Color</label>\r\n        <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"color\"");

WriteLiteral(">\r\n");

            
            #line 16 "..\..\Maps\Views\Map.cshtml"
            
            
            #line default
            #line hidden
            
            #line 16 "..\..\Maps\Views\Map.cshtml"
             foreach(var cp in colorProviders)
            {

            
            #line default
            #line hidden
WriteLiteral("                <option");

WriteAttribute("value", Tuple.Create(" value=\"", 706), Tuple.Create("\"", 722)
            
            #line 18 "..\..\Maps\Views\Map.cshtml"
, Tuple.Create(Tuple.Create("", 714), Tuple.Create<System.Object, System.Int32>(cp.Name
            
            #line default
            #line hidden
, 714), false)
);

WriteLiteral(">");

            
            #line 18 "..\..\Maps\Views\Map.cshtml"
                                    Write(cp.NiceName);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n");

            
            #line 19 "..\..\Maps\Views\Map.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </select>\r\n    </div>\r\n</div>\r\n\r\n<div");

WriteLiteral(" id=\"map\"");

WriteLiteral(">\r\n    <svg");

WriteLiteral(" id=\"svgMap\"");

WriteLiteral(">\r\n        <defs>\r\n          \r\n\r\n            <marker");

WriteLiteral(" id=\"normal_arrow\"");

WriteLiteral(" viewBox=\"0 -5 10 10\"");

WriteLiteral(" refX=\"10\"");

WriteLiteral(" refY=\"0\"");

WriteLiteral(" markerWidth=\"10\"");

WriteLiteral(" markerHeight=\"10\"");

WriteLiteral(" orient=\"auto\"");

WriteLiteral(">\r\n                <path");

WriteLiteral(" fill=\"gray\"");

WriteLiteral(" d=\"M0,0L0,-5L10,0L0,5L0,0\"");

WriteLiteral(" />\r\n            </marker>    \r\n\r\n            <marker");

WriteLiteral(" id=\"lite_arrow\"");

WriteLiteral(" viewBox=\"0 -5 10 10\"");

WriteLiteral(" refX=\"10\"");

WriteLiteral(" refY=\"0\"");

WriteLiteral(" markerWidth=\"10\"");

WriteLiteral(" markerHeight=\"10\"");

WriteLiteral(" orient=\"auto\"");

WriteLiteral(">\r\n                <path");

WriteLiteral(" fill=\"gray\"");

WriteLiteral(" d=\"M5,0L0,-5L10,0L0,5L5,0\"");

WriteLiteral(" />\r\n            </marker>    \r\n\r\n            <marker");

WriteLiteral(" id=\"mlist_arrow\"");

WriteLiteral(" viewBox=\"-10 -5 20 10\"");

WriteLiteral(" refX=\"10\"");

WriteLiteral(" refY=\"0\"");

WriteLiteral(" markerWidth=\"10\"");

WriteLiteral(" markerHeight=\"20\"");

WriteLiteral(" orient=\"auto\"");

WriteLiteral(">\r\n                <path");

WriteLiteral(" fill=\"black\"");

WriteLiteral(" d=\"M0,0L0,-5L10,0L0,5L0,0L-10,5L-10,-5L0,0\"");

WriteLiteral(" />\r\n            </marker>  \r\n            \r\n");

            
            #line 41 "..\..\Maps\Views\Map.cshtml"
            
            
            #line default
            #line hidden
            
            #line 41 "..\..\Maps\Views\Map.cshtml"
             foreach(var cp in colorProviders.Where(c=>c.Defs != null))
            {
                
            
            #line default
            #line hidden
            
            #line 43 "..\..\Maps\Views\Map.cshtml"
           Write(cp.Defs);

            
            #line default
            #line hidden
            
            #line 43 "..\..\Maps\Views\Map.cshtml"
                        ;
            } 

            
            #line default
            #line hidden
WriteLiteral("        </defs>\r\n    </svg>\r\n</div>\r\n<script>\r\n    function getProvider(name, nod" +
"es) {\r\n        return new Promise(function (resolve) {\r\n            switch (name" +
") {\r\n");

            
            #line 52 "..\..\Maps\Views\Map.cshtml"
                
            
            #line default
            #line hidden
            
            #line 52 "..\..\Maps\Views\Map.cshtml"
                 foreach (var cp in colorProviders)
                {

            
            #line default
            #line hidden
WriteLiteral("                    ");

WriteLiteral("case \"");

            
            #line 54 "..\..\Maps\Views\Map.cshtml"
                           Write(cp.Name);

            
            #line default
            #line hidden
WriteLiteral("\": \r\n                    require([\'");

            
            #line 55 "..\..\Maps\Views\Map.cshtml"
                         Write(cp.GetJsProvider.Module);

            
            #line default
            #line hidden
WriteLiteral("\'], function(mod) { \r\n                        resolve(mod.");

            
            #line 56 "..\..\Maps\Views\Map.cshtml"
                                Write(cp.GetJsProvider.FunctionName);

            
            #line default
            #line hidden
WriteLiteral("(");

            
            #line 56 "..\..\Maps\Views\Map.cshtml"
                                                                Write(Html.Raw(cp.GetJsProvider.Arguments.ToString(a=>a == MapClient.NodesConstant ? "nodes" : 
                                                             JsonConvert.SerializeObject(a, cp.GetJsProvider.JsonSerializerSettings),", ")));

            
            #line default
            #line hidden
WriteLiteral(")) \r\n                    }); break\r\n                    ");

WriteLiteral("\r\n");

            
            #line 60 "..\..\Maps\Views\Map.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            }\r\n        });\r\n    }\r\n\r\n\r\n");

WriteLiteral("    ");

            
            #line 66 "..\..\Maps\Views\Map.cshtml"
Write(MapClient.Module["createMap"]("map", "svgMap", "filter", "color", Model, "getProvider"));

            
            #line default
            #line hidden
WriteLiteral("\r\n</script>");

        }
    }
}
#pragma warning restore 1591
