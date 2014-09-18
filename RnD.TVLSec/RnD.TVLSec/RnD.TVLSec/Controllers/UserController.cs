using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RnD.TVLSec.Helpers;
using RnD.TVLSec.Models;
using System.Data;
using RnD.TVLSec.ViewModels;

namespace RnD.TVLSec.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        private readonly Repository<TblUser> _userRepository;
        private readonly Repository<TblModule> _moduleRepository;
        private readonly Repository<TblApplication> _applicationRepository;

        private readonly Repository<TblEmployee> _employeeRepository;
        private readonly Repository<TblGroup> _groupRepository;

        private readonly Repository<TblRole> _roleRepository;

        #region Constructor

        public UserController(Repository<TblUser> userRepository, Repository<TblModule> moduleRepository, Repository<TblApplication> applicationRepository, Repository<TblEmployee> employeeRepository, Repository<TblGroup> groupRepository, Repository<TblRole> roleRepository)
        {
            this._userRepository = userRepository;
            this._moduleRepository = moduleRepository;
            this._applicationRepository = applicationRepository;

            this._employeeRepository = employeeRepository;
            this._groupRepository = groupRepository;

            this._roleRepository = roleRepository;
        }

        #endregion

        #region Action

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult UserRead(KendoUiGridParam request)
        {
            var userViewModels = GetUserDataList().AsQueryable();
            var models = KendoUiHelper.ParseGridData<UserViewModel>(userViewModels, request);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /User/Details/By ID

        public ActionResult Details(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var user = _userRepository.GetById(id);
                var user = _userRepository.GetAll().SingleOrDefault(x => x.UserId == id);
                if (user != null)
                {

                    var singleOrDefaultGroup = _groupRepository.GetAll().SingleOrDefault(x => x.GroupId == user.GroupId);

                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == user.ApplicationId);

                    var singleOrDefaultEmployee = _employeeRepository.GetAll().SingleOrDefault(x => x.EmployeeId == user.EmployeeId);

                    if (singleOrDefaultApplication != null && singleOrDefaultGroup != null && singleOrDefaultEmployee != null)
                    {
                        var viewModel = new UserViewModel()
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            LoginId = user.LoginId,
                            EmailAddress = user.EmailAddress,
                            Phone = user.Phone,
                            Password = user.Password,
                            LastLoginDate = Convert.ToDateTime(user.LastLoginDate),
                            Status = user.Status,
                            Comment = user.Comment,

                            GroupId = Convert.ToInt32(user.GroupId),
                            GroupName = singleOrDefaultGroup.GroupName,

                            ApplicationId = Convert.ToInt32(user.ApplicationId),
                            ApplicationName = singleOrDefaultApplication.ApplicationName,

                            EmployeeId = Convert.ToInt32(user.EmployeeId),
                            EmployeeName = singleOrDefaultEmployee.EmployeeName
                        };

                        return PartialView("_Details", viewModel);
                    }
                }

                errorViewModel = ExceptionHelper.ExceptionErrorMessageForNullObject();
            }
            catch (Exception ex)
            {
                errorViewModel = ExceptionHelper.ExceptionErrorMessageFormat(ex);
            }

            return PartialView("_ErrorPopup", errorViewModel);
        }

        //
        // GET: /User/Add

        public ActionResult Add()
        {
            var groupList = SelectListItemExtension.PopulateDropdownList(_groupRepository.GetAll().ToList<TblGroup>(), "GroupId", "GroupName").ToList();

            var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName").ToList();

            var moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().ToList<TblModule>(), "ModuleId", "ModuleTitle").ToList();

            var employeeList = SelectListItemExtension.PopulateDropdownList(_employeeRepository.GetAll().ToList<TblEmployee>(), "EmployeeId", "EmployeeName").ToList();

            var roleList = _roleRepository.GetAll().Select(x => new KendoTreeviewViewModel { Id = x.RoleId.ToString(), Text = x.RoleName }).ToList();

            var viewModel = new UserViewModel() { UserId = 0, ddlApplications = applicationList, ddlModules = moduleList, ddlGroups = groupList, ddlEmployees = employeeList, RoleTreeViewModelList = roleList };

            //return View();
            return PartialView("_AddOrEdit", viewModel);
        }

        public JsonResult GetApplicationList()
        {
            //var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName").ToList();

            var applicationList = _applicationRepository.GetAll().Select(x => new { Text = x.ApplicationName, Value = x.ApplicationId.ToString() }).ToList();

            return Json(applicationList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModuleList(KendoDropDownParamViewModel model)
        {
            var moduleList = new List<SelectListItem>();

            if (model.Value == null)
            {
                moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().ToList<TblModule>(), "ModuleId", "ModuleTitle").ToList();
            }
            else
            {
                moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().Where(x => x.ApplicationId == Convert.ToInt32(model.Value)).ToList<TblModule>(), "ModuleId", "ModuleTitle").ToList();
            }

            return Json(moduleList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoleList(KendoTreeviewParamViewModel model)
        {
            var roleList = new List<KendoTreeviewViewModel>();

            var commonChildList = new List<KendoTreeviewViewModel>()
                                                {
                                                    new KendoTreeviewViewModel { Id = "1", Text = "Add", IsChecked = Boolean.FalseString, ParentId = "1" },
                                                    new KendoTreeviewViewModel { Id = "2", Text = "Edit", IsChecked = Boolean.FalseString, ParentId = "1" },
                                                    new KendoTreeviewViewModel { Id = "3", Text = "Delete", IsChecked = Boolean.FalseString, ParentId = "1" },
                                                   new KendoTreeviewViewModel { Id = "4", Text = "Cancel", IsChecked = Boolean.FalseString, ParentId = "1" },
                                                    new KendoTreeviewViewModel { Id = "5", Text = "Print", IsChecked = Boolean.FalseString, ParentId = "1" }
                                                };

            if (model.ApplicationId == null && model.ModuleId == null)
            {
                roleList = _roleRepository.GetAll().Select(x => new KendoTreeviewViewModel { Id = x.RoleId.ToString(), Text = x.RoleName, IsChecked = Boolean.FalseString, ParentId = null, Items = commonChildList }).ToList();
            }
            else
            {
                roleList = _roleRepository.GetAll().Where(x => x.ApplicationId == Convert.ToInt32(model.ApplicationId) && x.ModuleId == Convert.ToInt32(model.ModuleId)).Select(x => new KendoTreeviewViewModel { Id = x.RoleId.ToString(), Text = x.RoleName, IsChecked = Boolean.FalseString, ParentId = null, Items = commonChildList }).ToList();
            }

            return Json(roleList, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /User/Edit/By ID

        public ActionResult Edit(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var user = _userRepository.GetById(id);
                var user = _userRepository.GetAll().SingleOrDefault(x => x.UserId == id);
                if (user != null)
                {
                    var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName", isEdit: true, selectedValue: user.ApplicationId.ToString()).ToList();

                    var moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().ToList<TblModule>(), "ModuleId", "ModuleName").ToList();

                    var groupList = SelectListItemExtension.PopulateDropdownList(_groupRepository.GetAll().ToList<TblGroup>(), "GroupId", "GroupName", isEdit: true, selectedValue: user.GroupId.ToString()).ToList();

                    var employeeList = SelectListItemExtension.PopulateDropdownList(_employeeRepository.GetAll().ToList<TblEmployee>(), "EmployeeId", "EmployeeName", isEdit: true, selectedValue: user.EmployeeId.ToString()).ToList();


                    var roleList = _roleRepository.GetAll().Select(x => new KendoTreeviewViewModel { Id = x.RoleId.ToString(), Text = x.RoleName }).ToList();

                    var singleOrDefaultGroup = _groupRepository.GetAll().SingleOrDefault(x => x.GroupId == user.GroupId);

                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == user.ApplicationId);

                    var singleOrDefaultEmployee = _employeeRepository.GetAll().SingleOrDefault(x => x.EmployeeId == user.EmployeeId);

                    if (singleOrDefaultApplication != null && singleOrDefaultGroup != null && singleOrDefaultEmployee != null)
                    {
                        var viewModel = new UserViewModel()
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            LoginId = user.LoginId,
                            EmailAddress = user.EmailAddress,
                            Phone = user.Phone,
                            Password = user.Password,
                            LastLoginDate = Convert.ToDateTime(user.LastLoginDate),
                            Status = user.Status,
                            Comment = user.Comment,

                            GroupId = Convert.ToInt32(user.GroupId),
                            GroupName = singleOrDefaultGroup.GroupName,

                            ApplicationId = Convert.ToInt32(user.ApplicationId),
                            ApplicationName = singleOrDefaultApplication.ApplicationName,

                            EmployeeId = Convert.ToInt32(user.EmployeeId),
                            EmployeeName = singleOrDefaultEmployee.EmployeeName,

                            ddlApplications = applicationList,
                            ddlModules = moduleList,
                            ddlGroups = groupList,
                            ddlEmployees = employeeList,
                            RoleTreeViewModelList = roleList
                        };

                        return PartialView("_AddOrEdit", viewModel);
                    }
                }

                errorViewModel = ExceptionHelper.ExceptionErrorMessageForNullObject();
            }
            catch (Exception ex)
            {
                errorViewModel = ExceptionHelper.ExceptionErrorMessageFormat(ex);
            }

            return PartialView("_ErrorPopup", errorViewModel);
        }

        //
        // POST: /User/Save

        [HttpPost]
        public ActionResult Save(UserViewModel userViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //add
                    if (userViewModel.UserId == 0 && userViewModel.ActionName == "Add")
                    {
                        var model = new TblUser()
                        {
                            LoginId = userViewModel.LoginId,
                            Password = userViewModel.Password,
                            UserName = userViewModel.UserName,
                            EmailAddress = userViewModel.EmailAddress,
                            Phone = userViewModel.Phone,
                            ChangePasswordAtFirstLogin = userViewModel.ChangePasswordAtFirstLogin,
                            Status = userViewModel.Status,

                            GroupId = userViewModel.GroupId,
                            ApplicationId = userViewModel.ApplicationId,
                            EmployeeId = userViewModel.EmployeeId
                        };

                        //_userRepository.Insert(model);
                    }
                    else if (userViewModel.ActionName == "Edit") //edit
                    {
                        TblUser user = _userRepository.GetById(userViewModel.UserId);

                        if (user != null)
                        {

                            user.UserId = userViewModel.UserId;
                            user.LoginId = userViewModel.LoginId;
                            user.Password = userViewModel.Password;
                            user.UserName = userViewModel.UserName;
                            user.EmailAddress = userViewModel.EmailAddress;
                            user.Phone = userViewModel.Phone;
                            user.ChangePasswordAtFirstLogin = userViewModel.ChangePasswordAtFirstLogin;
                            user.Status = userViewModel.Status;

                            user.ApplicationId = userViewModel.ApplicationId;
                            user.GroupId = userViewModel.GroupId;
                            user.EmployeeId = userViewModel.EmployeeId;

                            //_userRepository.Update(user);

                        }
                        else
                        {
                            return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, userViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageForNullObject()));
                        }

                    }

                    //_userRepository.Save();

                    return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.TrueString, userViewModel.ActionName, MessageType.success.ToString(), "Saved Successfully."));

                }

                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, userViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ModelStateErrorFormat(ModelState)));
            }
            catch (Exception ex)
            {
                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, userViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageFormat(ex)));
            }
        }

        //
        // POST: /User/Delete/By ID
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                //var user = _userRepository.GetById(id);
                var user = _userRepository.GetAll().SingleOrDefault(x => x.UserId == id);
                if (user != null)
                {
                    _userRepository.Delete(user);
                    _userRepository.Save();

                    return Json(new { status = Boolean.FalseString, messageType = MessageType.success.ToString(), messageText = "Deleted Successfully." }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { status = Boolean.FalseString, messageType = MessageType.warning.ToString(), messageText = ExceptionHelper.ExceptionMessageForNullObject() }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = Boolean.FalseString, messageType = MessageType.danger.ToString(), messageText = ExceptionHelper.ExceptionMessageFormat(ex) }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Method

        private List<UserViewModel> GetUserDataList()
        {
            var dataList = _userRepository.GetAll().ToList().Select(c => new TblUser
            {
                UserId = c.UserId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                UserName = c.UserName,
                LoginId = c.LoginId,
                EmailAddress = c.EmailAddress,
                Phone = c.Phone,
                Password = c.Password,
                LastLoginDate = c.LastLoginDate,
                Status = c.Status,
                ChangePasswordAtFirstLogin = c.ChangePasswordAtFirstLogin,
                Comment = c.Comment,
                GroupId = c.GroupId,
                ApplicationId = c.ApplicationId,
                EmployeeId = c.EmployeeId

            });

            var viewModels = dataList.Select(
                md =>
                {
                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == md.ApplicationId);
                    return singleOrDefaultApplication != null ? new UserViewModel
                          {
                              UserId = md.UserId,
                              FirstName = md.FirstName,
                              LastName = md.LastName,
                              UserName = md.UserName,
                              LoginId = md.LoginId,
                              EmailAddress = md.EmailAddress,
                              Phone = md.Phone,
                              Password = md.Password,
                              LastLoginDate = Convert.ToDateTime(md.LastLoginDate),
                              Status = md.Status,
                              ChangePasswordAtFirstLogin = Convert.ToBoolean(md.ChangePasswordAtFirstLogin),
                              Comment = md.Comment,

                              GroupId = Convert.ToInt32(md.GroupId),
                              GroupName = _groupRepository.GetAll().SingleOrDefault(x => x.GroupId == md.GroupId).GroupName,

                              ApplicationId = Convert.ToInt32(md.ApplicationId),
                              ApplicationName = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == md.ApplicationId).ApplicationName,

                              EmployeeId = Convert.ToInt32(md.EmployeeId),
                              EmployeeName = _employeeRepository.GetAll().SingleOrDefault(x => x.EmployeeId == md.EmployeeId).EmployeeName,

                              ActionLink = KendoUiHelper.KendoUIGridActionLinkGenerate(md.UserId.ToString())
                          } : null;
                }).OrderBy(o => o.UserName).ToList();

            return viewModels;
        }

        #endregion

    }
}
