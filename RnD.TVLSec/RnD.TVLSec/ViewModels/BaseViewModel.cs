using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RnD.TVLSec.ViewModels
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            var httpContext = HttpContext.Current;
            var httpContextBase = new HttpContextWrapper(httpContext);
            string areaName = httpContextBase.Request.RequestContext.RouteData.DataTokens.ContainsKey("area") ? httpContextBase.Request.RequestContext.RouteData.DataTokens["area"].ToString() : "";
            string controllerName = httpContextBase.Request.RequestContext.RouteData.Values["controller"].ToString();
            string actionName = httpContextBase.Request.RequestContext.RouteData.Values["action"].ToString();

            this.AreaName = areaName;
            this.ControllerName = controllerName;
            this.ActionName = actionName;
        }

        [NotMapped]
        public string AreaName { get; set; }

        [NotMapped]
        public string ControllerName { get; set; }

        [NotMapped]
        public string ActionName { get; set; }

        [NotMapped]
        public virtual string ActionLink { get; set; }

        [NotMapped]
        public virtual bool HasCreate { get; set; }

        [NotMapped]
        public virtual bool HasUpdate { get; set; }

        [NotMapped]
        public virtual bool HasDelete { get; set; }
    }
}