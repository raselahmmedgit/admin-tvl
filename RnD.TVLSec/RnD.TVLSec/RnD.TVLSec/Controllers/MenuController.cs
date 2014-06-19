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
    public class MenuController : Controller
    {
        private readonly Repository<TblMenu> _menuRepository;
        private readonly Repository<TblModule> _moduleRepository;
        private readonly Repository<TblApplication> _applicationRepository;

        #region Constructor

        public MenuController(Repository<TblMenu> menuRepository, Repository<TblModule> moduleRepository, Repository<TblApplication> applicationRepository)
        {
            this._menuRepository = menuRepository;
            this._moduleRepository = moduleRepository;
            this._applicationRepository = applicationRepository;
        }

        #endregion

        #region Action

        //
        // GET: /Menu/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult MenuRead(KendoUiGridParam request)
        {
            var menuViewModels = GetMenuDataList().AsQueryable();
            var models = KendoUiHelper.ParseGridData<MenuViewModel>(menuViewModels, request);

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Menu/Details/By ID

        public ActionResult Details(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var menu = _menuRepository.GetById(id);
                var menu = _menuRepository.GetAll().SingleOrDefault(x => x.MenuId == id);
                if (menu != null)
                {
                    var singleOrDefaultParentMenu = _menuRepository.GetAll().SingleOrDefault(x => x.MenuId == menu.ParentMenuId);

                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == menu.ApplicationId);

                    var singleOrDefaultModule = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == menu.ModuleId);

                    if (singleOrDefaultApplication != null && singleOrDefaultModule != null)
                    {
                        var viewModel = new MenuViewModel() { MenuId = menu.MenuId, MenuName = menu.MenuName, MenuCaption = menu.MenuCaption, MenuCaptionBng = menu.MenuCaptionBng, PageUrl = menu.PageUrl, SerialNo = Convert.ToInt32(menu.SerialNo), OrderNo = Convert.ToInt32(menu.OrderNo), ParentMenuId = Convert.ToInt32(menu.ParentMenuId), ParentMenuName = singleOrDefaultParentMenu.MenuName, ApplicationId = menu.ApplicationId, ApplicationName = singleOrDefaultApplication.ApplicationName, ModuleId = menu.ModuleId, ModuleName = singleOrDefaultModule.ModuleName };

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
        // GET: /Menu/Add

        public ActionResult Add()
        {
            var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName").ToList();

            var moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().ToList<TblModule>(), "ModuleId", "ModuleTitle").ToList();

            var parentMenuList = SelectListItemExtension.PopulateDropdownList(_menuRepository.GetAll().ToList<TblMenu>(), "MenuId", "MenuName").ToList();

            var viewModel = new MenuViewModel() { MenuId = 0, ddlApplications = applicationList, ddlModules = moduleList, ddlParentMenus = parentMenuList };
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

        public JsonResult GetParentMenuList()
        {
            var parentMenuList = SelectListItemExtension.PopulateDropdownList(_menuRepository.GetAll().ToList<TblMenu>(), "MenuId", "MenuName").ToList();

            return Json(parentMenuList, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Menu/Edit/By ID

        public ActionResult Edit(int id)
        {
            var errorViewModel = new ErrorViewModel();

            try
            {
                //var menu = _menuRepository.GetById(id);
                var menu = _menuRepository.GetAll().SingleOrDefault(x => x.MenuId == id);
                if (menu != null)
                {
                    var parentMenuList = SelectListItemExtension.PopulateDropdownList(_menuRepository.GetAll().ToList<TblMenu>(), "MenuId", "MenuName", isEdit: true, selectedValue: menu.ParentMenuId.ToString()).ToList();

                    var moduleList = SelectListItemExtension.PopulateDropdownList(_moduleRepository.GetAll().ToList<TblModule>(), "ModuleId", "ModuleName", isEdit: true, selectedValue: menu.ModuleId.ToString()).ToList();

                    var applicationList = SelectListItemExtension.PopulateDropdownList(_applicationRepository.GetAll().ToList<TblApplication>(), "ApplicationId", "ApplicationName", isEdit: true, selectedValue: menu.ApplicationId.ToString()).ToList();

                    var singleOrDefaultParentMenu = _menuRepository.GetAll().SingleOrDefault(x => x.MenuId == menu.ParentMenuId);

                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == menu.ApplicationId);

                    var singleOrDefaultModule = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == menu.ModuleId);

                    if (singleOrDefaultApplication != null && singleOrDefaultModule != null)
                    {

                        var viewModel = new MenuViewModel()
                        {
                            MenuId = menu.MenuId,
                            MenuName = menu.MenuName,
                            MenuCaption = menu.MenuCaption,
                            MenuCaptionBng = menu.MenuCaptionBng,
                            PageUrl = menu.PageUrl,
                            SerialNo = Convert.ToInt32(menu.SerialNo),
                            OrderNo = Convert.ToInt32(menu.OrderNo),
                            ParentMenuId = Convert.ToInt32(menu.ParentMenuId),
                            ParentMenuName = singleOrDefaultParentMenu != null ? singleOrDefaultParentMenu.MenuName : null,
                            ddlParentMenus = parentMenuList,
                            ApplicationId = menu.ApplicationId,
                            ApplicationName = singleOrDefaultApplication != null ? singleOrDefaultApplication.ApplicationName : null,
                            ddlApplications = applicationList,
                            ModuleId = menu.ModuleId,
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
        // POST: /Menu/Save

        [HttpPost]
        public ActionResult Save(MenuViewModel menuViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //add
                    if (menuViewModel.MenuId == 0 && menuViewModel.ActionName == "Add")
                    {
                        var model = new TblMenu()
                        {
                            MenuId = menuViewModel.MenuId,
                            MenuName = menuViewModel.MenuName,
                            MenuCaption = menuViewModel.MenuCaption,
                            MenuCaptionBng = menuViewModel.MenuCaptionBng,
                            PageUrl = menuViewModel.PageUrl,
                            SerialNo = menuViewModel.SerialNo,
                            OrderNo = menuViewModel.OrderNo,
                            ParentMenuId = menuViewModel.ParentMenuId,
                            ApplicationId = menuViewModel.ApplicationId,
                            ModuleId = menuViewModel.ModuleId
                        };

                        _menuRepository.Insert(model);
                    }
                    else if (menuViewModel.ActionName == "Edit") //edit
                    {
                        TblMenu menu = _menuRepository.GetById(menuViewModel.MenuId);

                        if (menu != null)
                        {

                            menu.MenuId = menuViewModel.MenuId;
                            menu.MenuName = menuViewModel.MenuName;
                            menu.MenuCaption = menuViewModel.MenuCaption;
                            menu.MenuCaptionBng = menuViewModel.MenuCaptionBng;
                            menu.PageUrl = menuViewModel.PageUrl;
                            menu.SerialNo = menuViewModel.SerialNo;
                            menu.OrderNo = menuViewModel.OrderNo;
                            menu.ParentMenuId = menuViewModel.ParentMenuId;
                            menu.ApplicationId = menuViewModel.ApplicationId;
                            menu.ModuleId = menuViewModel.ModuleId;

                            _menuRepository.Update(menu);

                        }
                        else
                        {
                            return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, menuViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageForNullObject()));
                        }

                    }

                    _menuRepository.Save();

                    return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.TrueString, menuViewModel.ActionName, MessageType.success.ToString(), "Saved Successfully."));

                }

                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, menuViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ModelStateErrorFormat(ModelState)));
            }
            catch (Exception ex)
            {
                return Content(KendoUiHelper.GetKendoUiWindowAjaxSuccessMethod(Boolean.FalseString, menuViewModel.ActionName, MessageType.warning.ToString(), ExceptionHelper.ExceptionMessageFormat(ex)));
            }
        }

        //
        // POST: /Menu/Delete/By ID
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                //var menu = _menuRepository.GetById(id);
                var menu = _menuRepository.GetAll().SingleOrDefault(x => x.MenuId == id);
                if (menu != null)
                {
                    _menuRepository.Delete(menu);
                    _menuRepository.Save();

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

        private List<MenuViewModel> GetMenuDataList()
        {
            var dataList = _menuRepository.GetAll().ToList().Select(c => new TblMenu
            {
                MenuId = c.MenuId,
                MenuName = c.MenuName,
                MenuCaption = c.MenuCaption,
                MenuCaptionBng = c.MenuCaptionBng,
                PageUrl = c.PageUrl,
                SerialNo = c.SerialNo,
                OrderNo = c.OrderNo,
                ParentMenuId = c.ParentMenuId,
                ApplicationId = c.ApplicationId,
                ModuleId = c.ModuleId

            });

            var viewModels = dataList.Select(
                md =>
                {
                    var singleOrDefaultApplication = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == md.ApplicationId);
                    return singleOrDefaultApplication != null ? new MenuViewModel
                          {
                              MenuId = md.MenuId,
                              MenuName = md.MenuName,
                              MenuCaption = md.MenuCaption,
                              MenuCaptionBng = md.MenuCaptionBng,
                              PageUrl = md.PageUrl,
                              SerialNo = Convert.ToInt32(md.SerialNo),
                              OrderNo = Convert.ToInt32(md.OrderNo),
                              ParentMenuId = Convert.ToInt32(md.ParentMenuId),
                              ParentMenuName = _menuRepository.GetAll().SingleOrDefault(x => x.MenuId == md.MenuId).MenuName,
                              ApplicationId = md.ApplicationId,
                              ApplicationName = _applicationRepository.GetAll().SingleOrDefault(x => x.ApplicationId == md.ApplicationId).ApplicationName,
                              ModuleId = md.ModuleId,
                              ModuleName = _moduleRepository.GetAll().SingleOrDefault(x => x.ModuleId == md.ModuleId).ModuleName,

                              ActionLink = KendoUiHelper.KendoUIGridActionLinkGenerate(md.MenuId.ToString())
                          } : null;
                }).OrderBy(o => o.MenuName).ToList();

            return viewModels;
        }

        #endregion

    }
}
