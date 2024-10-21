using Microsoft.EntityFrameworkCore;
using BanMayTinh.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BanMayTinh.Repository
{
	public class DataContext : IdentityDbContext<AppUserModel>
    {
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }



		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }


    }
}
