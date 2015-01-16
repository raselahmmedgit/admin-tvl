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


        [DisplayName("Company: ")]
        [Required(ErrorMessage = "Select Company.")]
        [Range(1, long.MaxValue, ErrorMessage = "Select Company.")]
        public int CompanyId { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyViewModel CompanyViewModel { get; set; }
        public List<SelectListItem> ddlCompanies { get; set; }
    }
}