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
    using Signum.Entities.Authorization;
    using Signum.Utilities;
    using Signum.Web;
    using Signum.Web.Auth;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Auth/Views/Register.cshtml")]
    public partial class _Auth_Views_Register_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Auth_Views_Register_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<h2>Account Creation</h2>\r\n<p>\r\n    Use the form below to create a new account.\r\n" +
"</p>\r\n<p>\r\n    Passwords are required to be a minimum of ");

            
            #line 6 "..\..\Auth\Views\Register.cshtml"
                                         Write(ViewData["PasswordLength"]);

            
            #line default
            #line hidden
WriteLiteral(" characters\r\n    in length.\r\n</p>\r\n");

            
            #line 9 "..\..\Auth\Views\Register.cshtml"
Write(Html.ValudationSummaryStatic());

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 10 "..\..\Auth\Views\Register.cshtml"
 using (Html.BeginForm())
{

            
            #line default
            #line hidden
WriteLiteral("    <div>\r\n        <table>\r\n            <tr>\r\n                <td>Username:\r\n    " +
"            </td>\r\n                <td>\r\n");

WriteLiteral("                    ");

            
            #line 18 "..\..\Auth\Views\Register.cshtml"
               Write(Html.TextBox("username"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 19 "..\..\Auth\Views\Register.cshtml"
               Write(Html.ValidationMessage("username"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n                <td" +
">Email:\r\n                </td>\r\n                <td>\r\n");

WriteLiteral("                    ");

            
            #line 26 "..\..\Auth\Views\Register.cshtml"
               Write(Html.TextBox("email"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 27 "..\..\Auth\Views\Register.cshtml"
               Write(Html.ValidationMessage("email"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n                <td" +
">Password:\r\n                </td>\r\n                <td>\r\n");

WriteLiteral("                    ");

            
            #line 34 "..\..\Auth\Views\Register.cshtml"
               Write(Html.Password("password"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 35 "..\..\Auth\Views\Register.cshtml"
               Write(Html.ValidationMessage("password"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n                <td" +
">Confirm password:\r\n                </td>\r\n                <td>\r\n");

WriteLiteral("                    ");

            
            #line 42 "..\..\Auth\Views\Register.cshtml"
               Write(Html.Password("confirmPassword"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 43 "..\..\Auth\Views\Register.cshtml"
               Write(Html.ValidationMessage("confirmPassword"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </td>\r\n            </tr>\r\n            <tr>\r\n                <td" +
"></td>\r\n                <td>\r\n                    <input");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" value=\"Register\"");

WriteLiteral(" />\r\n                </td>\r\n            </tr>\r\n        </table>\r\n    </div>\r\n");

            
            #line 54 "..\..\Auth\Views\Register.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
