﻿using BanMayTinh.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanMayTinh.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderController : Controller
	{
		private readonly DataContext _dataContext;
		public OrderController(DataContext context)
		{
			_dataContext = context;

		}
		public async Task<IActionResult> Index()
		{

			return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
		}
	}
}