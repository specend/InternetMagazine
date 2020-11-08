using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InternetMagazine.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InternetMagazine.Controllers
{
    [AllowAnonymous]
    public class ChangeDateController : Controller
    {
        private ClassContext _db;

        public ChangeDateController(ClassContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> MyProfile()
        {
            Customer customer = await _db.Customer.FirstOrDefaultAsync(c => c.Login == User.Identity.Name);
            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(int? id)
        {
            if (id != null)
            {
                Customer customer = await _db.Customer.FirstOrDefaultAsync(c => c.Id_Customer == id);
                if (customer != null)
                    return View(customer);
            }
            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Customer c)
        {
            if (!ModelState.IsValid)
            {
                Customer customer = await _db.Customer.FirstOrDefaultAsync(cu => cu.Login == User.Identity.Name);
                
                Customer check_customer_login = await _db.Customer.FirstOrDefaultAsync(cus => cus.Login == c.Login);
                Customer check_customer_email = await _db.Customer.FirstOrDefaultAsync(cus => cus.Email == c.Email);
                Customer check_customer_phone = await _db.Customer.FirstOrDefaultAsync(cus => cus.Phone == c.Phone);
                if (check_customer_login == null || check_customer_email == null || check_customer_phone == null)
                {
                    customer.FIO = c.FIO;
                    customer.Gender = c.Gender;
                    customer.DateOfBirth = c.DateOfBirth;
                    customer.Email = c.Email;
                    customer.Phone = c.Phone;
                    customer.Login = c.Login;
                    _db.Customer.Update(customer);
                    await _db.SaveChangesAsync();

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    var claims = new List<Claim>
                    {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, customer.Login)
                    };
                    // создаем объект ClaimsIdentity
                    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    //установка идентификационных куки
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

                    return RedirectToAction("MyProfile");
                }
                else ModelState.AddModelError("", "Некорректные логин, e-mail или номер телефона либо аккаунт с такими данными уже существует");
            }

            return View(c);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ValidateViewModel v)
        {
            if (ModelState.IsValid)
            {
                Customer customer = await _db.Customer.FirstOrDefaultAsync(cu => cu.Email == v.Email);
                if (customer == null)
                {
                    return View("ForgotPasswordConfirmation");
                }

                var callbackUrl = Url.Action("ChangePassword", "ChangeDate", new { Id = customer.Id_Customer , email = customer.Email}, protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(v.Email, "Изменение пароля",
                    $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }
            return View(v);
        }

        [HttpGet]
        public IActionResult ChangePassword(string email = null)
        {
            return email == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            Customer customer = await _db.Customer.FirstOrDefaultAsync(c => c.Email == model.Email);

            customer.Password = model.Password;
            customer.ConfirmPassword = model.ConfirmPassword;

            _db.Customer.Update(customer);

            await _db.SaveChangesAsync();

            return View("ResetPasswordConfirmation");
        }
    }
}