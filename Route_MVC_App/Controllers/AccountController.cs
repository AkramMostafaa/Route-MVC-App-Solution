using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.DAL.Models;
using Route_MVC_App.Controllers;
using Route_MVC_App.PL.Helpers;
using Route_MVC_App.PL.Settings;
using Route_MVC_App.PL.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Route_MVC_App.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailSettings _mailSettings;
        private readonly ISmsService _smsService;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IMailSettings mailSettings,ISmsService smsService)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _mailSettings = mailSettings;
            _smsService = smsService;
        }
        #region Sign Up



        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user =await  _userManager.FindByNameAsync(model.Email);
                if (user is null)
                {
					user = new ApplicationUser()
					{
						UserName = model.UserName,
						Email = model.Email,
						IsAgree = model.IsAgree,
						FName = model.FName,
						LName = model.LName,

					};
				var result=	await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
				}
                ModelState.AddModelError(string.Empty, "UserName is Already exists :)");
            }
            return View(model);
        }

        #endregion

        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag=await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RemeberMe, false);
                        if (result.Succeeded)
						    return RedirectToAction(nameof(HomeController.Index), "Home");

					}
                }
                ModelState.AddModelError(string.Empty, "Invalid Login ");
            }
            return View(model);     
        }
        #endregion

        #region Sign Out 


        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }



        #endregion

        #region Forget Password


        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                { 
                    var token = _userManager.GeneratePasswordResetTokenAsync(user);  
                    var ResetPasswordUrl = Url.Action("ResetPassword","Account",new { email = model.Email,token = token},Request.Scheme);
                    var email = new Email() 
                    {
                        Subject="RESET YOUR PASSWORD",
                        Recipients=model.Email,
                        To=model.Email,
                        Body= ResetPasswordUrl
                    };
                    _mailSettings.SendMail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invalid Email");
            }
            return View(model);
        }
        
        
        public IActionResult CheckYourInbox()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendSMS(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = _userManager.GeneratePasswordResetTokenAsync(user);
                    var ResetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
                    var sms = new SmsMessage()
                    {
                        
                        PhoneNumber=user.PhoneNumber,
                        Body=ResetPasswordUrl
                  
                    };
                    _smsService.Send(sms);
                    return Ok("Check You Phone");
                }
                ModelState.AddModelError(string.Empty, "Invalid Email");
            }
            return View(model);
        }


        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleResponse))

            };
            return Challenge(prop,GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result=await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim =>

                new {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value

                }
                );
            return RedirectToAction("Index", "Home");
        }


        #endregion


    }
}
