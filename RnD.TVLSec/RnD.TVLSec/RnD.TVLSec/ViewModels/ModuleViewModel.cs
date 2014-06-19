using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace RnD.TVLSec.ViewModels
{
    public class ModuleViewModel : BaseViewModel
    {
        [Key]
        public int ModuleId { get; set; }

        [DisplayName("Name: ")]
        [Required(ErrorMessage = "Application Name is required")]
        [MaxLength(200)]
        public string ModuleName { get; set; }

        [DisplayName("Description: ")]
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(200)]
        public string Description { get; set; }

        [DisplayName("Title: ")]
        [Required(ErrorMessage = "Module Title is required")]
        [MaxLength(200)]
        public string ModuleTitle { get; set; }

        [DisplayName("Application: ")]
        [Required(ErrorMessage = "Select Application.")]
        [Range(1, long.MaxValue, ErrorMessage = "Select Application.")]
        public int ApplicationId { get; set; }
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }
        [ForeignKey("ApplicationId")]
        public virtual ApplicationViewModel ApplicationViewModel { get; set; }
        public List<SelectListItem> ddlApplications { get; set; }
    }
}