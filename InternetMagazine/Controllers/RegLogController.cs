using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using InternetMagazine.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InternetMagazine.Controllers
{
    public class RegLogController : Controller
    {
        
        private readonly ClassContext _cc;

        public RegLogController(ClassContext cc)
        {
            _cc = cc;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel l)
        {
            if (ModelState.IsValid)
            {
                //Проверка данных в БД
                Customer customer = await _cc.Customer.FirstOrDefaultAsync
                    (c => c.Login == l.Login && c.Password == l.Password);

                if (customer != null)
                {
                    //if (l.RememberMe) Properties.Resources.Remember = true;
                    //else Properties.Resources.Remember = false;

                    await Authenticate(l.Login); //аутентификация

                    customer.RememberMe = l.RememberMe;
                    _cc.Update(customer);
                    await _cc.SaveChangesAsync();

                    return RedirectToAction("Index", "Buy");
                }
                else ModelState.AddModelError("", "Некорректные логин или пароль");
            }
            return View(l);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Customer c)
        {
            if (ModelState.IsValid)
            {
                Customer customer = await _cc.Customer.FirstOrDefaultAsync(u => u.Login == c.Login || u.Email == c.Email || u.Phone == c.Phone);
                if (customer == null)
                {
                    //добавляем пользователя в БД
                    _cc.Add(c);
                    //сохраняем изменения в БД
                    await _cc.SaveChangesAsync();
                    //Properties.Resources.Remember = true;
                    await Authenticate(c.Login);//аутентификация

                    ViewBag.message = "Пользователь " + c.Login + " успешно зарегистрирован!";

                    return RedirectToAction("Index", "Buy");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин или пароль, либо введёные вами телефон или e-mail уже существуют");
            }
            return View(c);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "RegLog");
        }

        private async Task Authenticate(string userName)
        {
            //создаём один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            //установка идентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            
        }


    }
}