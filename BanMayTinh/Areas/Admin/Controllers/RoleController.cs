﻿using BanMayTinh.Models;
using BanMayTinh.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.InteropServices;

namespace BanMayTinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Role")]
    public class RoleController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(DataContext context, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = context;
            _roleManager= roleManager;

        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Roles.OrderByDescending(p => p.Id).ToListAsync());
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(IdentityRole model)
        {
            if(!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            }

            return Redirect("Index");

        }
    }
}
