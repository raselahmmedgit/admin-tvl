using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RnD.TVLSec.Helpers
{
    public static class ControllerExtension
    {
        #region Messsage Extensions

        public static void ShowMessage(this Controller controller, string message, MessageType messageType = MessageType.info, bool showAfterRedirect = true)
        {
            var messageTypeKey = messageType.ToString();
            if (showAfterRedirect)
            {
                controller.TempData[messageTypeKey] = message;
            }
            else
            {
                controller.ViewData[messageTypeKey] = message;
            }
        }

        #endregion

        #region Theme

        public static void ShowTitle(this Controller controller, string title)
        {
            title += "RaselBappi";
            controller.ViewData["Title"] = title;
            //controller.ViewBag.Title = title;
        }

        public static void SetTheme(this Controller controller, string cssFile, bool themeChange = false)
        {
            string cssPath = string.Empty;

            HttpCookie cookie = new HttpCookie("CookieTheme");

            if (themeChange)
            {
                //set selected theme
                cssPath += cssFile;
                cookie.Value = cssPath;
            }
            else
            {
                //set default theme
                //cssPath += @"switcher.css";
                cookie.Value = cssPath;
            }

            controller.HttpContext.Response.Cookies.Add(cookie);

        }

        public static void SetColour(this Controller controller, string cssFile, bool colourChange = false)
        {
            string cssPath = string.Empty;

            HttpCookie cookie = new HttpCookie("CookieColour");

            if (colourChange)
            {
                //set selected theme
                cssPath += cssFile;
                cookie.Value = cssPath;
            }
            else
            {
                //set default theme
                //cssPath += @"switcher.css";
                cookie.Value = cssPath;
            }

            controller.HttpContext.Response.Cookies.Add(cookie);

        }

        public static void SetLayout(this Controller controller, string cssFile, bool layoutChange = false)
        {
            string cssPath = string.Empty;

            HttpCookie cookie = new HttpCookie("CookieLayout");

            if (layoutChange)
            {
                //set selected theme
                cssPath += cssFile;
                cookie.Value = cssPath;
            }
            else
            {
                //set default theme
                //cssPath += @"switcher.css";
                cookie.Value = cssPath;
            }

            controller.HttpContext.Response.Cookies.Add(cookie);

        }

        public static void SetHeader(this Controller controller, string cssFile, bool headerChange = false)
        {
            string cssPath = string.Empty;

            HttpCookie cookie = new HttpCookie("CookieHeader");

            if (headerChange)
            {
                //set selected theme
                cssPath += cssFile;
                cookie.Value = cssPath;
            }
            else
            {
                //set default theme
                //cssPath += @"switcher.css";
                cookie.Value = cssPath;
            }

            controller.HttpContext.Response.Cookies.Add(cookie);

        }

        public static void SetBackGround(this Controller controller, string cssFile, bool bgChange = false)
        {
            string cssPath = string.Empty;

            HttpCookie cookie = new HttpCookie("CookieBackGround");

            if (bgChange)
            {
                //set selected theme
                cssPath += cssFile;
                cookie.Value = cssPath;
            }
            else
            {
                //set default theme
                //cssPath += @"switcher.css";
                cookie.Value = cssPath;
            }

            controller.HttpContext.Response.Cookies.Add(cookie);

        }

        #endregion
    }
}