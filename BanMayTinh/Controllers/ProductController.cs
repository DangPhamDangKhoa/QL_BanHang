using Microsoft.AspNetCore.Mvc;
using BanMayTinh.Repository;

namespace BanMayTinh.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;

		public ProductController(DataContext context)
		{
			_dataContext = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Details(int id)
		{
			if (id == null) return RedirectToAction("Index");
			var productsById = _dataContext.Products.Where(p => p.Id == id).FirstOrDefault();


			return View(productsById);
		}
	}
	
}
