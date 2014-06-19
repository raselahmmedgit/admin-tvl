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
    public class ModuleController : Controller
    {
        private readonly Repository<TblModule> _moduleRepository;
        private readonly Repository<TblApplication> _applicationRepository;

        #region Constructor

        public ModuleController(Repository<TblModule> moduleRepository, Repository<TblApplication> applicationRepository)
        {
            this._moduleRepository = moduleRepository;
            this._applicationRepository = applicationRepository;
        }

        #endregion

        #region Action

        //
        // GET: /Module/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ModuleRead(KendoUiGridParam request)
        {
            var moduleViewModels = GetModuleDataList().AsQueryable();
            var models = KendoUiHelper.ParseGridData<ModuleViewModel>(moduleViewModels, request);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Module/Details/By ID

        public ActionResult Details(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var module = _moduleRepository.GetById(id);
                var module = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == id);
                if (module != null)
                {
                    var singleOrDefault = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == module.ApplicationId);
                    if (singleOrDefault != null)
                    {
                        var viewModel = new ModuleViewModel() { ModuleId = module.ModuleId, ModuleName = module.ModuleName, Description = module.Description, ModuleTitle = module.ModuleTitle, ApplicationId = module.ApplicationId, ApplicationName = singleOrDefault.ApplicationName };
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
        // GET: /Module/Add

        public ActionResult Add()
        {
            var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName").ToList();

            var viewModel = new ModuleViewModel() { ModuleId = 0, ddlApplications = applicationList };
            //return View();
            return PartialView("_AddOrEdit", viewModel);
        }

        //
        // GET: /Module/Edit/By ID

        public ActionResult Edit(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var module = _moduleRepository.GetById(id);
                var module = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == id);
                if (module != null)
                {
                    var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName", isEdit: true, selectedValue: module.ApplicationId.ToString()).ToList();

                    var singleOrDefault = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == module.ApplicationId);
                    if (singleOrDefault != null)
                    {

                        var viewModel = new ModuleViewModel()
                        {
                            ModuleId = module.ModuleId,
                            ModuleName = module.ModuleName,
                            Description = module.Description,
                            ModuleTitle = module.ModuleTitle,
                            ApplicationId = module.ApplicationId,
                            ApplicationName = singleOrDefault.ApplicationName,
                            ddlApplications = applicationList
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
        // POST: /Module/Save

        [HttpPost]
        public ActionResult Save(ModuleViewModel moduleViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //add
                    if (moduleViewModel.ModuleId == 0 && moduleViewModel.ActionName == "Add")
                    {
                        var model = new TblModule() { ModuleId = moduleViewModel.ModuleId, ModuleName = moduleViewModel.ModuleName, Description = moduleViewModel.Description, ModuleTitle = moduleViewModel.ModuleTitle, ApplicationId = moduleViewModel.ApplicationId };

                        _moduleRepository.Insert(model);
                    }
                    else if (moduleViewModel.ActionName == "Edit") //edit
                    {
                        TblModule module = _moduleRepository.GetById(moduleViewModel.ModuleId);

                        if (module != null)
                        {

                            module.ModuleId = moduleViewModel.ModuleId;
                            module.ModuleName = moduleViewModel.ModuleName;
                            module.Description = moduleViewModel.Description;
                            module.ModuleTitle = moduleViewModel.ModuleTitle;
                            module.ApplicationId = moduleViewModel.ApplicationId;

                            _moduleRepository.Update(module);

                        }
                        else
                        {
                            return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, moduleViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageForNullObject()));
                        }

                    }

                    _moduleRepository.Save();

                    return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.TrueString, moduleViewModel.ActionName, MessageType.success.ToString(), "Saved Successfully."));

                }

                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, moduleViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ModelStateErrorFormat(ModelState)));
            }
            catch (Exception ex)
            {
                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, moduleViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageFormat(ex)));
            }
        }

        //
        // POST: /Module/Delete/By ID
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                //var module = _moduleRepository.GetById(id);
                var module = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == id);
                if (module != null)
                {
                    _moduleRepository.Delete(module);
                    _moduleRepository.Save();

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

        private List<ModuleViewModel> GetModuleDataList()
        {
            var dataList = _moduleRepository.GetAll().ToList().Select(c => new TblModule { ModuleId = c.ModuleId, ModuleName = c.ModuleName, Description = c.Description, ModuleTitle = c.ModuleTitle, ApplicationId = c.ApplicationId });

            var viewModels = dataList.Select(
                md =>
                {
                    var singleOrDefault = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == md.ApplicationId);
                    return singleOrDefault != null ? new ModuleViewModel
                          {
                              ModuleId = md.ModuleId,
                              ModuleName = md.ModuleName,
                              ModuleTitle = md.ModuleTitle,
                              Description = md.Description,
                              ApplicationId = md.ApplicationId,
                              ApplicationName = singleOrDefault.ApplicationName,

                              ActionLink = KendoUiHelper.KendoUIGridActionLinkGenerate(md.ModuleId.ToString())
                          } : null;
                }).OrderBy(o => o.ModuleName).ToList();

            return viewModels;
        }

        #endregion

    }
}
