using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BanMayTinh.Models
{
	public class UserModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập Username")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập Email"), EmailAddress]
		public string Email { get; set; }

		[DataType(DataType.Password), Required(ErrorMessage = "Yêu cầu nhập PassWord")]
		public string Password { get; set; }


			
	}
}
