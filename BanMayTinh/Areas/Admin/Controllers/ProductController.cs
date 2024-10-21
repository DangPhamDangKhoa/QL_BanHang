using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanMayTinh.Repository;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BanMayTinh.Models;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace BanMayTinh.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
    public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
		{
			_dataContext = context;
            _webHostEnvironment = webHostEnvironment;

        }
		public async Task< IActionResult> Index()
		{

			return View(await _dataContext.Products.OrderByDescending(p=>p.Id).Include(p=>p.Category).Include(p => p.Brand).ToListAsync());
		}
        [HttpGet]
        public  IActionResult Create()
        {
			ViewBag.Categories = new SelectList(_dataContext.Categories,"Id","Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands,"Id","Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
		{
				
			ViewBag.Categories = new SelectList(_dataContext.Categories,"Id","Name",product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands,"Id","Name",product.BrandId);

            if(ModelState.IsValid) // ModelState.IsValid là tình trạng của Model nếu ổn thì nó sẽ thực hiện dòng code
            {
                product.Slug= product.Name.Replace(" ","-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p=>p.Slug == product.Slug);
                if(slug != null) 
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(product);
                }
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }
                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Added product successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model có vài thứ bị lỗi cần coi lại";
                List<string> errors = new List<string>();
                foreach(var value in ModelState.Values)
                {
                    foreach(var error in value.Errors) 
					{
						errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
			
            return View(product);
		}






        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            return View(product);
        }
        //[Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductModel product)
        {

            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            var existed_product = _dataContext.Products.Find(product.Id); // tìm sp

            if (ModelState.IsValid) // ModelState.IsValid là tình trạng của Model nếu ổn thì nó sẽ thực hiện dòng code
            {
                product.Slug = product.Name.Replace(" ", "-");
                
             
                if (product.ImageUpload != null)
                {

                 
                    //upload new image
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    // delete old
                    string oldfilePath = Path.Combine(uploadsDir, existed_product.Image);
                    try
                    {

                        if (System.IO.File.Exists(oldfilePath))
                        {
                            System.IO.File.Delete(oldfilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while deleting the product image.");
                    }

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_product.Image = imageName;


                   
                }

                existed_product.Name= product.Name;
                existed_product.BrandId = product.BrandId;
                existed_product.CategoryId = product.CategoryId;
                existed_product.Description = product.Description;
                existed_product.Price = product.Price;

                _dataContext.Update(existed_product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Product update successful";
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

            return View(product);
        }



        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);

            if (product == null)
            {
                return NotFound();
            }

            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
            string oldfilePath = Path.Combine(uploadsDir, product.Image);
            try
            {
                
                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", "An error occurred while deleting the product image.");
            }
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "product has been deleted";
            return RedirectToAction("Index");
        }
    }

	
}
