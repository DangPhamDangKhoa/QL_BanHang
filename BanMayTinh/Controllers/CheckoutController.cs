using BanMayTinh.Models;
using BanMayTinh.Models.ViewModels;
using BanMayTinh.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BanMayTinh.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		public CheckoutController(DataContext context)
		{
			_dataContext = context;
		}

		public async Task<IActionResult> Checkout()
		{
			var userEmail =  User.FindFirstValue(ClaimTypes.Email);
			if(userEmail == null)
			{
				return RedirectToAction("Login","Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
			}
			return View();
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
