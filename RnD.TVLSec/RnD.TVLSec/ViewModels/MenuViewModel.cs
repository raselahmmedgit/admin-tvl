using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace RnD.TVLSec.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        [Key]
        public int MenuId { get; set; }

        [DisplayName("Name: ")]
        [Required(ErrorMessage = "Menu Name is required")]
        [MaxLength(200)]
        public string MenuName { get; set; }

        [DisplayName("Caption: ")]
        public string MenuCaption { get; set; }

        [DisplayName("Caption Image: ")]
        public string MenuCaptionBng { get; set; }

        [DisplayName("Icon: ")]
        public string MenuIcon { get; set; }

        [DisplayName("Page Url: ")]
        public string PageUrl { get; set; }

        [DisplayName("Serial No: ")]
        public int SerialNo { get; set; }

        [DisplayName("Order No: ")]
        public int OrderNo { get; set; }

        [DisplayName("Parent Menu: ")]
        //[Required(ErrorMessage = "Select Parent Menu.")]
        //[Range(1, long.MaxValue, ErrorMessage = "Select Parent Menu.")]
        public int ParentMenuId { get; set; }
        [DisplayName("Parent Menu Name")]
        public string ParentMenuName { get; set; }
        [ForeignKey("ParentMenuId")]
        public virtual MenuViewModel ParentMenuViewModel { get; set; }
        public List<SelectListItem> ddlParentMenus { get; set; }

        [DisplayName("Application")]
        [Required(ErrorMessage = "Select Application.")]
        [Range(1, long.MaxValue, ErrorMessage = "Select Application.")]
        public int ApplicationId { get; set; }
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual ApplicationViewModel ApplicationViewModel { get; set; }
        public List<SelectListItem> ddlApplications { get; set; }

        [DisplayName("Module")]
        [Required(ErrorMessage = "Select Module.")]
        [Range(1, long.MaxValue, ErrorMessage = "Select Module.")]
        public int ModuleId { get; set; }
        [DisplayName("Module Name")]
        public string ModuleName { get; set; }
        [ForeignKey("ModuleId")]
        public virtual ModuleViewModel ModuleViewModel { get; set; }
        public List<SelectListItem> ddlModules { get; set; }
    }
}