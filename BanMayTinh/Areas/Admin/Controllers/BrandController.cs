using Microsoft.AspNetCore.Mvc;
using BanMayTinh.Repository;
using BanMayTinh.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;

namespace BanMayTinh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Brand")]
    [Authorize]
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;

        public BrandController(DataContext context)
        {
            _dataContext = context;

        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
        }

        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {


            if (ModelState.IsValid) // ModelState.IsValid là tình trạng của Model nếu ổn thì nó sẽ thực hiện dòng code
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh muc đã có trong database");
                    return View(brand);
                }
                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Brand added successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có vài thứ bị lỗi cần coi lại";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(brand);

        }

        [Route("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);

            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "The brands has been deleted";
            return RedirectToAction("Index");
        }


        [Route("Edit")]
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);
            return View(brand);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {


            if (ModelState.IsValid) // ModelState.IsValid là tình trạng của Model nếu ổn thì nó sẽ thực hiện dòng code
            {
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh muc đã có trong database");
                    return View(brand);
                }


           
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Updated directory successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có vài thứ bị lỗi cần coi lại";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(brand);

        }
    }
}
