using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RnD.TVLSec.ViewModels
{
    public class ThemeMenuViewModel
    {
        public ThemeMenuViewModel()
        {
            string strUrl = string.Empty;

            if (!String.IsNullOrEmpty(this.AreaName))
            {
                strUrl += "/" + this.AreaName;
            }
            else if (!String.IsNullOrEmpty(this.ControllerName))
            {
                strUrl += "/" + this.ControllerName;
            }
            else if (!String.IsNullOrEmpty(this.ActionName))
            {
                strUrl += "/" + ActionName;
            }
            else if (!String.IsNullOrEmpty(this.ActionParam))
            {
                strUrl += "/" + ActionParam;
            }

            this.Url = strUrl;
        }

        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public string Title { get; set; }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ActionParam { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Badge { get; set; }
        public string BadgeColour { get; set; }

        public List<ThemeMenuViewModel> ChildMenus { get; set; }


    }
}