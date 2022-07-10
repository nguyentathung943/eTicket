
using eTickets.Data;
using eTickets.Data.Static;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers;

[Authorize(Roles = UserRoles.Admin)]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AppDbContext _context;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManage,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManage;
        _context = context;
    }

    public async Task<IActionResult> Users()
    {
        var users = await _context.Users.ToListAsync();
        return View(users);
    }

    // GET
    [AllowAnonymous]
    public IActionResult Login() => View(new LoginVM());

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginVM loginVm)
    {
        bool isValidCredential = false;
        if (!ModelState.IsValid)
        {
            return View(loginVm);
        }

        var user = await _userManager.FindByEmailAsync(loginVm.EmailAddress);
        if (user != null)
        {
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVm.Password);
            if (passwordCheck)
            {
                isValidCredential = true;
                var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Movies");
                }
            }
        }

        if (!isValidCredential)
        {
            TempData["Error"] = "Wrong credentials. Please try again!";
            return View(loginVm);
        }

        return null;
    }

    // GET
    [AllowAnonymous]
    public IActionResult Register() => View(new RegisterVM());

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid) return View(registerVM);

        var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
        if (user != null)
        {
            TempData["Error"] = "This email address is already in use";
            return View(registerVM);
        }

        var newUser = new ApplicationUser()
        {
            FullName = registerVM.FullName,
            Email = registerVM.EmailAddress,
            UserName = registerVM.EmailAddress
        };
        var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

        if (newUserResponse.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            return View("RegisterCompleted");
        }
        else
        {
            TempData["Error"] = "Password Not Strong Enough!";
            return View(registerVM);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Movies");
    }

    public IActionResult AccessDenied(string ReturnUrl)
    {
        return View();
    }

}