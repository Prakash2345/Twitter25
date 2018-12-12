using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using EFProject.Models;
using System.Web.Security;
using DAL;
using Model;

namespace EFProject.Controllers
{
    public class AccountController : Controller
    {
        private PersonService person;

        public AccountController()
        {
            person = new PersonService();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public PartialViewResult Header()
        {
            return PartialView();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginView, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (person.ValidateUser(loginView.Email, loginView.Password))
                {
                    FormsAuthentication.SetAuthCookie(loginView.Email.ToString(), false);
                    
                        return RedirectToAction("Index", "Tweet");
                }
                else
                {
                    loginView.ErrorMessage = "Either UserName and Password is incorrect or your account might not be active";
                }
            }
            return View(loginView);
        }
        public ActionResult Register(string userId = null)
        {
            if(userId == null)
            {
                return View();
            }
            else
            {
               var personDetails =  person.GetPersonDetails(userId);
                RegisterViewModel model = new Models.RegisterViewModel();
                model.Email = personDetails.email;
                model.FullName = personDetails.fullName;
                model.UserName = personDetails.user_id;
                model.Password = personDetails.password;
                return View(model);
            }
        }
        [HttpPost]
        public JsonResult ValidateUserName(RegisterViewModel model)
        {
            var response = person.ValidateUserName(model.UserName);
            return Json(response);
        }
        public ActionResult UpdateAccount(string userId)
        {
            
                var personDetails = person.GetPersonDetails(userId);
                RegisterViewModel model = new Models.RegisterViewModel();
                model.Email = personDetails.email;
                model.FullName = personDetails.fullName;
                model.UserName = personDetails.user_id;
                model.Password = personDetails.password;
                return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            Person personDetails = new Person();
            personDetails.email = model.Email;
            personDetails.password = model.Password;
            personDetails.user_id = model.UserName;
            personDetails.fullName = model.FullName;
            person.RegisterOrUpdateAccount(personDetails);
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAccount(RegisterViewModel model)
        {
            Person personDetails = new Person();
            personDetails.email = model.Email;
            personDetails.password = model.Password;
            personDetails.user_id = model.UserName;
            personDetails.fullName = model.FullName;
            person.RegisterOrUpdateAccount(personDetails);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccount(RegisterViewModel model)
        {
            person.DeleteAccount(model.UserName);
            return RedirectToAction("Login", "Account");
        }
    }
}