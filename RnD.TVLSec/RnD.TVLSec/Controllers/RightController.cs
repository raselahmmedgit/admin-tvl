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
    public class RightController : Controller
    {
        private readonly Repository<TblRight> _rightRepository;
        private readonly Repository<TblModule> _moduleRepository;
        private readonly Repository<TblApplication> _applicationRepository;

        #region Constructor

        public RightController(Repository<TblRight> rightRepository, Repository<TblModule> moduleRepository, Repository<TblApplication> applicationRepository)
        {
            this._rightRepository = rightRepository;
            this._moduleRepository = moduleRepository;
            this._applicationRepository = applicationRepository;
        }

        #endregion

        #region Action

        //
        // GET: /Right/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult RightRead(KendoUiGridParam request)
        {
            var rightViewModels = GetRightDataList().AsQueryable();
            var models = KendoUiHelper.ParseGridData<RightViewModel>(rightViewModels, request);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Right/Details/By ID

        public ActionResult Details(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var right = _rightRepository.GetById(id);
                var right = _rightRepository.GetAll().SingleOrDefault(x => x.RightId == id);
                if (right != null)
                {

                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == right.ApplicationId);

                    var singleOrDefaultModule = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == right.ModuleId);

                    if (singleOrDefaultApplication != null && singleOrDefaultModule != null)
                    {
                        var viewModel = new RightViewModel() { RightId = right.RightId, RightName = right.RightName, RightTitle = right.RightTitle, Description = right.Description, ApplicationId = Convert.ToInt32(right.ApplicationId), ApplicationName = singleOrDefaultApplication.ApplicationName, ModuleId = Convert.ToInt32(right.ModuleId), ModuleName = singleOrDefaultModule.ModuleName };

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
        // GET: /Right/Add

        public ActionResult Add()
        {
            var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName").ToList();

            var moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().ToList<TblModule>(), "ModuleId", "ModuleTitle").ToList();

            var viewModel = new RightViewModel() { RightId = 0, ddlApplications = applicationList, ddlModules = moduleList };
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

        //
        // GET: /Right/Edit/By ID

        public ActionResult Edit(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var right = _rightRepository.GetById(id);
                var right = _rightRepository.GetAll().SingleOrDefault(x => x.RightId == id);
                if (right != null)
                {

                    var moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().ToList<TblModule>(), "ModuleId", "ModuleName", isEdit: true, selectedValue: right.ModuleId.ToString()).ToList();

                    var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName", isEdit: true, selectedValue: right.ApplicationId.ToString()).ToList();


                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == right.ApplicationId);

                    var singleOrDefaultModule = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == right.ModuleId);

                    if (singleOrDefaultApplication != null && singleOrDefaultModule != null)
                    {

                        var viewModel = new RightViewModel()
                        {
                            RightId = right.RightId,
                            RightName = right.RightName,
                            RightTitle = right.RightTitle,
                            Description = right.Description,
                            ApplicationId = Convert.ToInt32(right.ApplicationId),
                            ApplicationName = singleOrDefaultApplication != null ? singleOrDefaultApplication.ApplicationName : null,
                            ddlApplications = applicationList,
                            ModuleId = Convert.ToInt32(right.ModuleId),
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
        // POST: /Right/Save

        [HttpPost]
        public ActionResult Save(RightViewModel rightViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //add
                    if (rightViewModel.RightId == 0 && rightViewModel.ActionName == "Add")
                    {
                        var model = new TblRight()
                        {
                            RightId = rightViewModel.RightId,
                            RightName = rightViewModel.RightName,
                            RightTitle = rightViewModel.RightTitle,
                            Description = rightViewModel.Description,
                            ApplicationId = rightViewModel.ApplicationId,
                            ModuleId = rightViewModel.ModuleId
                        };

                        _rightRepository.Insert(model);
                    }
                    else if (rightViewModel.ActionName == "Edit") //edit
                    {
                        TblRight right = _rightRepository.GetById(rightViewModel.RightId);

                        if (right != null)
                        {

                            right.RightId = rightViewModel.RightId;
                            right.RightName = rightViewModel.RightName;
                            right.RightTitle = rightViewModel.RightTitle;
                            right.Description = rightViewModel.Description;

                            right.ApplicationId = rightViewModel.ApplicationId;
                            right.ModuleId = rightViewModel.ModuleId;

                            _rightRepository.Update(right);

                        }
                        else
                        {
                            return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, rightViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageForNullObject()));
                        }

                    }

                    _rightRepository.Save();

                    return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.TrueString, rightViewModel.ActionName, MessageType.success.ToString(), "Saved Successfully."));

                }

                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, rightViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ModelStateErrorFormat(ModelState)));
            }
            catch (Exception ex)
            {
                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, rightViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageFormat(ex)));
            }
        }

        //
        // POST: /Right/Delete/By ID
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                //var right = _rightRepository.GetById(id);
                var right = _rightRepository.GetAll().SingleOrDefault(x => x.RightId == id);
                if (right != null)
                {
                    _rightRepository.Delete(right);
                    _rightRepository.Save();

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

        private List<RightViewModel> GetRightDataList()
        {
            var dataList = _rightRepository.GetAll().ToList().Select(c => new TblRight
            {
                RightId = c.RightId,
                RightName = c.RightName,
                RightTitle = c.RightTitle,
                Description = c.Description,
                ApplicationId = c.ApplicationId,
                ModuleId = c.ModuleId

            });

            var viewModels = dataList.Select(
                md =>
                {
                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == md.ApplicationId);
                    return singleOrDefaultApplication != null ? new RightViewModel
                          {
                              RightId = md.RightId,
                              RightName = md.RightName,
                              RightTitle = md.RightTitle,
                              Description = md.Description,
                              ApplicationId = Convert.ToInt32(md.ApplicationId),
                              ApplicationName = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == md.ApplicationId).ApplicationName,
                              ModuleId = Convert.ToInt32(md.ModuleId),
                              ModuleName = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == md.ModuleId).ModuleName,

                              ActionLink = KendoUiHelper.KendoUIGridActionLinkGenerate(md.RightId.ToString())
                          } : null;
                }).OrderBy(o => o.RightName).ToList();

            return viewModels;
        }

        #endregion

    }
}
