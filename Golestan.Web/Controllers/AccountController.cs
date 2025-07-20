using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using System.Security.Claims;
using System.Text.Encodings.Web;
using Application.DTOs.Account;
using Application.Interfaces;
using Base;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Shared.Constants;


public class AccountController : BaseController {

    private readonly SignInManager<AppUser> _signInManager;

    private readonly UserManager<AppUser> _userManager;

    private readonly IEmailService _emailService;

    private readonly IUserService _userService;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFacultyService facultyService, IEmailService emailService, IUserService userService) : base(facultyService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _emailService = emailService;
        _userService = userService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model, string returnUrl = "")
    {
        if (ModelState.IsValid){
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded){
                return RedirectToRoleBasedPage();
            }

            if (result.IsLockedOut){
                ShowMessage("Your account locked out.", false);
            }

            if (result.IsNotAllowed){
                ShowMessage("Email not exist or password is wrong.", false);
            }
        }


        return View(model);
    }

    [HttpGet]
    public IActionResult UniversalNumberLogin()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UniversalNumberLogin(UniNumberLoginDto model)
    {
        if (!ModelState.IsValid){
            return View(model);
        }

        var result = await _userService.UniversalNumberLogin(model);

        if (!result.Succeeded){
            ShowMessage(result.Message, result.Succeeded);

            return View(model);
        }

        return RedirectToAction("RedirectToRoleBasedPage");
    }

    [HttpGet]
    public IActionResult ResetPassword()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ForgetPasswordDto model)
    {
        if (!ModelState.IsValid){
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null){
            return RedirectToAction("ForgetPasswordConfirmation");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var callbackUrl = Url.Action(
        action: "ResetPassword",
        controller: "Account",
        values: new { email = user.Email, token = token },
        protocol: Request.Scheme);

        await _emailService.SendEmailAsync(
        model.Email,
        "Reset Password",
        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        return RedirectToAction("ForgetPasswordConfirmation");
    }

    [HttpGet]
    public IActionResult ResetPassword(string email, string token)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token)){
            ModelState.AddModelError("", "Invalid email or password.");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
    {
        if (!ModelState.IsValid){
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null){
            return RedirectToAction("ForgetPasswordConfirmation");
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

        if (result.Succeeded){
            return RedirectToAction("ForgetPasswordConfirmation");
        }

        foreach (var error in result.Errors){
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ForgetPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    public IActionResult RedirectToRoleBasedPage()
    {
        var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        if (User.Identity.IsAuthenticated){
            if (User.IsInRole(AppRoles.Admin)){
                return RedirectToAction("Index", "Admin");
            }

            if (User.IsInRole(AppRoles.Instructor)){
                return RedirectToAction("Index", "Instructors");
            }

            if (User.IsInRole(AppRoles.Student)){
                return RedirectToAction("Index", "Students");
            }

            return RedirectToAction("AccessDenied", "Account");
        }


        return RedirectToAction("Login", "Account");
    }

}
