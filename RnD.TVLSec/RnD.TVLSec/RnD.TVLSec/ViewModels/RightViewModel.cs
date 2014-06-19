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
    public class RightViewModel : BaseViewModel
    {
        [Key]
        public int RightId { get; set; }

        [DisplayName("Title: ")]
        [Required(ErrorMessage = "Right Title is required")]
        [MaxLength(200)]
        public string RightTitle { get; set; }

        [DisplayName("Description: ")]
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(200)]
        public string Description { get; set; }

        [DisplayName("Name: ")]
        [Required(ErrorMessage = "Right Name is required")]
        [MaxLength(200)]
        public string RightName { get; set; }

        [DisplayName("Application: ")]
        [Required(ErrorMessage = "Select Application.")]
        [Range(1, long.MaxValue, ErrorMessage = "Select Application.")]
        public int ApplicationId { get; set; }
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual ApplicationViewModel ApplicationViewModel { get; set; }
        public List<SelectListItem> ddlApplications { get; set; }

        [DisplayName("Module: ")]
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