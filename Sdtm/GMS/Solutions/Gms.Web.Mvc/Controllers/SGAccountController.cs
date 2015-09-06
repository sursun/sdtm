using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SecurityGuard.Interfaces;
using SecurityGuard.Services;
using SecurityGuard.ViewModels;

namespace Gms.Web.Mvc.Controllers
{
    /// <summary>
    /// This class handles all the normal logon, logoff, 
    /// register, change password, and forgot password operations
    /// that occur in the public part of your web application.
    /// </summary>
    [HandleError]
    public class SGAccountController : BaseController
    {
                
        #region ctors

        private IMembershipService membershipService;
        private IAuthenticationService authenticationService;
   
        public SGAccountController()
        {
            this.membershipService = new MembershipService(Membership.Provider);
            this.authenticationService = new AuthenticationService(membershipService, new FormsAuthenticationService());
        }

        #endregion
        
        #region LogOn Methods

        [HttpGet]
        public virtual ActionResult Login()
        {
            var viewModel = new LogOnViewModel()
            {
                EnablePasswordReset = membershipService.EnablePasswordReset
            };

            ViewData["SysVersion"] = GetVersion();
            return ContextDependentView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Login(LogOnViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //var doctor = this.DoctorRepository.Get(model.UserName);

                //if (doctor == null)
                //{
                //    ModelState.AddModelError("", "�˻�������");
                //}
                //else 
                    if (authenticationService.LogOn(model.UserName, model.Password, model.RememberMe))
                {
                    OnLogin(model.UserName, model.RememberMe);


                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {

                    MembershipUser user = membershipService.GetUser(model.UserName);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "�˻�������");
                    }
                    else
                    {
                        if (!user.IsApproved)
                        {
                            ModelState.AddModelError("", "Your account has not been approved yet.");
                        }
                        else if (user.IsLockedOut)
                        {
                            ModelState.AddModelError("", "�����˻��ѱ�����");
                        }
                        else
                        {
                            ModelState.AddModelError("", "�û��������벻��ȷ");
                        }
                    }

                }
            }
            
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Login");
        }

        protected void OnLogin(string userName, bool rememberMe)
        {

            var user = Membership.GetUser(userName);

            SysLogRepository.Write(string.Format("��{0}����¼�ɹ���", userName), null);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                      userName,
                      DateTime.Now,
                      DateTime.Now.AddMinutes(12 * 60),
                      rememberMe,
                      user.ProviderUserKey.ToString(),
                      FormsAuthentication.FormsCookiePath);
            string encTicket = FormsAuthentication.Encrypt(ticket);


            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [ValidateAntiForgeryToken()]
        public virtual ActionResult LogOff()
        {
            SysLogRepository.Write(string.Format("��{0}���˳�ϵͳ��", CurrentUser==null?"ϵͳ����Ա":CurrentUser.LoginName), null);
            
            FormsAuthentication.SignOut();

            authenticationService.LogOff();

            return RedirectToAction("Index", "Home");
        } 
        #endregion


        #region ChangePassword Methods
        
        ///// <summary>
        ///// This allows the logged on user to change his password.
        ///// </summary>
        ///// <returns></returns>
        //[Authorize]
        //public virtual ActionResult ChangePassword()
        //{
        //    var viewModel = new ChangePasswordViewModel();

        //    return View(viewModel);
        //}

        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken()]
        //public virtual ActionResult ChangePassword(ChangePasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        // ChangePassword will throw an exception rather
        //        // than return false in certain failure scenarios.
        //        bool changePasswordSucceeded;
        //        try
        //        {
        //            MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
        //            changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
        //        }
        //        catch (Exception)
        //        {
        //            changePasswordSucceeded = false;
        //        }

        //        if (changePasswordSucceeded)
        //        {
        //            return RedirectToAction("ChangePasswordSuccess");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return RedirectToAction("ChangePassword");
        //}

        ////
        //// GET: /Account/ChangePasswordSuccess

        //public virtual ActionResult ChangePasswordSuccess()
        //{
        //    return View();
        //}


        #endregion

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
       
        #region Json Methods

        [HttpPost]
        public JsonResult JsonLogOn(LogOnViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authenticationService.LogOn(model.UserName, model.Password, model.RememberMe))
                {
                    return Json(new { success = true, redirect = returnUrl });
                }
                else
                {
                    MembershipUser user = membershipService.GetUser(model.UserName);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "�˻�������");
                    }
                    else
                    {
                        if (!user.IsApproved)
                        {
                            ModelState.AddModelError("", "Your account has not been approved yet.");
                        }
                        else if (user.IsLockedOut)
                        {
                            ModelState.AddModelError("", "Your account is currently locked.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "The user name or password provided is incorrect.");
                        }
                    }
                }
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }
        
        #endregion

        #region Private Helpers

        private ActionResult ContextDependentView(object model)
        {
            string actionName = ControllerContext.RouteData.GetRequiredString("action");

            //if (actionName.ToLower().Contains("login"))
                model = (LogOnViewModel)model;
            //else
            //    model = (RegisterViewModel)model;

            if (Request.QueryString["content"] != null)
            {
                ViewBag.FormAction = "Json" + actionName;
                return PartialView(model);
            }
            else
            {
                ViewBag.FormAction = actionName;
                return View(model);
            }
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        #endregion
        
    }
}
