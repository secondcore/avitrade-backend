using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Repositories;
using Vimba.AviTrade.Web.ViewModels;
using Vimba.AviTrade.Web.Helpers;

namespace Vimba.AviTrade.Web.Controllers
{
    public class LoginController : Controller
    {
        private IUsersRepository _usersRepository;
        private IUserConfigurationItemsRepository _userConfigurationItemsRepository;
        private IGroupsRepository _groupsRepository;
        private IRolesRepository _rolesRepository;
        private ITradersRepository _tradersRepository;

        public LoginController(IUsersRepository usrRep, IUserConfigurationItemsRepository cfgRep, IGroupsRepository grpRep, IRolesRepository rolRep, ITradersRepository trdRep)
        {
            _usersRepository = usrRep;
            _userConfigurationItemsRepository = cfgRep;
            _groupsRepository = grpRep;
            _rolesRepository = rolRep;
            _tradersRepository = trdRep;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        // Remote call from the viewer to validate the model trader code against the database
        // WARNING: The parameter name i.e. traderCode must match the model property name!!!
        public JsonResult VerifyTraderCode(string traderCode)
        {
            var result = false;
            var trader = _tradersRepository.IsRegistrationTokenValid(traderCode);

            if (trader != null)
                result = true;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Remote call from the viewer to validate the model MEBAA code against the database
        // WARNING: The parameter name i.e. mebaaCode must match the model property name!!!
        public JsonResult VerifyMebaaCode(string mebaaCode)
        {
            var result = false;
            // TODO: For now, validate locally
            if (mebaaCode == "YLFM1990")
                result = true;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Remote call from the viewer to validate the model AviTrade code against the database
        // WARNING: The parameter name i.e. aviTradeCode must match the model property name!!!
        public JsonResult VerifyAviTradeCode(string aviTradeCode)
        {
            var result = false;
            // TODO: For now, validate locally
            if (aviTradeCode == "YLFM1990")
                result = true;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LogOn(LogOnViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var actualUser = _usersRepository.FindByLogin(model.UserName);
                if (actualUser == null)
                {
                    //user not exists
                    ModelState.AddModelError("", "The user name provided is incorrect.");
                }
                else
                {
                    string hashedPassword = PasswordHelper.GetHashedPassword(model.Password);

                    if (!actualUser.Password.Equals(hashedPassword))
                    {
                        //password not valid
                        ModelState.AddModelError("", "The password provided is incorrect.");
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                        SessionHelper.Authenticated = actualUser;

                        //TODO: Probably there is no need for redirect
                        // We should look up the user's group and redirect accordingly
                        /*
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            // TODO: Must redirect to the members area
                            return RedirectToAction("Index", "Home");
                        }
                        */
                        if (actualUser.Group.Id == Group.Traders)
                        {
                            return RedirectToAction("Index", "Home", new { area = "Traders" });
                        }
                        else if (actualUser.Group.Id == Group.AviTrade)
                        {
                            return RedirectToAction("Index", "Home", new { area = "AviTrade" });
                        }
                        else if (actualUser.Group.Id == Group.Mebaa)
                        {
                            return RedirectToAction("Index", "Home", new { area = "Mebaa" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            SessionHelper.Authenticated = null;
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult TradersRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TradersRegister(TradersRegistrationViewModel model)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                // Make sure that the provided trader code has not been already registered
                var memItem = _userConfigurationItemsRepository.FindByKeyAndValue(UserConfigurationItem.TraderCode, model.TraderCode);
                if (memItem == null)
                {
                    var trader = _tradersRepository.FindByRegistrationToken(model.TraderCode);
                    if (trader != null)
                    {
                        var role = _rolesRepository.FindByRegistrationToken(model.TraderCode);

                        var newUser = CreateUser(model.Password, Group.Traders, role == null ? Role.Sales : role.Id, model.UserName, model.Name,
                                                 model.Email, UserConfigurationItem.TraderCode, model.TraderCode);

                        if (newUser != null)
                        {
                            FormsAuthentication.SetAuthCookie(newUser.Login, false /* createPersistentCookie */);

                            SessionHelper.Authenticated = newUser;

                            // Create a new trader registration token
                            _tradersRepository.CreateRegistrationToken(trader.Id, role == null ? Role.Sales : role.Id);
                            return RedirectToAction("Index", "Home", new { area = "Traders" });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "A trader could not be found!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "A user with this Login or Email exists yet.");
                }
            }
            else
            {
                ModelState.AddModelError("", "This trader code has already been registered.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult AviTradeRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AviTradeRegister(AviTradeRegistrationViewModel model)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                // Allow as many AviTrade users to sign up with the same AviTrade code
                var newUser = CreateUser(model.Password, Group.AviTrade, Role.Executives, model.UserName, model.Name, model.Email, UserConfigurationItem.AviTradeCode, model.AviTradeCode);

                if (newUser != null)
                {
                    FormsAuthentication.SetAuthCookie(newUser.Login, false /* createPersistentCookie */);

                    SessionHelper.Authenticated = newUser;

                    return RedirectToAction("Index", "Home", new { area = "AviTrade" });
                }
                else
                {
                    ModelState.AddModelError("", "A user with this Login or Email exists yet.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult MebaaRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MebaaRegister(MebaaRegistrationViewModel model)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                // Allow as many MEBAA users to sign up with the same MEBAA code
                var newUser = CreateUser(model.Password, Group.Mebaa, Role.Executives, model.UserName, model.Name, model.Email, UserConfigurationItem.AviTradeCode, model.MebaaCode);

                if (newUser != null)
                {
                    FormsAuthentication.SetAuthCookie(newUser.Login, false /* createPersistentCookie */);

                    SessionHelper.Authenticated = newUser;

                    return RedirectToAction("Index", "Home", new { area = "Mebaa" });
                }
                else
                {
                    ModelState.AddModelError("", "A user with this Login or Email exists yet.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedNewPassword = PasswordHelper.GetHashedPassword(model.NewPassword);
                string hashedOldPassword = PasswordHelper.GetHashedPassword(model.OldPassword);

                var actualUser = SessionHelper.Authenticated;

                if (actualUser.Password.Equals(hashedOldPassword))
                {
                    actualUser.Password = hashedNewPassword;
                    _usersRepository.Modify(actualUser);

                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        // P R I V A T E  M E T H O D S
        private User CreateUser(string password, string group, string role, string userName, string name, string email, string itemKey, string itemValue)
        {
            string hashedPassword = PasswordHelper.GetHashedPassword(password);

            // Pick up the group
            Group groupObject = _groupsRepository.FindById(group);
            if (groupObject == null)
                return null;

            // Pick up the role
            Role roleObject = _rolesRepository.FindById(role);
            if (roleObject == null)
                return null;

            var key = UserConfigurationItem.TraderCode;

            if (groupObject.Id == Group.Traders)
                key = UserConfigurationItem.TraderCode;
            else if (groupObject.Id == Group.AviTrade)
                key = UserConfigurationItem.AviTradeCode;
            else if (groupObject.Id == Group.Mebaa)
                key = UserConfigurationItem.MebaaCode;

            // Attempt to register the user
            User newUser = new User();
            newUser.Login = userName;
            newUser.Name = userName;
            newUser.Password = hashedPassword;
            newUser.Email = email;
            newUser.Group = groupObject;
            newUser.Role = roleObject;

            UserConfigurationItem item = new UserConfigurationItem();
            item.Key = key;
            item.Value = itemValue;
            newUser.ConfigurationItems.Add(item);
            _usersRepository.Add(newUser);

            if (newUser != null && newUser.Id != 0)
                return newUser;
            else
                return null;
        }
    }
}
