using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using RnD.TVLSec.Helpers;
using RnD.TVLSec.ViewModels;

namespace RnD.TVLSec
{
    public static class HtmlHelperExtension
    {
        #region Message Helpers

        public static IHtmlString RenderMessages(this HtmlHelper htmlHelper)
        {
            var messages = String.Empty;
            foreach (var messageType in Enum.GetNames(typeof(MessageType)))
            {
                var message = htmlHelper.ViewContext.ViewData.ContainsKey(messageType)
                                ? htmlHelper.ViewContext.ViewData[messageType]
                                : htmlHelper.ViewContext.TempData.ContainsKey(messageType)
                                    ? htmlHelper.ViewContext.TempData[messageType]
                                    : null;
                if (message != null)
                {
                    messages += "<div class='" + messageType.ToString().ToLower() + "'>" + message + "</div>";
                }
            }

            return MvcHtmlString.Create(messages);
        }

        #endregion

        #region Wrapper Panels

        #region Primitive Controls

        public static IHtmlString EditorCalenderFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            Func<TModel, TProperty> deleg = expression.Compile();
            var result = deleg(htmlHelper.ViewData.Model);

            string value = null;

            if (result.ToString() == DateTime.MinValue.ToString())
                value = string.Empty;
            else
                value = string.Format("{0:M/dd/yyyy}", result);

            return htmlHelper.TextBoxFor(expression, new { @class = "datepicker text", Value = value });
        }

        public static IHtmlString RequiredSymbolFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string symbol = "* ")
        {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            if (modelMetadata.IsRequired)
            {
                return MvcHtmlString.Create(symbol);
            }

            return MvcHtmlString.Create(string.Empty);
        }

        #endregion

        #region Panel Controls

        public static IHtmlString EditorCalenderPanelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return BuildEditorPanelFor(htmlHelper, expression, htmlHelper.EditorCalenderFor(expression));
        }

        public static IHtmlString EditorPanelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return BuildEditorPanelFor(htmlHelper, expression, htmlHelper.EditorFor(expression));
        }

        public static IHtmlString EditorDropdownPanelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel)
        {
            return BuildEditorPanelFor(htmlHelper, expression, htmlHelper.DropDownListFor(expression, selectList, optionLabel));
        }

        #endregion

        #region Panel Builders

        static IHtmlString BuildEditorPanelFor<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IHtmlString editorForString)
        {
            IHtmlString labelForString = htmlHelper.LabelFor(expression);
            IHtmlString requiredSymbolFor = htmlHelper.RequiredSymbolFor(expression);
            IHtmlString validationMessageForString = htmlHelper.ValidationMessageFor(expression);

            IHtmlString outout = MvcHtmlString.Create(
                labelForString.ToHtmlString() + ":" + requiredSymbolFor.ToHtmlString() + "<br class=\"clear\" />" +
                editorForString.ToHtmlString() + "<br class=\"clear\" />" +
                validationMessageForString.ToHtmlString() + "<br class=\"clear\" />");

            return outout;
        }

        #endregion

        #endregion

        #region Theme

        public static IHtmlString RenderTitle(this HtmlHelper htmlHelper)
        {
            var title = String.Empty;

            var viewDataTitle = htmlHelper.ViewContext.ViewData["Title"] == null ? null : htmlHelper.ViewContext.ViewData["Title"];
            if (viewDataTitle != null)
            {
                var tempTitle = viewDataTitle;
                title += tempTitle;

                htmlHelper.ViewContext.ViewData["Title"] = null;
            }

            return MvcHtmlString.Create(title);
        }

        public static IHtmlString RenderTheme(this HtmlHelper htmlHelper)
        {
            string themePath = String.Empty;
            string themeCookies = String.Empty;

            //string cssPath = @"switcher.css";
            //string cssPath = @"switcher";

            HttpContext httpContext = System.Web.HttpContext.Current;

            if (httpContext.Request.Cookies["CookieTheme"] != null)
            {
                themeCookies += httpContext.Request.Cookies["CookieTheme"].Value.ToString();
            }

            if (!String.IsNullOrEmpty(themeCookies))
            {
                themePath += themeCookies;
            }

            return MvcHtmlString.Create(themePath);
        }

        public static IHtmlString RenderColour(this HtmlHelper htmlHelper)
        {
            string themePath = String.Empty;
            string themeCookies = String.Empty;

            //string cssPath = @"switcher.css";
            //string cssPath = @"switcher";

            HttpContext httpContext = System.Web.HttpContext.Current;

            if (httpContext.Request.Cookies["CookieColour"] != null)
            {
                themeCookies += httpContext.Request.Cookies["CookieColour"].Value.ToString();
            }

            if (!String.IsNullOrEmpty(themeCookies))
            {
                themePath += themeCookies;
            }

            return MvcHtmlString.Create(themePath);
        }

        public static IHtmlString RenderLayout(this HtmlHelper htmlHelper)
        {
            string layoutPath = String.Empty;
            string layoutCookies = String.Empty;

            //string cssPath = @"switcher.css";
            //string cssPath = @"switcher";

            HttpContext httpContext = System.Web.HttpContext.Current;

            if (httpContext.Request.Cookies["CookieLayout"] != null)
            {
                layoutCookies += httpContext.Request.Cookies["CookieLayout"].Value.ToString();
            }

            if (!String.IsNullOrEmpty(layoutCookies))
            {
                layoutPath += layoutCookies;
            }

            return MvcHtmlString.Create(layoutPath);
        }

        public static IHtmlString RenderHeader(this HtmlHelper htmlHelper)
        {
            string headerPath = String.Empty;
            string headerCookies = String.Empty;

            //string cssPath = @"switcher.css";
            //string cssPath = @"switcher";

            HttpContext httpContext = System.Web.HttpContext.Current;

            if (httpContext.Request.Cookies["CookieHeader"] != null)
            {
                headerCookies += httpContext.Request.Cookies["CookieHeader"].Value.ToString();
            }

            if (!String.IsNullOrEmpty(headerCookies))
            {
                headerPath += headerCookies;
            }

            return MvcHtmlString.Create(headerPath);
        }

        public static IHtmlString RenderBackGround(this HtmlHelper htmlHelper)
        {
            string bgPath = String.Empty;
            string bgCookies = String.Empty;

            //string cssPath = @"switcher.css";
            //string cssPath = @"switcher";

            HttpContext httpContext = System.Web.HttpContext.Current;

            if (httpContext.Request.Cookies["CookieBackGround"] != null)
            {
                bgCookies += httpContext.Request.Cookies["CookieBackGround"].Value.ToString();
            }

            if (!String.IsNullOrEmpty(bgCookies))
            {
                bgPath += bgCookies;
            }

            return MvcHtmlString.Create(bgPath);
        }

        #endregion

        #region Breadcrumb

        public static IHtmlString RenderBreadcrumb(this HtmlHelper htmlHelper)
        {
            var strContent = String.Empty;

            var httpContext = HttpContext.Current;
            var httpContextBase = new HttpContextWrapper(httpContext);
            string areaName = httpContextBase.Request.RequestContext.RouteData.DataTokens.ContainsKey("area") ? httpContextBase.Request.RequestContext.RouteData.DataTokens["area"].ToString() : "";
            string controllerName = httpContextBase.Request.RequestContext.RouteData.Values["controller"].ToString();
            string actionName = httpContextBase.Request.RequestContext.RouteData.Values["action"].ToString();

            var headerTitle = controllerName;
            var headerSubTitle = "Information";
            var breadcrumbUrl = "/" + areaName + "/" + controllerName + "/" + actionName;
            var breadcrumbControllerName = controllerName;
            var breadcrumbActionName = actionName;

            strContent += "<section class='content-header'>";

            strContent += "<h1>" + headerTitle;
            strContent += "<small>" + headerSubTitle + "</small>";
            strContent += "</h1>";
            strContent += "<ol class='breadcrumb'>";
            strContent += "<li><a href='" + breadcrumbUrl + "'><i class='fa fa-dashboard'></i> " + breadcrumbControllerName + "</a></li>";
            strContent += "<li class='active'>" + breadcrumbActionName + "</li>";
            strContent += "</ol>";

            strContent += "</section>";

            return MvcHtmlString.Create(strContent);
        }

        #endregion

        #region Menu

        public static IHtmlString RenderMenu(this HtmlHelper htmlHelper)
        {
            var title = String.Empty;



            return MvcHtmlString.Create(title);
        }

        public static IHtmlString RenderSideMenu(this HtmlHelper htmlHelper)
        {
            var strContent = String.Empty;

            var menuList = new List<ThemeMenuViewModel>
            {
                new ThemeMenuViewModel() { Id = 1, ParentId = 1, ChildId = 0, Title = "Dashboard", AreaName = string.Empty, ControllerName = "Home", ActionName = "Index", Url = "", ActionParam = string.Empty , Icon ="fa-dashboard", Badge = string.Empty },

                new ThemeMenuViewModel() { Id = 2, ParentId = 2, ChildId = 0, Title = "Cost/Expense", AreaName = string.Empty, ControllerName = "CostOrExpense", ActionName = "Index", ActionParam = string.Empty , Icon ="fa-th", Badge = "out", BadgeColour = "bg-red" },

                new ThemeMenuViewModel() { Id = 3, ParentId = 3, ChildId = 0, Title = "Sale/Income", AreaName = string.Empty, ControllerName = "SaleOrIncome", ActionName = "Index", ActionParam = string.Empty , Icon ="fa-th", Badge = "in" , BadgeColour="bg-green" },

                new ThemeMenuViewModel() { Id = 4, ParentId = 4, ChildId = 0, Title = "Charts", AreaName = string.Empty, ControllerName = string.Empty, ActionName = string.Empty, ActionParam = string.Empty , Icon ="fa-bar-chart-o", Badge = "paid feature", BadgeColour="bg-yellow", ChildMenus = new List<ThemeMenuViewModel>()
                {
                    new ThemeMenuViewModel() { Id = 5, ParentId = 4, ChildId = 1, Title = "Cost/Expense Charts", AreaName = string.Empty, ControllerName = "Charts", ActionName = "CostOrExpense", ActionParam = string.Empty , Icon =string.Empty, Badge = string.Empty },
                    new ThemeMenuViewModel() { Id = 6, ParentId = 4, ChildId = 2, Title = "Sale/Income Charts", AreaName = string.Empty, ControllerName = "Charts", ActionName = "SaleOrIncome", ActionParam = string.Empty , Icon =string.Empty, Badge = string.Empty },

                }},

                new ThemeMenuViewModel() { Id = 7, ParentId = 7, ChildId = 0, Title = "Settings", AreaName = string.Empty, ControllerName = string.Empty, ActionName = string.Empty, ActionParam = string.Empty , Icon ="fa-gears", Badge = string.Empty, ChildMenus = new List<ThemeMenuViewModel>()
                {
                    new ThemeMenuViewModel() { Id = 8, ParentId = 7, ChildId = 1, Title = "Account", AreaName = string.Empty, ControllerName = "Account", ActionName = "Index", ActionParam = string.Empty , Icon =string.Empty, Badge = string.Empty },
                    new ThemeMenuViewModel() { Id = 9, ParentId = 7, ChildId = 2, Title = "Currency", AreaName = string.Empty, ControllerName = "Currency", ActionName = "Index", ActionParam = string.Empty , Icon =string.Empty, Badge = string.Empty },
                    new ThemeMenuViewModel() { Id = 10, ParentId = 7, ChildId = 3, Title = "Cost/Expense Category", AreaName = string.Empty, ControllerName = "CostOrExpenseCategory", ActionName = "Index", ActionParam = string.Empty , Icon =string.Empty, Badge = string.Empty },
                    new ThemeMenuViewModel() { Id = 11, ParentId = 7, ChildId = 4, Title = "Sale/Income Category", AreaName = string.Empty, ControllerName = "SaleOrIncomeCategory", ActionName = "Index", ActionParam = string.Empty , Icon =string.Empty, Badge = string.Empty },
                    new ThemeMenuViewModel() { Id = 12, ParentId = 7, ChildId = 5, Title = "Settings", AreaName = string.Empty, ControllerName = "AppSetting", ActionName = "Index", ActionParam = string.Empty , Icon =string.Empty, Badge = string.Empty }

                }},

                new ThemeMenuViewModel() { Id = 13, ParentId = 13, ChildId = 0, Title = "Help", AreaName = string.Empty, ControllerName = "Help", ActionName = "Index", ActionParam = string.Empty , Icon ="fa-laptop", Badge = string.Empty },

            };

            var sortMenuList = menuList.OrderBy(x => x.ParentId).ToList();

            strContent += "<ul class='sidebar-menu'>";

            foreach (var menu in sortMenuList)
            {
                if (menu.ChildMenus == null)
                {
                    if (String.IsNullOrEmpty(menu.Badge))
                    {
                        //Without Child Menu and Badge
                        strContent += "<li class='active'>";
                        strContent += "<a href='" + menu.Url + "'><i class='fa " + menu.Icon + "'></i>";
                        strContent += "<span>" + menu.Title + "</span>";
                        strContent += "</a>";
                        strContent += "</li>";
                    }
                    else if (!String.IsNullOrEmpty(menu.Badge))
                    {
                        //Without Child Menu and With Badge
                        strContent += "<li class='active'>";
                        strContent += "<a href='" + menu.Url + "'><i class='fa " + menu.Icon + "'></i>";
                        strContent += "<span>" + menu.Title + "</span>";
                        strContent += "<small class='badge pull-right " + menu.BadgeColour + "'>" + menu.Badge + "</small>";
                        strContent += "</a>";
                        strContent += "</li>";
                    }
                }
                else
                {
                    if (menu.ChildMenus.Count > 0 && String.IsNullOrEmpty(menu.Badge))
                    {
                        //With Child Menu and Without Badge
                        strContent += "<li class='treeview'>";
                        strContent += "<a href='" + menu.Url + "'><i class='fa " + menu.Icon + "'></i>";
                        strContent += "<span>" + menu.Title + "</span>";
                        strContent += "<i class='fa fa-angle-left pull-right'> </i>";
                        strContent += "</a>";

                        //Child Menu
                        strContent += "<ul class='treeview-menu'>";

                        var sortChildMenuList = menu.ChildMenus.OrderBy(x => x.ParentId).ThenBy(x => x.ChildId).ToList();

                        foreach (var childMenu in sortChildMenuList)
                        {
                            //Loop
                            strContent += "<li>";
                            strContent += "<a href='" + childMenu.Url + "'><i class='fa fa-angle-double-right'></i>" + childMenu.Title + "</a>";
                            strContent += "</li>";
                        }

                        strContent += "</ul>";
                        strContent += "</li>";
                    }
                    else if (menu.ChildMenus.Count > 0 && !String.IsNullOrEmpty(menu.Badge))
                    {
                        //With Child Menu and With Badge
                        strContent += "<li class='treeview'>";
                        strContent += "<a href='" + menu.Url + "'><i class='fa " + menu.Icon + "'></i>";
                        strContent += "<span>" + menu.Title + "</span>";
                        strContent += "<i class='fa fa-angle-left pull-right'> </i>";
                        strContent += "<small class='badge pull-right " + menu.BadgeColour + "'>" + menu.Badge + "</small>";
                        strContent += "</a>";
                        //Child Menu
                        strContent += "<ul class='treeview-menu'>";

                        var sortChildMenuList = menu.ChildMenus.OrderBy(x => x.ParentId).ThenBy(x => x.ChildId).ToList();

                        foreach (var childMenu in sortChildMenuList)
                        {
                            //Loop
                            strContent += "<li>";
                            strContent += "<a href='" + childMenu.Url + "'><i class='fa fa-angle-double-right'></i>" + childMenu.Title + "</a>";
                            strContent += "</li>";
                        }

                        strContent += "</ul>";
                        strContent += "</li>";
                    }

                }
            }
            strContent += "</ul>";

            return MvcHtmlString.Create(strContent);
        }

        #endregion

        #region Email Notification

        public static IHtmlString RenderEmailNotify(this HtmlHelper htmlHelper)
        {
            var strContent = String.Empty;

            var totalEmail = 5;
            var emailNotifyList = new List<EmailNotifyViewModel>
            {
                new EmailNotifyViewModel() { EmailSubject = "Support Team", EmailBody = "Why not buy a new awesome theme?", EmailDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()), UserViewModel = new UserViewModel() { UserName = "Rasel", UserPhotoPath = "../../Theme/img/avatar3.png" } },

                new EmailNotifyViewModel() { EmailSubject = "AdminLTE Design Team", EmailBody = "Why not buy a new awesome theme?", EmailDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()), UserViewModel = new UserViewModel() { UserName = "Sohel", UserPhotoPath = "../../Theme/img/avatar2.png" } },

                new EmailNotifyViewModel() { EmailSubject = "Developers", EmailBody = "Why not buy a new awesome theme?", EmailDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()), UserViewModel = new UserViewModel() { UserName = "Shafin", UserPhotoPath = "../../Theme/img/avatar.png" } },

                new EmailNotifyViewModel() { EmailSubject = "Sales Department", EmailBody = "Why not buy a new awesome theme?", EmailDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()), UserViewModel = new UserViewModel() { UserName = "Ahmmed", UserPhotoPath = "../../Theme/img/avatar2.png" } },

                new EmailNotifyViewModel() { EmailSubject = "Reviewers", EmailBody = "Why not buy a new awesome theme?", EmailDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()), UserViewModel = new UserViewModel() { UserName = "Bappi", UserPhotoPath = "../../Theme/img/avatar.png" } },

            };

            var allMessageLink = "";

            strContent += "<a href='" + allMessageLink + "' class='dropdown-toggle' data-toggle='dropdown'>";
            strContent += "<i class='fa fa-envelope'></i><span class='label label-success'>" + totalEmail + "</span>";
            strContent += "</a>";
            strContent += "<ul class='dropdown-menu'>";

            strContent += "<li class='header'>You have" + totalEmail + " messages</li>";
            strContent += "<li>";
            strContent += "<ul class='menu'>";

            //All Email Notify List
            foreach (var emailNotify in emailNotifyList)
            {
                var emailNotifyLink = "";
                strContent += "<li>";
                strContent += "<a href='" + emailNotifyLink + "'>";

                //User Info
                strContent += "<div class='pull-left'>";
                strContent += "<img src='" + emailNotify.UserViewModel.UserPhotoPath + "' class='img-circle' alt='" + emailNotify.UserViewModel.UserName + "' />";
                strContent += "</div>";

                //Subject
                strContent += "<h4>" + emailNotify.EmailSubject;
                strContent += "<small><i class='fa fa-clock-o'></i>" + emailNotify.EmailDate + "</small>";
                strContent += "</h4>";

                //Body
                strContent += "<p>" + emailNotify.EmailBody + "</p>";

                strContent += "</a>";
                strContent += "</li>";
            }

            strContent += "</ul>";
            strContent += "</li>";
            strContent += "<li class='footer'><a href='" + allMessageLink + "'>See All Messages</a></li>";
            strContent += "</ul>";


            return MvcHtmlString.Create(strContent);
        }

        #endregion

        #region Error Massege Notification

        public static IHtmlString RenderAppErrorNotify(this HtmlHelper htmlHelper)
        {
            var strContent = String.Empty;

            var totalError = 5;
            var errorNotifyList = new List<ErrorNotifyViewModel>
            {
                new ErrorNotifyViewModel() { ErrorIcon = "fa-warning", ErrorType = MessageType.warning.ToString(), ErrorMessage = "Very long description here that may not fit into the page and may cause design problems", ErrorDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()) },

                new ErrorNotifyViewModel() { ErrorIcon = "fa-check", ErrorType = MessageType.success.ToString(), ErrorMessage = "25 sales made", ErrorDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()) },

                new ErrorNotifyViewModel() { ErrorIcon = "fa-info", ErrorType = MessageType.info.ToString(), ErrorMessage = "5 new members joined", ErrorDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()) },

                new ErrorNotifyViewModel() { ErrorIcon = "fa-ban", ErrorType = MessageType.danger.ToString(), ErrorMessage = "System has been off", ErrorDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()) },

                new ErrorNotifyViewModel() { ErrorIcon = "fa-warning", ErrorType = MessageType.warning.ToString(), ErrorMessage = "You changed your username", ErrorDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()) },

            };

            var allMessageLink = "";

            strContent += "<a href='" + allMessageLink + "' class='dropdown-toggle' data-toggle='dropdown'>";
            strContent += "<i class='fa fa-warning'></i><span class='label label-warning'>" + totalError + "</span>";
            strContent += "</a>";
            strContent += "<ul class='dropdown-menu'>";

            strContent += "<li class='header'>You have" + totalError + " notifications</li>";
            strContent += "<li>";
            strContent += "<ul class='menu'>";

            //All Email Notify List
            foreach (var errorNotify in errorNotifyList)
            {
                var errorNotifyLink = "";
                strContent += "<li>";
                strContent += "<a href='" + errorNotifyLink + "'>";

                //Error
                strContent += "<i class='fa " + errorNotify.ErrorIcon + " " + errorNotify.ErrorType + "'></i>" + errorNotify.ErrorType;

                strContent += "</a>";
                strContent += "</li>";

            }

            strContent += "</ul>";
            strContent += "</li>";
            strContent += "<li class='footer'><a href='" + allMessageLink + "'>View all</a></li>";
            strContent += "</ul>";


            return MvcHtmlString.Create(strContent);
        }

        #endregion

        #region User Navbar

        public static IHtmlString RenderUserNavbar(this HtmlHelper htmlHelper)
        {
            var strContent = String.Empty;

            var userViewModel = new UserViewModel() { UserName = "Rasel", FullName = "Rasel Ahmmed Bappi", UserPhotoPath = "../../Theme/img/avatar3.png" };


            var userProfileLink = "";
            var userProfileTxt = "Profile";
            var followersLink = "";
            var followersTxt = "Followers";
            var salesLink = "";
            var salesTxt = "Sales";
            var friendsLink = "";
            var friendsTxt = "Friends";
            var userSignOutLink = "";
            var userSignOutTxt = "Sign Out";

            strContent += "<a href='" + userProfileLink + "' class='dropdown-toggle' data-toggle='dropdown'>";
            strContent += "<i class='glyphicon glyphicon-user'></i><span>" + userViewModel.UserName + " <i class='caret'></i></span>";
            strContent += "</a>";
            strContent += "<ul class='dropdown-menu'>";

            //User Info
            strContent += "<li class='user-header bg-light-blue'>";
            strContent += "<img src='" + userViewModel.UserPhotoPath + "' class='img-circle' alt='User Image' />";

            strContent += "<p>";
            strContent += userViewModel.FullName + " - Web Developer <small>Member since Nov. 2012</small>";
            strContent += "</p>";
            strContent += "</li>";

            strContent += "<li class='user-body'>";
            strContent += "<div class='col-xs-4 text-center'>";
            strContent += "<a href='" + followersLink + "'>" + followersTxt + "</a>";
            strContent += "</div>";
            strContent += "<div class='col-xs-4 text-center'>";
            strContent += "<a href='" + salesLink + "'>" + salesTxt + "</a>";
            strContent += "</div>";
            strContent += "<div class='col-xs-4 text-center'>";
            strContent += "<a href='" + friendsLink + "'>" + friendsTxt + "</a>";
            strContent += "</div>";
            strContent += "</li>";

            strContent += "<li class='user-footer'>";
            strContent += "<div class='pull-left'>";
            strContent += "<a href='" + userProfileLink + "' class='btn btn-default btn-flat'>" + userProfileTxt + "</a>";
            strContent += "</div>";
            strContent += "<div class='pull-right'>";
            strContent += "<a href='" + userSignOutLink + "' class='btn btn-default btn-flat'>" + userSignOutTxt + "</a>";
            strContent += "</div>";
            strContent += "</li>";

            strContent += "</ul>";

            return MvcHtmlString.Create(strContent);
        }

        #endregion

        #region User Sidebar

        public static IHtmlString RenderUserSidebar(this HtmlHelper htmlHelper)
        {
            var strContent = String.Empty;

            var userViewModel = new UserViewModel() { UserName = "Rasel", FullName = "Rasel Ahmmed Bappi", UserPhotoPath = "../../Theme/img/avatar3.png" };

            var userProfileLink = "";

            strContent += "<div class='pull-left image'>";
            strContent += "<img src='" + userViewModel.UserPhotoPath + "' class='img-circle' alt='" + userViewModel.UserName + "' />";
            strContent += "</div>";

            strContent += "<div class='pull-left info'>";
            strContent += "<p> Hello, ";
            strContent += userViewModel.UserName;
            strContent += "</p>";
            strContent += "<a href='" + userProfileLink + "'><i class='fa fa-circle text-success'></i>Online</a>";
            strContent += "</div>";


            return MvcHtmlString.Create(strContent);
        }

        #endregion
    }
}