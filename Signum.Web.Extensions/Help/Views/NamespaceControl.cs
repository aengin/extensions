﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
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
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Signum.Utilities;
    using Signum.Entities;
    using Signum.Web;
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
    using Signum.Entities.Reflection;
    using Signum.Engine.Help;
    using Signum.Web.Help;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Help/Views/NamespaceControl.cshtml")]
    public class _Page_Help_Views_NamespaceControl_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
#line hidden

        public _Page_Help_Views_NamespaceControl_cshtml()
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




   NamespaceModel nm = (NamespaceModel)Model;

WriteLiteral("<ul>\r\n    <li>\r\n");


         if (nm.Types.Count > 0)
        {

WriteLiteral("            <h2>");


           Write(Html.ActionLink(nm.ShortNamespace, "Namespace", new { @namespace = nm.Namespace }));

WriteLiteral("</h2>\r\n");


        }
        else
        {

WriteLiteral("            <h2>");


           Write(nm.ShortNamespace);

WriteLiteral("</h2>           \r\n");


        }


         if (nm.Types.Count > 0)
        {  

WriteLiteral("            <ul>\r\n");


                 foreach (Type type in nm.Types)
                {
                    string urlName = HelpLogic.EntityUrl(type);
                    string niceName = type.NiceName();

WriteLiteral("                    <li><a href=\"");


                            Write(urlName);

WriteLiteral("\">");


                                      Write(niceName);

WriteLiteral("</a> </li>\r\n");


                }

WriteLiteral("            </ul>\r\n");


        }


         if (nm.Namespaces.Count > 0)
        {
            foreach (NamespaceModel item in nm.Namespaces)
            {
                Html.RenderPartial(HelpClient.NamespaceControlUrl, item);
            }
        }
WriteLiteral(" </li>\r\n</ul>\r\n");


        }
    }
}