using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RnD.TVLSec.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        [Key]
        public int ApplicationId { get; set; }

        [DisplayName("Name: ")]
        [Required(ErrorMessage = "Application Name is required")]
        [MaxLength(200)]
        public string ApplicationName { get; set; }

        [DisplayName("Description: ")]
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(200)]
        public string Description { get; set; }

        [DisplayName("Title: ")]
        [Required(ErrorMessage = "Application Title is required")]
        [MaxLength(200)]
        public string ApplicationTitle { get; set; }
    }
}