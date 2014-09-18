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
    public class RoleController : Controller
    {
        private readonly Repository<TblRole> _roleRepository;
        private readonly Repository<TblModule> _moduleRepository;
        private readonly Repository<TblApplication> _applicationRepository;

        private readonly Repository<TblMenu> _menuRepository;
        private readonly Repository<TblRight> _rightRepository;

        #region Constructor

        public RoleController(Repository<TblRole> roleRepository, Repository<TblModule> moduleRepository, Repository<TblApplication> applicationRepository, Repository<TblMenu> menuRepository, Repository<TblRight> rightRepository)
        {
            this._roleRepository = roleRepository;
            this._moduleRepository = moduleRepository;
            this._applicationRepository = applicationRepository;

            this._menuRepository = menuRepository;
            this._rightRepository = rightRepository;
        }

        #endregion

        #region Action

        //
        // GET: /Role/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult RoleRead(KendoUiGridParam request)
        {
            var roleViewModels = GetRoleDataList().AsQueryable();
            var models = KendoUiHelper.ParseGridData<RoleViewModel>(roleViewModels, request);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Role/Details/By ID

        public ActionResult Details(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var role = _roleRepository.GetById(id);
                var role = _roleRepository.GetAll().SingleOrDefault(x => x.RoleId == id);
                if (role != null)
                {

                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == role.ApplicationId);

                    var singleOrDefaultModule = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == role.ModuleId);

                    if (singleOrDefaultApplication != null && singleOrDefaultModule != null)
                    {
                        var viewModel = new RoleViewModel() { RoleId = role.RoleId, RoleName = role.RoleName, Description = role.Description, ApplicationId = Convert.ToInt32(role.ApplicationId), ApplicationName = singleOrDefaultApplication.ApplicationName, ModuleId = Convert.ToInt32(role.ModuleId), ModuleName = singleOrDefaultModule.ModuleName };

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
        // GET: /Role/Add

        public ActionResult Add()
        {
            var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName").ToList();

            var moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().ToList<TblModule>(), "ModuleId", "ModuleTitle").ToList();

            var menuList = _menuRepository.GetAll().Select(x => new KendoTreeviewViewModel { Id = x.MenuId.ToString(), Text = x.MenuName }).ToList();

            var rightList = _rightRepository.GetAll().Select(x => new KendoTreeviewViewModel { Id = x.RightId.ToString(), Text = x.RightName }).ToList();

            var viewModel = new RoleViewModel() { RoleId = 0, ddlApplications = applicationList, ddlModules = moduleList, MenuTreeViewModelList = menuList, RightTreeViewModelList = rightList };

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

        public JsonResult GetMenuList(KendoTreeviewParamViewModel model)
        {
            var menuList = new List<KendoTreeviewViewModel>();

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
                menuList = _menuRepository.GetAll().Select(x => new KendoTreeviewViewModel { Id = x.MenuId.ToString(), Text = x.MenuName, IsChecked = Boolean.FalseString, ParentId = null, Items = commonChildList }).ToList();
            }
            else
            {
                menuList = _menuRepository.GetAll().Where(x => x.ApplicationId == Convert.ToInt32(model.ApplicationId) && x.ModuleId == Convert.ToInt32(model.ModuleId)).Select(x => new KendoTreeviewViewModel { Id = x.MenuId.ToString(), Text = x.MenuName, IsChecked = Boolean.FalseString, ParentId = null, Items = commonChildList }).ToList();
            }

            return Json(menuList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRightList(KendoTreeviewParamViewModel model)
        {
            var rightList = new List<KendoTreeviewViewModel>();

            if (model.ApplicationId == null && model.ModuleId == null)
            {
                rightList = _rightRepository.GetAll().Select(x => new KendoTreeviewViewModel { Id = x.RightId.ToString(), Text = x.RightName, IsChecked = Boolean.FalseString, ParentId = null }).ToList();
            }
            else
            {
                rightList = _rightRepository.GetAll().Where(x => x.ApplicationId == Convert.ToInt32(model.ApplicationId) && x.ModuleId == Convert.ToInt32(model.ModuleId)).Select(x => new KendoTreeviewViewModel { Id = x.RightId.ToString(), Text = x.RightName, IsChecked = Boolean.FalseString, ParentId = null }).ToList();
            }

            return Json(rightList, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Role/Edit/By ID

        public ActionResult Edit(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var role = _roleRepository.GetById(id);
                var role = _roleRepository.GetAll().SingleOrDefault(x => x.RoleId == id);
                if (role != null)
                {

                    var moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().ToList<TblModule>(), "ModuleId", "ModuleName", isEdit: true, selectedValue: role.ModuleId.ToString()).ToList();

                    var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName", isEdit: true, selectedValue: role.ApplicationId.ToString()).ToList();


                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == role.ApplicationId);

                    var singleOrDefaultModule = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == role.ModuleId);

                    if (singleOrDefaultApplication != null && singleOrDefaultModule != null)
                    {

                        var viewModel = new RoleViewModel()
                        {
                            RoleId = role.RoleId,
                            RoleName = role.RoleName,
                            Description = role.Description,
                            ApplicationId = Convert.ToInt32(role.ApplicationId),
                            ApplicationName = singleOrDefaultApplication != null ? singleOrDefaultApplication.ApplicationName : null,
                            ddlApplications = applicationList,
                            ModuleId = Convert.ToInt32(role.ModuleId),
                            ModuleName = singleOrDefaultModule != null ? singleOrDefaultModule.ModuleName : null,
                            ddlModules = moduleList
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
        // POST: /Role/Save

        [HttpPost]
        public ActionResult Save(RoleViewModel roleViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //add
                    if (roleViewModel.RoleId == 0 && roleViewModel.ActionName == "Add")
                    {
                        var model = new TblRole()
                        {
                            RoleId = roleViewModel.RoleId,
                            RoleName = roleViewModel.RoleName,
                            Description = roleViewModel.Description,
                            ApplicationId = roleViewModel.ApplicationId,
                            ModuleId = roleViewModel.ModuleId
                        };

                        //_roleRepository.Insert(model);
                    }
                    else if (roleViewModel.ActionName == "Edit") //edit
                    {
                        TblRole role = _roleRepository.GetById(roleViewModel.RoleId);

                        if (role != null)
                        {

                            role.RoleId = roleViewModel.RoleId;
                            role.RoleName = roleViewModel.RoleName;
                            role.Description = roleViewModel.Description;

                            role.ApplicationId = roleViewModel.ApplicationId;
                            role.ModuleId = roleViewModel.ModuleId;

                            //_roleRepository.Update(role);

                        }
                        else
                        {
                            return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, roleViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageForNullObject()));
                        }

                    }

                    //_roleRepository.Save();

                    return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.TrueString, roleViewModel.ActionName, MessageType.success.ToString(), "Saved Successfully."));

                }

                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, roleViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ModelStateErrorFormat(ModelState)));
            }
            catch (Exception ex)
            {
                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, roleViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageFormat(ex)));
            }
        }

        //
        // POST: /Role/Delete/By ID
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                //var role = _roleRepository.GetById(id);
                var role = _roleRepository.GetAll().SingleOrDefault(x => x.RoleId == id);
                if (role != null)
                {
                    _roleRepository.Delete(role);
                    _roleRepository.Save();

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

        private List<RoleViewModel> GetRoleDataList()
        {
            var dataList = _roleRepository.GetAll().ToList().Select(c => new TblRole
            {
                RoleId = c.RoleId,
                RoleName = c.RoleName,
                Description = c.Description,
                ApplicationId = c.ApplicationId,
                ModuleId = c.ModuleId

            });

            var viewModels = dataList.Select(
                md =>
                {
                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == md.ApplicationId);
                    return singleOrDefaultApplication != null ? new RoleViewModel
                          {
                              RoleId = md.RoleId,
                              RoleName = md.RoleName,
                              Description = md.Description,
                              ApplicationId = Convert.ToInt32(md.ApplicationId),
                              ApplicationName = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == md.ApplicationId).ApplicationName,
                              ModuleId = Convert.ToInt32(md.ModuleId),
                              ModuleName = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == md.ModuleId).ModuleName,

                              ActionLink = KendoUiHelper.KendoUIGridActionLinkGenerate(md.RoleId.ToString())
                          } : null;
                }).OrderBy(o => o.RoleName).ToList();

            return viewModels;
        }

        #endregion

    }
}
