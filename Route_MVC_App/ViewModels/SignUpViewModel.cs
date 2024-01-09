using System.ComponentModel.DataAnnotations;

namespace Route_MVC_App.PL.ViewModels
{
	public class SignUpViewModel
	{
        [Required(ErrorMessage ="User Name Is Required")]
        [MinLength(5,ErrorMessage ="Minimum Length Is 5")]
        public string UserName { get; set; }
		[Required(ErrorMessage = "First Name Is Required")]

		public string FName { get; set; }
		[Required(ErrorMessage = "Last Name Is Required")]

		public string LName { get; set; }
        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password Is Required")]
        [DataType(DataType.Password)]
        [MinLength(5,ErrorMessage ="Minimum Password Length is 5")]
        public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password Is Required")]
		[Compare(nameof(Password),ErrorMessage ="Confirm Password does not match  Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }

    }
}
