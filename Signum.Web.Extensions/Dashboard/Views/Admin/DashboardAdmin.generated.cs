﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Signum.Web.Extensions.Dashboard.Views.Admin
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    
    #line 3 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
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
    using Signum.Entities;
    
    #line 1 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
    using Signum.Entities.Dashboard;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    #line 2 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
    using Signum.Web.Dashboard;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
    using Signum.Web.UserAssets;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Dashboard/Views/Admin/DashboardAdmin.cshtml")]
    public partial class DashboardAdmin : System.Web.Mvc.WebViewPage<dynamic>
    {
        public DashboardAdmin()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 6 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
Write(Html.ScriptCss("~/Dashboard/Content/Dashboard.css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n");

            
            #line 9 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
 using (var tc = Html.TypeContext<DashboardDN>())
{
    using (var sc = tc.SubContext())
    {
        sc.FormGroupStyle = FormGroupStyle.Basic;
    

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-vertical\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 18 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
           Write(Html.ValueLine(sc, cp => cp.DisplayName));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n            <div");

WriteLiteral(" class=\"col-sm-3\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 21 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
           Write(Html.ValueLine(sc, cp => cp.DashboardPriority));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n             <div");

WriteLiteral(" class=\"col-sm-3\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 24 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
           Write(Html.ValueLine(sc, cp => cp.AutoRefreshPeriod));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 29 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
           Write(Html.EntityLine(sc, cp => cp.Owner, el => el.Create = false));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n            <div");

WriteLiteral(" class=\"col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 32 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
           Write(Html.EntityLine(sc, cp => cp.EntityType, el => { 
               el.AutocompleteUrl = Url.Action("TypeAutocomplete", "Finder");
               el.AttachFunction = UserAssetsClient.Module["attachShowEmbeddedInEntity"](el);
           }));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n             <div");

WriteLiteral(" class=\"col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("                 ");

            
            #line 38 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
            Write(Html.ValueLine(sc, f => f.EmbeddedInEntity));

            
            #line default
            #line hidden
WriteLiteral("\r\n             </div>\r\n        </div>\r\n    </div>\r\n");

            
            #line 42 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
    
    }
    
    
            
            #line default
            #line hidden
            
            #line 45 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
Write(Html.GridRepeater(tc, cp => cp.Parts, grid =>
        {
            grid.PartialViewName = DashboardClient.AdminViewPrefix.Formato("PanelPartViewAdmin");
            grid.AttachFunction = DashboardClient.Module["attachGridControl"](grid,
               Url.Action("AddNewPart", "Dashboard"),
               DashboardClient.PanelPartViews.Keys.Select(t => t.ToJsTypeInfo(isSearch: false, prefix: grid.Prefix)).ToArray());
        }));

            
            #line default
            #line hidden
            
            #line 51 "..\..\Dashboard\Views\Admin\DashboardAdmin.cshtml"
          ;
}

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
