using practic1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace practic1.Controllers
{
    public class AccountController : Controller
    {
        private ps2Entities db = new ps2Entities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                Пользователи user = null;


                user = db.Пользователи.FirstOrDefault(u => u.Логин == model.Name);

                if (user != null)
                {
                    //string salt = user;
                    //string hashed = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password + salt, "SHA1");

                    if (user.Хэш_пароля == model.Password)
                    {
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                        return RedirectToAction("Index", "Home");
                    }
                    else { ModelState.AddModelError("", "Пользователя с таким логином и паролем нет"); }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }
            return View(model);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}