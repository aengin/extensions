﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
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
    using Signum.Entities;
    using Signum.Utilities;
    
    #line 1 "..\..\Profiler\Views\Statistics.cshtml"
    using Signum.Utilities.ExpressionTrees;
    
    #line default
    #line hidden
    using Signum.Web;
    
    #line 2 "..\..\Profiler\Views\Statistics.cshtml"
    using Signum.Web.Profiler;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Profiler/Views/Statistics.cshtml")]
    public partial class _Profiler_Views_Statistics_cshtml : System.Web.Mvc.WebViewPage<IOrderedEnumerable<SqlProfileResume>>
    {
        public _Profiler_Views_Statistics_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Profiler\Views\Statistics.cshtml"
             
    SqlProfileResumeOrder order = ViewBag.Order;

            
            #line default
            #line hidden
WriteLiteral("\r\n<h2>");

            
            #line 7 "..\..\Profiler\Views\Statistics.cshtml"
Write(ViewData[ViewDataKeys.Title]);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n<div>\r\n");

WriteLiteral("  ");

            
            #line 9 "..\..\Profiler\Views\Statistics.cshtml"
Write(Html.Partial(ProfilerClient.ViewPrefix.FormatWith("ProfilerButtons")));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("  ");

            
            #line 10 "..\..\Profiler\Views\Statistics.cshtml"
Write(Html.ActionLink("Root Entries", "Heavy", new {@class = "sf-button" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n</div>\r\n\r\n");

            
            #line 13 "..\..\Profiler\Views\Statistics.cshtml"
Write(Html.Partial(ProfilerClient.ViewPrefix.FormatWith("StatisticsTable")));

            
            #line default
            #line hidden
WriteLiteral("\r\n<script>\r\n    $(function () {\r\n");

WriteLiteral("        ");

            
            #line 16 "..\..\Profiler\Views\Statistics.cshtml"
    Write(ProfilerClient.Module["initStats"]());

            
            #line default
            #line hidden
WriteLiteral(";\r\n    });\r\n</script>\r\n");

        }
    }
}
#pragma warning restore 1591
