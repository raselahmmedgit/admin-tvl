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
    public class ApplicationController : Controller
    {
        private readonly Repository<TblApplication> _applicationRepository;

        #region Constructor

        public ApplicationController(Repository<TblApplication> applicationRepository)
        {
            this._applicationRepository = applicationRepository;
        }

        #endregion

        #region Action

        //
        // GET: /Application/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ApplicationRead(KendoUiGridParam request)
        {
            var applicationViewModels = GetApplicationDataList().AsQueryable();
            var models = KendoUiHelper.ParseGridData<ApplicationViewModel>(applicationViewModels, request);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Application/Details/By ID

        public ActionResult Details(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var application = _applicationRepository.GetById(id);
                var application = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == id);
                if (application != null)
                {
                    var viewModel = new ApplicationViewModel() { ApplicationId = application.ApplicationId, ApplicationName = application.ApplicationName, Description = application.Description, ApplicationTitle = application.ApplicationTitle };
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
        // GET: /Application/Add

        public ActionResult Add()
        {
            var viewModel = new ApplicationViewModel() { ApplicationId = 0 };
            //return View();
            return PartialView("_AddOrEdit", viewModel);
        }

        //
        // GET: /Application/Edit/By ID

        public ActionResult Edit(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var application = _applicationRepository.GetById(id);
                var application = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == id);
                if (application != null)
                {
                    var viewModel = new ApplicationViewModel() { ApplicationId = application.ApplicationId, ApplicationName = application.ApplicationName, Description = application.Description, ApplicationTitle = application.ApplicationTitle };
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
        // POST: /Application/Save

        [HttpPost]
        public ActionResult Save(ApplicationViewModel applicationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //add
                    if (applicationViewModel.ApplicationId == 0 && applicationViewModel.ActionName == "Add")
                    {
                        var model = new TblApplication() { ApplicationId = applicationViewModel.ApplicationId, ApplicationName = applicationViewModel.ApplicationName, Description = applicationViewModel.Description, ApplicationTitle = applicationViewModel.ApplicationTitle };

                        _applicationRepository.Insert(model);
                    }
                    else if (applicationViewModel.ActionName == "Edit") //edit
                    {
                        TblApplication application = _applicationRepository.GetById(applicationViewModel.ApplicationId);

                        if (application != null)
                        {

                            application.ApplicationId = applicationViewModel.ApplicationId;
                            application.ApplicationName = applicationViewModel.ApplicationName;
                            application.Description = applicationViewModel.Description;
                            application.ApplicationTitle = applicationViewModel.ApplicationTitle;

                            _applicationRepository.Update(application);

                        }
                        else
                        {
                            return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, applicationViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageForNullObject()));
                        }

                    }

                    _applicationRepository.Save();

                    return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.TrueString, applicationViewModel.ActionName, MessageType.success.ToString(), "Saved Successfully."));

                }

                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, applicationViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ModelStateErrorFormat(ModelState)));
            }
            catch (Exception ex)
            {
                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, applicationViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageFormat(ex)));
            }
        }

        //
        // POST: /Application/Delete/By ID
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                //var application = _applicationRepository.GetById(id);
                var application = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == id);
                if (application != null)
                {
                    _applicationRepository.Delete(application);
                    _applicationRepository.Save();

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

        private List<ApplicationViewModel> GetApplicationDataList()
        {
            var dataList = _applicationRepository.GetAll().ToList().Select(c => new TblApplication { ApplicationId = c.ApplicationId, ApplicationName = c.ApplicationName, Description = c.Description, ApplicationTitle = c.ApplicationTitle });

            var viewModels = dataList.Select(
                md => new ApplicationViewModel
                {
                    ApplicationId = md.ApplicationId,
                    ApplicationName = md.ApplicationName,
                    ApplicationTitle = md.ApplicationTitle,
                    Description = md.Description,

                    ActionLink = KendoUiHelper.KendoUIGridActionLinkGenerate(md.ApplicationId.ToString())
                }).OrderBy(o => o.ApplicationName).ToList();

            return viewModels;
        }

        #endregion

    }
}
