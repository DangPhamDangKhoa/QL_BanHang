using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace BanMayTinh.Repository.Validation_XacNhan
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var extention = Path.GetExtension(file.FileName); //123.jpg
                string[] extensions = {"jpg","png","jpeg" };

                bool result = extensions.Any(x => extention.EndsWith(x));

                if(!result)
                {
                    return new ValidationResult("Allowed extension are jpg or png or jpeg"); 
                }
            }
            return ValidationResult.Success;

        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
