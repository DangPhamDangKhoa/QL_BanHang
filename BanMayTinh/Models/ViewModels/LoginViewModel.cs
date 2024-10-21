using System.ComponentModel.DataAnnotations;

namespace BanMayTinh.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập Username")]
		public string Username { get; set; }

		[DataType(DataType.Password), Required(ErrorMessage = "Yêu cầu nhập PassWord")]

		public string Password { get; set; }

		public string ReturnUrl { get; set; }
	}
}
