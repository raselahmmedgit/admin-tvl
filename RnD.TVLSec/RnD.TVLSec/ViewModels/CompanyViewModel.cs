using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RnD.TVLSec.ViewModels
{
    public class CompanyViewModel : BaseViewModel
    {
        [Key]
        public int CompanyId { get; set; }

        [DisplayName("Name: ")]
        [Required(ErrorMessage = "Company Name is required")]
        [MaxLength(200)]
        public string CompanyName { get; set; }

        [DisplayName("Address: ")]
        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200)]
        public string Address { get; set; }

    }
}