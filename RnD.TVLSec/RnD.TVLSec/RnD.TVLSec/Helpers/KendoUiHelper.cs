using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace RnD.TVLSec.Helpers
{
    public static class KendoUiHelper
    {
        public static string GetKendoUiWindowAjaxSuccessMethod(string status, string messageType, string messageText)
        {
            string strReturn = string.Empty;

            strReturn = status + "|" + messageType + "|" + messageText;

            return strReturn;
        }

        public static string GetKendoUiWindowAjaxSuccessMethod(string status, string actionName, string messageType, string messageText)
        {
            string strReturn = string.Empty;

            strReturn = status + "|" + actionName + "|" + messageType + "|" + messageText;

            return strReturn;
        }

        public static KendoUiGridResult<T> ParseGridData<T>(IQueryable<T> collection, KendoUiGridParam requestParam)
        {
            return ReturnGridData<T>(requestParam, ref collection);
        }

        public static KendoUiGridResult<T> ParseGridData<T>(IQueryable<T> collection, long totalRows)
        {
            return ReturnGridData<T>(ref collection, totalRows);
        }

        private static KendoUiGridResult<T> ReturnGridData<T>(KendoUiGridParam requestParam, ref IQueryable<T> collection)
        {
            List<T> gridData = new List<T>();

            try
            {
                //If the sort Order is provided perform a sort on the specified column
                if (!String.IsNullOrEmpty(requestParam.SortOrd))
                {
                    var sortCollection = Sort<T>(collection, requestParam.SortOn, requestParam.SortOrd);

                    //If sort and paging
                    gridData = sortCollection.Skip(requestParam.Skip).Take(requestParam.PageSize).ToList();
                }
                else
                {
                    //If only paging
                    gridData = collection.Skip(requestParam.Skip).Take(requestParam.PageSize).ToList();
                }
            }
            catch
            {

            }

            return new KendoUiGridResult<T>
            {
                Data = gridData.Count > 0 ? gridData : collection.ToList(),
                Total = collection.Count()
            };
        }

        private static KendoUiGridResult<T> ReturnGridData<T>(ref IQueryable<T> collection, long totalRows)
        {
            List<T> gridData = new List<T>();

            try
            {
                gridData = collection.ToList();
            }
            catch
            {

            }

            return new KendoUiGridResult<T>
            {
                Data = gridData.Count > 0 ? gridData : collection.ToList(),
                Total = Convert.ToInt32(totalRows)
            };
        }

        private static IQueryable<T> Sort<T>(IQueryable<T> collection, string sortOn, string sortOrd)
        {
            var param = Expression.Parameter(typeof(T));

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(Expression.Property(param, sortOn), typeof(object)), param);

            switch (sortOrd.ToLower())
            {
                case "asc":
                    return collection.AsQueryable<T>().OrderBy<T, object>(sortExpression);
                default:
                    return collection.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);

            }
        }

        /// <summary>
        /// Kendo UI Grid Action Links Generate Method
        /// Create By : Rasel Ahmmed Bappi
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="controllerName"></param>
        /// <param name="id"></param>
        /// <param name="isDetailPermitted"></param>
        /// <param name="isEditPermitted"></param>
        /// <param name="isDeletePermitted"></param>
        /// <returns></returns>
        public static string KendoUIGridActionLinkGenerate(string areaName, string controllerName, string id, bool isDetailPermitted = true, bool isEditPermitted = true, bool isDeletePermitted = true)
        {
            string strLink = string.Empty;

            string strDetailsContentUrl = "~/" + areaName + "/" + controllerName + "/Details/" + id;
            string strEditContentUrl = "~/" + areaName + "/" + controllerName + "/Edit/" + id;
            string strDeleteContentUrl = "~/" + areaName + "/" + controllerName + "/Delete/" + id;

            var httpContext = HttpContext.Current;
            var httpContextBase = new HttpContextWrapper(httpContext);

            string urlDetails = UrlHelper.GenerateContentUrl(strDetailsContentUrl, httpContextBase);
            string urlEdit = UrlHelper.GenerateContentUrl(strEditContentUrl, httpContextBase);
            string urlDelete = UrlHelper.GenerateContentUrl(strDeleteContentUrl, httpContextBase);

            if (isDetailPermitted)
            {
                //Details Link
                strLink += @" <a class='lnkDetailCommon btn btn-success btn-sm btn-flat' href='" + urlDetails + "' title='Details' ><i class='fa fa-search'></i></a>";
            }

            if (isEditPermitted)
            {
                //Edit Link
                strLink += @" <a class='lnkEditCommon btn btn-info btn-sm btn-flat' href='" + urlEdit + "' title='Edit' ><i class='fa fa-edit'></i></a>";
            }

            if (isDeletePermitted)
            {
                //Delete Link
                strLink += @" <a class='lnkDeleteCommon btn btn-danger btn-sm btn-flat' href='" + urlDelete + "' title='Delete' ><i class='fa fa-trash-o'></i></a>";
            }

            return strLink;

        }

        /// <summary>
        /// Kendo UI Grid Action Links Generate Method
        /// Create By : Rasel Ahmmed Bappi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDetailPermitted"></param>
        /// <param name="isEditPermitted"></param>
        /// <param name="isDeletePermitted"></param>
        /// <returns></returns>
        public static string KendoUIGridActionLinkGenerate(string id, bool isDetailPermitted = true, bool isEditPermitted = true, bool isDeletePermitted = true)
        {
            string strLink = string.Empty;

            var httpContext = HttpContext.Current;
            var httpContextBase = new HttpContextWrapper(httpContext);

            string areaName = httpContextBase.Request.RequestContext.RouteData.DataTokens.ContainsKey("area") ? httpContextBase.Request.RequestContext.RouteData.DataTokens["area"].ToString() : "";
            string controllerName = httpContextBase.Request.RequestContext.RouteData.Values["controller"].ToString();

            string strDetailsContentUrl = "~/" + areaName + "/" + controllerName + "/Details/" + id;
            string strEditContentUrl = "~/" + areaName + "/" + controllerName + "/Edit/" + id;
            string strDeleteContentUrl = "~/" + areaName + "/" + controllerName + "/Delete/" + id;

            string urlDetails = UrlHelper.GenerateContentUrl(strDetailsContentUrl, httpContextBase);
            string urlEdit = UrlHelper.GenerateContentUrl(strEditContentUrl, httpContextBase);
            string urlDelete = UrlHelper.GenerateContentUrl(strDeleteContentUrl, httpContextBase);

            if (isDetailPermitted)
            {
                //Details Link
                strLink += @" <a class='lnkDetailCommon btn btn-success btn-sm btn-flat' href='" + urlDetails + "' title='Details' ><i class='fa fa-search'></i></a>";
            }

            if (isEditPermitted)
            {
                //Edit Link
                strLink += @" <a class='lnkEditCommon btn btn-info btn-sm btn-flat' href='" + urlEdit + "' title='Edit' ><i class='fa fa-edit'></i></a>";
            }

            if (isDeletePermitted)
            {
                //Delete Link
                strLink += @" <a class='lnkDeleteCommon btn btn-danger btn-sm btn-flat' href='" + urlDelete + "' title='Delete' ><i class='fa fa-trash-o'></i></a>";
            }
            return strLink;

        }


        /// <summary>
        /// Kendo UI Grid Action Links Generate Method
        /// Create By : Rasel Ahmmed Bappi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDetailPermitted"></param>
        /// <param name="isEditPermitted"></param>
        /// <param name="isDeletePermitted"></param>
        /// <returns></returns>
        public static string KendoUIGridActionLinkGenerate2(string id, bool isDetailPermitted = true, bool isEditPermitted = true, bool isDeletePermitted = true)
        {
            string strLink = string.Empty;

            var httpContext = HttpContext.Current;
            var httpContextBase = new HttpContextWrapper(httpContext);

            string areaName = httpContextBase.Request.RequestContext.RouteData.DataTokens.ContainsKey("area") ? httpContextBase.Request.RequestContext.RouteData.DataTokens["area"].ToString() : "";
            string controllerName = httpContextBase.Request.RequestContext.RouteData.Values["controller"].ToString();

            string strDetailsContentUrl = "~/" + areaName + "/" + controllerName + "/Details/" + id;
            string strEditContentUrl = "~/" + areaName + "/" + controllerName + "/Edit/" + id;
            string strDeleteContentUrl = "~/" + areaName + "/" + controllerName + "/Delete/" + id;

            string urlDetails = UrlHelper.GenerateContentUrl(strDetailsContentUrl, httpContextBase);
            string urlEdit = UrlHelper.GenerateContentUrl(strEditContentUrl, httpContextBase);
            string urlDelete = UrlHelper.GenerateContentUrl(strDeleteContentUrl, httpContextBase);

            if (isDetailPermitted)
            {
                //Details Link
                strLink += @" <a class='lnkDetailCommon btn btn-success btn-sm btn-flat' href='" + urlDetails + "' title='Details' ><i class='fa fa-search'></i></a>";
            }

            if (isEditPermitted)
            {
                //Edit Link
                strLink += @" <a class='lnkEditCommon btn btn-info btn-sm btn-flat' href='" + urlEdit + "' title='Edit' ><i class='fa fa-edit'></i></a>";
            }

            if (isDeletePermitted)
            {
                //Delete Link
                strLink += @" <a class='lnkDeleteCommon btn btn-danger btn-sm btn-flat' href='" + urlDelete + "' title='Delete' ><i class='fa fa-trash-o'></i></a>";
            }
            return strLink;

        }

    }

    public class KendoUiGridResult<T>
    {
        public IEnumerable<T> AggregateResults { get; set; }
        public IEnumerable<T> Data { get; set; }
        public IEnumerable<T> Errors { get; set; }
        public int Total { get; set; }
    }

    public class KendoUiGridParam
    {
        public KendoUiGridParam()
        {
            if (HttpContext.Current != null)
            {
                HttpRequest curRequest = HttpContext.Current.Request;

                //this.Page = curRequest["page"].Parse<int>(1);
                //this.PageSize = curRequest["pageSize"].Parse<int>(5);
                //this.Skip = curRequest["skip"].Parse<int>(0);
                //this.Take = curRequest["take"].Parse<int>(5);

                this.Page = Convert.ToInt32(curRequest["page"]) == 0 ? 1 : Convert.ToInt32(curRequest["page"]);
                this.PageSize = Convert.ToInt32(curRequest["pageSize"]) == 0 ? 10 : Convert.ToInt32(curRequest["pageSize"]);
                this.Skip = Convert.ToInt32(curRequest["skip"]) == 0 ? 0 : Convert.ToInt32(curRequest["skip"]);
                this.Take = Convert.ToInt32(curRequest["take"]) == 0 ? 10 : Convert.ToInt32(curRequest["take"]);

                this.SortOrd = curRequest["sort[0][dir]"];
                this.SortOn = curRequest["sort[0][field]"];

                this.FilterLogic = curRequest["filter[logic]"];

                this.FilterField = curRequest["filter[filters][0][field]"];
                this.FilterOperator = curRequest["filter[filters][0][operator]"];
                this.FilterValue = curRequest["filter[filters][0][value]"];

                //this.Export = curRequest["export"];
            }
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        public string SortOrd { get; set; }
        public string SortOn { get; set; }

        public string FilterLogic { get; set; }

        public string FilterField { get; set; }
        public string FilterOperator { get; set; }
        public string FilterValue { get; set; }

        //public string Export { get; set; }
    }
}