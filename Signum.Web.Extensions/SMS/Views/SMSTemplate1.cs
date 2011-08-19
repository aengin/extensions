﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
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
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Caching;
    using System.Web.DynamicData;
    using System.Web.SessionState;
    using System.Web.Profile;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using System.Xml.Linq;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Signum.Utilities;
    using Signum.Entities;
    using Signum.Web;
    using Signum.Engine;
    using Signum.Entities.SMS;
    using Signum.Web.SMS;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/SMS/Views/SMSTemplate.cshtml")]
    public class _Page_SMS_Views_SMSTemplate_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {


        public _Page_SMS_Views_SMSTemplate_cshtml()
        {
        }
        protected System.Web.HttpApplication ApplicationInstance
        {
            get
            {
                return ((System.Web.HttpApplication)(Context.ApplicationInstance));
            }
        }
        public override void Execute()
        {




WriteLiteral("\r\n");


Write(Html.ScriptCss("~/SMS/Content/SMS.css"));

WriteLiteral("\r\n\r\n");


 using (var e = Html.TypeContext<SMSTemplateDN>())
{   
    
Write(Html.ValueLine(e, s => s.Name));

                                   
    
Write(Html.ValueLine(e, s => s.Active, vl => vl.ReadOnly = true));

                                                               
    
Write(Html.ValueLine(e, s => s.Message, vl =>
        {
            vl.ValueLineType = ValueLineType.TextArea;
            vl.ValueHtmlProps["cols"] = "30";
            vl.ValueHtmlProps["rows"] = "6";
        }));

          
    

WriteLiteral("    <div id=\"charactersleft\" data-url=\"");


                                   Write(Url.Action<SMSController>(s => s.GetDictionaries()));

WriteLiteral("\" style=\"margin-left: 150px;\">\r\n        <p>Quedan disponibles <span id=\"numberofc" +
"har\"></span> carácteres</p>\r\n    </div>\r\n");


    
    
    
Write(Html.ValueLine(e, s => s.From));

                                   
    
Write(Html.ValueLine(e, s => s.StartDate));

                                        
    
Write(Html.ValueLine(e, s => s.EndDate));

                                      
   
}

WriteLiteral("\r\n");


Write(Html.ScriptsJs("~/SMS/scripts/SF_SMS.js"));


        }
    }
}
