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
    public class AccountController : Controller
    {
        #region Action

        //LogIn
        public ActionResult LogIn()
        {
            return View();
        }

        //Register
        public ActionResult Register()
        {
            return View();
        }

        //ForgotPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }

        #endregion

        #region Methods

        #endregion
    }
}
