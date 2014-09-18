using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RnD.TVLSec.Helpers;
using RnD.TVLSec.Models;
using System.Data;
using RnD.TVLSec.ViewModels;
using RnD.TVLSec.ViewModels;

namespace RnD.TVLSec.Controllers
{
    public class UserGroupController : Controller
    {
        private readonly Repository<TblGroup> _groupRepository;
        private readonly Repository<TblModule> _moduleRepository;
        private readonly Repository<TblApplication> _applicationRepository;

        private readonly Repository<TblRole> _roleRepository;

        #region Constructor

        public UserGroupController(Repository<TblGroup> groupRepository, Repository<TblModule> moduleRepository, Repository<TblApplication> applicationRepository, Repository<TblRole> roleRepository)
        {
            this._groupRepository = groupRepository;
            this._moduleRepository = moduleRepository;
            this._applicationRepository = applicationRepository;

            this._roleRepository = roleRepository;
        }

        #endregion

        #region Action

        //
        // GET: /UserGroup/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult UserGroupRead(KendoUiGridParam request)
        {
            var groupViewModels = GetUserGroupDataList().AsQueryable();
            var models = KendoUiHelper.ParseGridData<GroupViewModel>(groupViewModels, request);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /UserGroup/Details/By ID

        public ActionResult Details(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var group = _groupRepository.GetById(id);
                var group = _groupRepository.GetAll().SingleOrDefault(x => x.GroupId == id);
                if (group != null)
                {


                    var viewModel = new GroupViewModel() { GroupId = group.GroupId, GroupName = group.GroupName, Description = group.Description };

                    return PartialView("_Details", viewModel);

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
        // GET: /UserGroup/Add

        public ActionResult Add()
        {
            var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName").ToList();

            var moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().ToList<TblModule>(), "ModuleId", "ModuleTitle").ToList();

            var viewModel = new GroupViewModel() { GroupId = 0, ddlApplications = applicationList, ddlModules = moduleList };

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
        // GET: /UserGroup/Edit/By ID

        public ActionResult Edit(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var group = _groupRepository.GetById(id);
                var group = _groupRepository.GetAll().SingleOrDefault(x => x.GroupId == id);
                if (group != null)
                {

                    var moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().ToList<TblModule>(), "ModuleId", "ModuleName").ToList();

                    var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName").ToList();

                    var viewModel = new GroupViewModel()
                    {
                        GroupId = group.GroupId,
                        GroupName = group.GroupName,
                        Description = group.Description,

                        ddlApplications = applicationList,

                        ddlModules = moduleList
                    };
                    return PartialView("_AddOrEdit", viewModel);

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
        // POST: /UserGroup/Save

        [HttpPost]
        public ActionResult Save(GroupViewModel groupViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //add
                    if (groupViewModel.GroupId == 0 && groupViewModel.ActionName == "Add")
                    {
                        var model = new TblGroup()
                        {
                            GroupId = groupViewModel.GroupId,
                            GroupName = groupViewModel.GroupName,
                            Description = groupViewModel.Description,
                            //ApplicationId = groupViewModel.ApplicationId,
                            //ModuleId = groupViewModel.ModuleId
                        };

                        //_groupRepository.Insert(model);
                        //add role to group wise
                    }
                    else if (groupViewModel.ActionName == "Edit") //edit
                    {
                        TblGroup group = _groupRepository.GetById(groupViewModel.GroupId);

                        if (group != null)
                        {

                            group.GroupId = groupViewModel.GroupId;
                            group.GroupName = groupViewModel.GroupName;
                            group.Description = groupViewModel.Description;

                            //group.ApplicationId = groupViewModel.ApplicationId;
                            //group.ModuleId = groupViewModel.ModuleId;

                            //_groupRepository.Update(group);

                        }
                        else
                        {
                            return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, groupViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageForNullObject()));
                        }

                    }

                    //_groupRepository.Save();

                    return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.TrueString, groupViewModel.ActionName, MessageType.success.ToString(), "Saved Successfully."));

                }

                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, groupViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ModelStateErrorFormat(ModelState)));
            }
            catch (Exception ex)
            {
                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, groupViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageFormat(ex)));
            }
        }

        //
        // POST: /UserGroup/Delete/By ID
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                //var group = _groupRepository.GetById(id);
                var group = _groupRepository.GetAll().SingleOrDefault(x => x.GroupId == id);
                if (group != null)
                {
                    _groupRepository.Delete(group);
                    _groupRepository.Save();

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

        private List<GroupViewModel> GetUserGroupDataList()
        {
            var dataList = _groupRepository.GetAll().ToList().Select(c => new TblGroup
            {
                GroupId = c.GroupId,
                GroupName = c.GroupName,
                Description = c.Description,
                //ApplicationId = c.ApplicationId,
                //ModuleId = c.ModuleId

            });

            var viewModels = dataList.Select(
                md => new GroupViewModel
                          {
                              GroupId = md.GroupId,
                              GroupName = md.GroupName,
                              Description = md.Description,

                              ActionLink = KendoUiHelper.KendoUIGridActionLinkGenerate(md.GroupId.ToString())
                          }
               ).OrderBy(o => o.GroupName).ToList();

            return viewModels;
        }

        #endregion

    }
}
