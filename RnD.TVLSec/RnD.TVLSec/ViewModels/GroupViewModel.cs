using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RnD.TVLSec.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace RnD.TVLSec.ViewModels
{
    public class GroupViewModel : BaseViewModel
    {
        [Key]
        public int GroupId { get; set; }

        [DisplayName("Name: ")]
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200)]
        public string GroupName { get; set; }

        [DisplayName("Description: ")]
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(200)]
        public string Description { get; set; }

        [DisplayName("Application: ")]
        public int ApplicationId { get; set; }
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual ApplicationViewModel ApplicationViewModel { get; set; }
        public List<SelectListItem> ddlApplications { get; set; }

        [DisplayName("Module: ")]
        public int ModuleId { get; set; }
        [DisplayName("Module Name")]
        public string ModuleName { get; set; }
        [ForeignKey("ModuleId")]
        public virtual ModuleViewModel ModuleViewModel { get; set; }
        public List<SelectListItem> ddlModules { get; set; }

        public List<RoleViewModel> Roles { get; set; }

        public List<KendoTreeviewViewModel> RoleViewModelList { get; set; }

    }
}