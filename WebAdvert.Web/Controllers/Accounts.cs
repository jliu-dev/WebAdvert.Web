using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Controllers
{
    public class Accounts: Controller
    {
        private readonly SignInManager<CognitoUser> _singInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;

        public Accounts(SignInManager<CognitoUser> singInManager, UserManager<CognitoUser> userManager, CognitoUserPool pool)
        {
           this._singInManager = singInManager;
            _userManager = userManager;
            _pool = pool;


        }

        [HttpGet]
       public async Task<IActionResult> Signup()
        {
            var model = new SignupModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _pool.GetUser(model.Email);
                if(user.Status != null)
                {
                    ModelState.AddModelError("UserExists", "User with this email already exists");
                    return View(model);
                }

                user.Attributes.Add(CognitoAttribute.Name.AttributeName, model.Email);//not same as course
               var createdUser = await _userManager.CreateAsync(user, model.Password);

                if (createdUser.Succeeded)
                {
                    return RedirectToAction("Confirm");
                }
            }
            
            return View();
            //return null;
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(ConfirmModel model)
        {
            //var model = new ConfirmModel();
            return View(model);
        }

        [HttpPost]
        [ActionName("Confirm")]
        public async Task<IActionResult> Confirm_Post(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("NotFound", "User with this email was not found");
                    return View(model);
                }

                // var result =await _userManager.ConfirmEmailAsync(user, model.Code);//the third parameter is solve the nullReference issue
                var result = await ((CognitoUserManager<CognitoUser>)_userManager).ConfirmSignUpAsync(user, model.Code, true);

                if (result.Succeeded)
                {
                   return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var item in result.Errors)
                    {

                        ModelState.AddModelError(item.Code, item.Description);
                    }
                    return View(model);
                }
            }

            return View(model);
            //return null;
        }

        [HttpGet]
        public async Task<IActionResult> Login(LoginModel model)
        {
            //var model = new SignupModel();
            return View(model);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> Login_Post(LoginModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _singInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                
             

                if (result.Succeeded)
                {
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("LoginError", "some error here");
                }
            }

            return View("Login", model);
            //return null;
        }



    }


}

