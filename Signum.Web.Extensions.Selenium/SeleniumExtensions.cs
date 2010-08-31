﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Selenium;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Signum.Web.Selenium
{
    public static class SeleniumExtensions
    {
        public static Process LaunchSeleniumProcess()
        {
            Process seleniumServerProcess = new Process();
            seleniumServerProcess.StartInfo.FileName = "java";
            if (System.IO.Directory.Exists("D:\\Signum\\Selenium"))
                seleniumServerProcess.StartInfo.Arguments =
                    "-jar c:/selenium/selenium-server.jar -firefoxProfileTemplate D:\\Signum\\Selenium";
            else
                seleniumServerProcess.StartInfo.Arguments =
                    "-jar c:/selenium/selenium-server.jar";

            seleniumServerProcess.Start();
            return seleniumServerProcess;
        }

        public static ISelenium InitializeSelenium()
        {
            ISelenium selenium = new DefaultSelenium("localhost", 4444, "*iexplore", "http://localhost/");
            selenium.Start();
            selenium.SetSpeed("1000");
            selenium.SetTimeout("600000");

            selenium.AddLocationStrategy("jq",
            "var loc = locator; " +
            "var attr = null; " +
            "var isattr = false; " +
            "var inx = locator.lastIndexOf('@'); " +

            "if (inx != -1){ " +
            "   loc = locator.substring(0, inx); " +
            "   attr = locator.substring(inx + 1); " +
            "   isattr = true; " +
            "} " +

            "var found = jQuery(inDocument).find(loc); " +
            "if (found.length >= 1) { " +
            "   if (isattr) { " +
            "       return found[0].getAttribute(attr); " +
            "   } else { " +
            "       return found[0]; " +
            "   } " +
            "} else { " +
            "   return null; " +
            "}"
        );

            return selenium;
        }

        public static void KillSelenium(Process seleniumProcess)
        {
            if (seleniumProcess != null && !seleniumProcess.HasExited)
                seleniumProcess.Kill();
            
            //Kill java process so it frees application folder and the next build can delete it
            foreach (var p in Process.GetProcessesByName("java").Where(proc => !proc.HasExited))
                p.Dispose();

            //Kill IIS worker process so it frees application folder and the next build can delete it
            foreach (var p in Process.GetProcessesByName("w3wp").Where(proc => !proc.HasExited))
                p.Dispose();
        }

        public const string DefaultPageLoadTimeout = "100000"; //1.66666667 minutes

        public const int DefaultAjaxTimeout = 100000;

        public static void WaitAjaxFinished(this ISelenium selenium, Func<bool> condition)
        {
            WaitAjaxFinished(selenium, condition, DefaultAjaxTimeout);
        }

        public static void WaitAjaxFinished(this ISelenium selenium, Func<bool> condition, int timeout)
        {
            DateTime now = DateTime.Now;
            while (DateTime.Now < now.AddMilliseconds(timeout) && !condition())
                Thread.Sleep(1000);
            Assert.IsTrue(condition());
        }
    }
}