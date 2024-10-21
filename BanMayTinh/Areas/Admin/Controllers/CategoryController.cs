using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BanMayTinh.Repository;
using BanMayTinh.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Authorization;
namespace BanMayTinh.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;

		public CategoryController(DataContext context )
		{
			_dataContext = context;
			
		}
		public async Task<IActionResult> Index()
		{

			return View(await _dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
		}


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {


            if (ModelState.IsValid) // ModelState.IsValid là tình trạng của Model nếu ổn thì nó sẽ thực hiện dòng code
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh muc đã có trong database");
                    return View(category);
                }
                _dataContext.Add(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Added Categories successfully";
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

            return View(category);
          
        }




        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);

            _dataContext.Categories.Remove(category );
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "The category has been deleted";
            return RedirectToAction("Index");
        }







        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category)
        {


            if (ModelState.IsValid) // ModelState.IsValid là tình trạng của Model nếu ổn thì nó sẽ thực hiện dòng code
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh muc đã có trong database");
                    return View(category);
                }
                _dataContext.Update(category);
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

            return View(category);

        }
    }


}
