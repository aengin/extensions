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
    
    #line 1 "..\..\Dashboard\Views\Admin\LinkElement.cshtml"
    using Signum.Entities.Dashboard;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    #line 2 "..\..\Dashboard\Views\Admin\LinkElement.cshtml"
    using Signum.Web.Dashboard;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Dashboard/Views/Admin/LinkElement.cshtml")]
    public partial class _Dashboard_Views_Admin_LinkElement_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Dashboard_Views_Admin_LinkElement_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 4 "..\..\Dashboard\Views\Admin\LinkElement.cshtml"
 using (var tc = Html.TypeContext<LinkElementEntity>())
{
    
            
            #line default
            #line hidden
            
            #line 6 "..\..\Dashboard\Views\Admin\LinkElement.cshtml"
Write(Html.ValueLine(tc, l => l.Label));

            
            #line default
            #line hidden
            
            #line 6 "..\..\Dashboard\Views\Admin\LinkElement.cshtml"
                                     
    
            
            #line default
            #line hidden
            
            #line 7 "..\..\Dashboard\Views\Admin\LinkElement.cshtml"
Write(Html.ValueLine(tc, l => l.Link));

            
            #line default
            #line hidden
            
            #line 7 "..\..\Dashboard\Views\Admin\LinkElement.cshtml"
                                    
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
