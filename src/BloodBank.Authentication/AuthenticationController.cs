using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BloodBank.Authentication.Integration;
using BloodBank.Authentication.Models;
using BloodBank.Core.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BloodBank.Authentication
{
    [Route("api/auth")]
    
    public class AuthenticationController : Controller
    {
        private readonly IBus bus;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<AuthenticationController> logger;
        private readonly UserManager<User> userManager;

        public AuthenticationController(IBus bus,SignInManager<User> signInManager, ILogger<AuthenticationController> logger, UserManager<User> userManager)
        {
            this.bus = bus;
            this.signInManager = signInManager;
            this.logger = logger;
            this.userManager = userManager;
        }
        [Route("login")]
        [HttpPost]
        [ProducesResponseType(typeof(void),200)]
        [ProducesResponseType(403)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] SignInModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Username,
             model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                logger.LogInformation(1, "User logged in.");
                return Ok();
            }
            return Forbid();
           
            //if (result.IsLockedOut)
            //{
            //    logger.LogWarning(2, "User account locked out.");
            //    return View("Lockout");
            //}
        }
        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                user.LastName = model.LastName;
                user.FirstName = model.Name;
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {  
                    await signInManager.SignInAsync(user, isPersistent: false);
                    logger.LogInformation(3, "User created a new account with password.");
                    return Ok();
                }
                return BadRequest(result);
            }

            // If we got this far, something failed, redisplay form
            return BadRequest(model);
        }
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await  signInManager.SignOutAsync();
            return Ok();
        }
        [HttpGet]
        [Route("forgot")]
        [AllowAnonymous]
        public async Task<IActionResult> Forgot( string email)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(email);
                if (user != null)
                {
                    var token = await userManager.GenerateUserTokenAsync(user, "PasswordlessLoginTotpProvider", "passwordless-auth");
                    await bus.Publish(new PasswordResetMessage { Data = JsonConvert.SerializeObject(new { user.Email , token ,Name =  $"{user.FirstName} {user.LastName}"   }).ToByteArray() });
                }

            }
            return Ok();

        }
        [HttpPost]
        [Route("reset",Name = "reset")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var isValid = await userManager.VerifyUserTokenAsync(user, "PasswordlessLoginTotpProvider", "passwordless-auth", model.Token);
                    if (isValid)
                    {
                        var token = await userManager.GeneratePasswordResetTokenAsync(user);
                        var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                        if (result.Succeeded)
                        {
                            return Ok();
                        }

                        else
                        {
                            return BadRequest(result);
                        }
                    }

                }

            }
            return BadRequest(ModelState.Select(x => x.Value.Errors));

        }

    }
}
