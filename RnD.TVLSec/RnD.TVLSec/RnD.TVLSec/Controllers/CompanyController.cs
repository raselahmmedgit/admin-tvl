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
    public class CompanyController : Controller
    {
        private readonly Repository<TblCompany> _companyRepository;

        #region Constructor

        public CompanyController(Repository<TblCompany> companyRepository)
        {
            this._companyRepository = companyRepository;
        }

        #endregion

        #region Action

        //
        // GET: /Company/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CompanyRead(KendoUiGridParam request)
        {
            var companyViewModels = GetCompanyDataList().AsQueryable();
            var models = KendoUiHelper.ParseGridData<CompanyViewModel>(companyViewModels, request);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Company/Details/By ID

        public ActionResult Details(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var company = _companyRepository.GetById(id);
                var company = _companyRepository.GetAll().SingleOrDefault(x => x.CompanyId == id);
                if (company != null)
                {
                    var viewModel = new CompanyViewModel() { CompanyId = company.CompanyId, CompanyName = company.CompanyName, Address = company.Address };
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
        // GET: /Company/Add

        public ActionResult Add()
        {
            var viewModel = new CompanyViewModel() { CompanyId = 0 };
            //return View();
            return PartialView("_AddOrEdit", viewModel);
        }

        //
        // GET: /Company/Edit/By ID

        public ActionResult Edit(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var company = _companyRepository.GetById(id);
                var company = _companyRepository.GetAll().SingleOrDefault(x => x.CompanyId == id);
                if (company != null)
                {
                    var viewModel = new CompanyViewModel() { CompanyId = company.CompanyId, CompanyName = company.CompanyName, Address = company.Address };
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
        // POST: /Company/Save

        [HttpPost]
        public ActionResult Save(CompanyViewModel companyViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //add
                    if (companyViewModel.CompanyId == 0 && companyViewModel.ActionName == "Add")
                    {
                        var model = new TblCompany() { CompanyId = companyViewModel.CompanyId + 1, CompanyName = companyViewModel.CompanyName, Address = companyViewModel.Address, CreatedBy = "Rasel", CreatedDate = DateTime.Now, UpdatedBy = "Rasel", UpdatedDate = DateTime.Now };

                        _companyRepository.Insert(model);
                    }
                    else if (companyViewModel.ActionName == "Edit") //edit
                    {
                        TblCompany company = _companyRepository.GetById(Convert.ToInt32(companyViewModel.CompanyId));

                        if (company != null)
                        {

                            company.CompanyId = companyViewModel.CompanyId;
                            company.CompanyName = companyViewModel.CompanyName;
                            company.Address = companyViewModel.Address;
                            company.UpdatedBy = "Rasel";
                            company.UpdatedDate = DateTime.Now;

                            _companyRepository.Update(company);

                        }
                        else
                        {
                            return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, companyViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageForNullObject()));
                        }

                    }

                    _companyRepository.Save();

                    return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.TrueString, companyViewModel.ActionName, MessageType.success.ToString(), "Saved Successfully."));

                }

                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, companyViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ModelStateErrorFormat(ModelState)));
            }
            catch (Exception ex)
            {
                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, companyViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageFormat(ex)));
            }
        }

        //
        // POST: /Company/Delete/By ID
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                //var company = _companyRepository.GetById(id);
                var company = _companyRepository.GetAll().SingleOrDefault(x => x.CompanyId == id);
                if (company != null)
                {
                    _companyRepository.Delete(company);
                    _companyRepository.Save();

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

        private List<CompanyViewModel> GetCompanyDataList()
        {
            var dataList = _companyRepository.GetAll().ToList().Select(c => new TblCompany { CompanyId = c.CompanyId, CompanyName = c.CompanyName, Address = c.Address });

            var viewModels = dataList.Select(
                md => new CompanyViewModel
                {
                    CompanyId = md.CompanyId,
                    CompanyName = md.CompanyName,
                    Address = md.Address,

                    ActionLink = KendoUiHelper.KendoUIGridActionLinkGenerate(md.CompanyId.ToString())
                }).OrderBy(o => o.CompanyName).ToList();

            return viewModels;
        }

        #endregion

    }
}
