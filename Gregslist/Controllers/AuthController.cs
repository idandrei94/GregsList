using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gregslist.Models;
using Gregslist.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gregslist.Controllers
{
    public class AuthController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] AuthViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userManager.CreateAsync(new User
            {
                Email = formData.Email,
                UserName = formData.Email
            }, formData.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, err.Description);
                }
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] AuthViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(formData.Email, formData.Password, false, true);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Login failed.");
                if(result.IsLockedOut)
                {
                    ModelState.AddModelError(String.Empty, "Too many failed attempts, you are locked out. Try again later.");
                }
                return View();
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}