using Park_University_MVC.BusinessLayer;
using Park_University_MVC.Models;
using Park_University_MVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;

namespace Park_University_MVC.Controllers
{
    public class AuthController : ControllerBase
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            LoginModel lm = new LoginModel();
            return View(lm);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel lm)
        {
            if (ModelState.IsValid)
            {
                bool isValid = BusinessAuth.VerifyLogin(lm.UserName,lm.Password);
                if (isValid)
                {
                    string roles = BusinessAuth.GetRolesForUser(lm.UserName);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, lm.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, roles);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);
                    string redirectUrl = FormsAuthentication.GetRedirectUrl(lm.UserName, false);
                    if (redirectUrl == "/default.aspx")
                        redirectUrl = "~/home/index";
                    Response.Redirect(redirectUrl);
                }
                else
                {
                    ViewBag.Msg = "Login Fail!";
                }
            }
            return View(lm);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
