using System.ComponentModel.DataAnnotations;

namespace Route_MVC_App.PL.ViewModels
{
	public class SignInViewModel
	{
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
        public bool RemeberMe { get; set; }

    }
}
