using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.EmailServices;
using ShopApp.WebUI.Extensions;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private ICartService _cartService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ICartService cartService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _cartService = cartService;
        }

        public IActionResult Register()
        {
            return View(new RegisterModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });
                //send email
                string siteUri = "http://localhost:2146";
                string activateUri = $"{siteUri}{callbackUrl}";
                string body = $"Merhaba {model.UserName}; <br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'> tıklayınız.</a>";
                MailHelper.SendMail(body, model.Email, "ShopApp Hesap Aktifleştirme");

                // Create Cart Object
                _cartService.InitializeCart(user.Id);

                TempData.Put("message", new ResultMessage()
                {
                    Title = "Hesap Onayı",
                    Message = "EPosta adresinize gelen link ile hesabınızı onaylayınız",
                    Css = "warning"
                });

                await _userManager.AddToRoleAsync(user, "user");
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Oppss!! birşeyler ters gitti lütfen tekrar deneyiniz.");

            return View(model);
        }

        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model /*, string? returnUrl*/)
        {
            //returnUrl = returnUrl ?? "~/";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            #region UserName Login
            //var user = await _userManager.FindByNameAsync(model.UserName);

            //if (user == null)
            //{
            //    ModelState.AddModelError("", "Bu kullanıcı ile daha önce hesap oluşturulmamış");
            //    return View(model);
            //}

            //var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);

            //if (result.Succeeded)
            //{
            //    return Redirect(returnUrl);
            //}

            //ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");

            //return View(new LoginModel());


            #endregion

            #region Email Login

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu email ile daha önce hesap oluşturulmamıştır.");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("","Lütfen hesabınızı gönderilen email ile aktifleştirniz.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }

            ModelState.AddModelError("", "Email veya şifre hatalı.");

            return View(new LoginModel());

            #endregion

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData.Put("message", new ResultMessage()
            {
                Title = "Oturum Kapatıldı",
                Message = "Hesabınız güvenli bir şekilde sonlandırıldı.",
                Css = "warning"
            });

            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Hesap Onayı",
                    Message = "Hesap onayı için bilgileriniz yanlış",
                    Css = "danger"
                });
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Hesap Onayı",
                        Message = "Hesabınız başarıyla onaylanmıştır.",
                        Css = "success"
                    });
                    return RedirectToAction("Login");
                }
            }

            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesap Onayı",
                Message = "Hesabınız onaylanamadı.",
                Css = "danger"
            });
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forgot Password",
                    Message = "Bilgileriniz Hatalı",
                    Css = "danger"
                });

                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forgot Password",
                    Message = "EPosta adresi ile bir kullanıcı bulunamadı",
                    Css = "danger"
                });

                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                token = code
            });
            //send email
            string siteUri = "http://localhost:2146";
            string body = $"Parolanızı yenilemek için link <a href='{siteUri}{callbackUrl}'> tıklayınız. </a>";
            MailHelper.SendMail(body,Email, "ShopApp Şifre Resetleme");

            TempData.Put("message", new ResultMessage()
            {
                Title = "Forgot Password",
                Message = "Parola yenilemek için hesabınıza mail gönderildi.",
                Css = "warning"
            });

            return RedirectToAction("Login", "Account");

            //return View();
        }

        public IActionResult ResetPassword(string token)
        {
            if (token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordModel { Token = token };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forgot Password",
                    Message = "EPosta adresi ile bir kullanıcı bulunamadı",
                    Css = "danger"
                });

                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        public IActionResult Accessdenied()
        {
            return View();
        }
    }
}
