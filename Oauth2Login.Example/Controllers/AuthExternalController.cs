using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MultipleOauth2Mvc.Models;
using Oauth2Login.Core;
using Oauth2Login.Service;

namespace MultipleOauth2Mvc.Controllers
{
    public enum AuthExternalMode
    {
        Default = 0,
        AttachLogin = 1
    }

    public class AuthExternalController : Controller
    {
        public ActionResult Login(string id, AuthExternalMode? mode)
        {
            var service = BaseOauth2Service.GetService(id);

            if (service != null)
            {
                var url = service.BeginAuthentication();

                if (mode.HasValue)
                    TempData["AuthExternalMode"] = mode;

                return Redirect(url);
            }
            else
            {
                return RedirectToAction("LoginFail");
            }
        }

        public ActionResult Callback(string id)
        {
            var service = BaseOauth2Service.GetService(id);

            if (service != null)
            {
                try
                {
                    var redirectUrl = service.ValidateLogin(Request);
                    if (redirectUrl != null)
                    {
                        return Redirect(redirectUrl);
                    }

                    // This is demo, so I am not handling saving of data into database
                    // 
                    AuthCallbackResult respModel = null;
                    TokenCallbackResult respTModel = null;
                    AuthExternalMode authMode = TempData["AuthExternalMode"] as AuthExternalMode? ?? AuthExternalMode.Default;
                    if (authMode == AuthExternalMode.AttachLogin)
                    {
                        // var userSession = GetUserSession();
                        // if (userSession == null)
                        //    throw new Exception("Initial attach call was probably coming from other domain / session");

                        // var login = BaseAttachToExistingLogin(userSession.UserId, service.UserData);
                        respModel = new AuthCallbackResult { RedirectUrl = "/Accounts/AttachLoginProviders" };
                    }
                    else
                    {
                        // respModel = InsertNewUserIntoDatabase(service);
                        respModel = new AuthCallbackResult {RedirectUrl = "/AuthExternal/LoginSuccess"};
                        respTModel = new TokenCallbackResult {RedirectUrl = "/AuthExternal/TokenSuccess"};
                    }

                    return View(respModel);
                }
                catch (Exception ex)
                {
                    throw ex;
                    //RedirectToAction("Error");
                }
            }
            else
            {
                return RedirectToAction("LoginFail");
            }
        }

        public ActionResult LoginFail()
        {
            return View();
        }

        public ActionResult LoginSuccess(string id)
        {
            if (id == null) Response.Redirect("/");
            var service = BaseOauth2Service.GetService(id);
            if (service != null)
            {
                try
                {
                    var redirectUrl = service.ValidateLogin(Request);
                    if (redirectUrl != null)
                    {
                        return Redirect(redirectUrl);
                    }
                    ViewBag.Email = service.UserData.Email;
                    ViewBag.Name = service.UserData.FullName;
                    ViewBag.Id = service.UserData.UserId;

                }
                catch (Exception ex)
                {
                    throw ex;
                    //RedirectToAction("Error");
                }
            }
            else
            {
                return RedirectToAction("LoginFail");
            }
            return View();
        }

        public ActionResult TokenSuccess(string id)
        {
            var service = BaseOauth2Service.GetService(id);
            return View();
        }
        // plumbing
    }
}