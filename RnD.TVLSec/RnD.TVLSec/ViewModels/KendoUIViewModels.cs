using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RnD.TVLSec.ViewModels
{
    public class KendoDropDownParamViewModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class KendoTreeviewParamViewModel
    {
        public string ApplicationId { get; set; }
        public string ModuleId { get; set; }
    }

    public class KendoTreeviewViewModel
    {
        //public string Id { get; set; }
        //public string Text { get; set; }
        //public List<KendoTreeviewViewModel> Items { get; set; }

        public string Id { get; set; }
        public string Text { get; set; }
        public string IsChecked { get; set; }
        public string ParentId { get; set; }
        public List<KendoTreeviewViewModel> Items { get; set; }
    }
}