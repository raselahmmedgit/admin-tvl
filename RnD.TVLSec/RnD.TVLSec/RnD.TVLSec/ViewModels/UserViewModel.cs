using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace RnD.TVLSec.ViewModels
{
    public class UserViewModel : BaseViewModel
    {

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string UserPhotoPath { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string LoginId { get; set; }

        public string EmailAddress { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public string NeverExperied { get; set; }

        public DateTime LastLoginDate { get; set; }
        
        public bool Status { get; set; }

        public bool IsLockedOut { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }

        public DateTime LastLockoutDate { get; set; }

        public int FailedPasswordAttemptCount { get; set; }

        public string Comment { get; set; }

        public bool ChangePasswordAtFirstLogin { get; set; }

        [DisplayName("Group: ")]
        [Required(ErrorMessage = "Select Group.")]
        [Range(1, long.MaxValue, ErrorMessage = "Select Group.")]
        public int GroupId { get; set; }
        [DisplayName("Group Name")]
        public string GroupName { get; set; }
        [ForeignKey("GroupId")]
        public virtual GroupViewModel GroupViewModel { get; set; }
        public List<SelectListItem> ddlGroups { get; set; }

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
        //[Required(ErrorMessage = "Select Module.")]
        //[Range(1, long.MaxValue, ErrorMessage = "Select Module.")]
        public int ModuleId { get; set; }
        [DisplayName("Module Name")]
        public string ModuleName { get; set; }
        [ForeignKey("ModuleId")]
        public virtual ModuleViewModel ModuleViewModel { get; set; }
        public List<SelectListItem> ddlModules { get; set; }

        [DisplayName("Employee: ")]
        [Required(ErrorMessage = "Select Employee.")]
        [Range(1, long.MaxValue, ErrorMessage = "Select Employee.")]
        public int EmployeeId { get; set; }
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual EmployeeViewModel EmployeeViewModel { get; set; }
        public List<SelectListItem> ddlEmployees { get; set; }

        public List<RoleViewModel> Roles { get; set; }

        public List<KendoTreeviewViewModel> RoleTreeViewModelList { get; set; }

    }
}