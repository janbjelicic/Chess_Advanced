using Chess.Domain.Concrete;
using Chess.Domain.Entities;
using Chess.Domain.Resources;
using Chess.Models;
using Microsoft.Web.WebPages.OAuth;
using Postal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Chess.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ChessDatabase db = new ChessDatabase();
        public AccountController() { }
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {            
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                if (HttpRuntime.Cache["LoggedInUsers"] != null)
                {
                    List<string> loggedInUsers = (List<string>)HttpRuntime.Cache["LoggedInUsers"];
                    loggedInUsers.Add(model.UserName);
                    HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                }
                else
                {
                    List<string> loggedInUsers = new List<string>();
                    loggedInUsers.Add(model.UserName);
                    HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                }
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", Resources.LoginErrorMessage);
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            string username = User.Identity.Name; 
            if (HttpRuntime.Cache["LoggedInUsers"] != null)
            {
                List<string> loggedInUsers = (List<string>)HttpRuntime.Cache["LoggedInUsers"];
                if (loggedInUsers.Contains(username))
                {
                    loggedInUsers.Remove(username);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    string confirmationToken = WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { Email = model.Email }, true);
                    dynamic email = new Email("RegEmail");
                    email.To = model.Email;
                    email.UserName = model.UserName;
                    email.ConfirmationToken = confirmationToken;
                    email.Send();                    
                    using (var db = new ChessDatabase())
                    {
                        var user = new User
                        {
                            ID = WebSecurity.CurrentUserId,
                            UserName = model.UserName,
                            PlayingRating = 1200,
                            ProblemRating = 0,
                            DateCreated = DateTime.Now,
                            Email = model.Email,
                            ConfirmationToken = confirmationToken,
                            CultureInfo = "en",
                            Gain = 0,
                            Minutes = 5,
                            RatingFrom = 0,
                            RatingTo = 3000,
                            IsRated = true,
                            Comments = new List<Comment>(),
                            Matches = new List<Match>()
                        };
                        Roles.AddUserToRole(model.UserName, db.Users.Any() ? UserRoles.Player.ToString() : UserRoles.Administrator.ToString());
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                    return RedirectToAction("RegisterStepTwo", "Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult RegisterStepTwo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegisterConfirmation(string Id)
        {
            if (WebSecurity.ConfirmAccount(Id))
            {
                return RedirectToAction("ConfirmationSuccess");
            }
            return RedirectToAction("ConfirmationFailure");
        }

        [AllowAnonymous]
        public ActionResult ConfirmationSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmationFailure()
        {
            return View();
        }      

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            int userId = WebSecurity.CurrentUserId;
            LocalPasswordModel model = new LocalPasswordModel
            {
                User = db.Users
                        .Include(x => x.Matches)
                        .Include(x => x.Comments)
                        .Include(x => x.Comments.Select(y => y.Position))
                        .Include(x => x.Comments.Select(y => y.Match))
                        .FirstOrDefault(x => x.ID == userId)
            };
            model.User.Matches.AddRange(db.Matches.Where(x => x.White.ID == userId || x.Black.ID == userId).ToList());
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : "";
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View(model);
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Information(LocalPasswordModel model)
        {
            using (var db = new ChessDatabase())
            {
                var modifiedUser = db.Users.Find(model.User.ID);
                if (modifiedUser == null)
                    return RedirectToAction("Manage");
                modifiedUser.FirstName = model.User.FirstName;
                modifiedUser.LastName = model.User.LastName;
                modifiedUser.Address = model.User.Address;
                modifiedUser.UserName = model.User.UserName;
                db.SaveChanges();
            }
            return RedirectToAction("Manage");
        }

        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            return Redirect(returnUrl);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return Resources.DuplicateUserName;

                case MembershipCreateStatus.DuplicateEmail:
                    return Resources.DuplicateEmail;

                case MembershipCreateStatus.InvalidPassword:
                    return Resources.InvalidPassword;

                case MembershipCreateStatus.InvalidEmail:
                    return Resources.InvalidEmail;

                default:
                    return Resources.DefaultErrorMessage;
            }
        }
        #endregion
    }
}
