using BanMayTinh.Models;
using BanMayTinh.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BanMayTinh.Controllers
{
    public class AccountController : Controller
    {
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _singInManager;


		public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> singInManager)
		{
			_userManager = userManager;
			_singInManager = singInManager;
		}

		public IActionResult Login(string returnUrl)
        {
			return View(new LoginViewModel {ReturnUrl=returnUrl });
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _singInManager.PasswordSignInAsync(loginVM.Username,loginVM.Password, false, false);
				if (result.Succeeded)
				{
				
					return Redirect(loginVM.ReturnUrl ?? "/");
				}
				ModelState.AddModelError("", "Invalid Username and Password");
			}

			return View(loginVM);
		}



		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(UserModel user)
		{
			if(ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel { UserName = user.Username, Email= user.Email };
				IdentityResult result= await _userManager.CreateAsync(newUser, user.Password);
				if (result.Succeeded) 
				{
					TempData["success"] = "Account created successfull";
					//return RedirectToAction("Index", "Home");
					return Redirect("/account/login");
				}
				foreach(IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

			}
			return View(user);
		}

		public async Task<IActionResult> Logout(string returnUrl="/")
		{
			await _singInManager.SignOutAsync();
			return Redirect(returnUrl);
		}

	}
}
