using System.ComponentModel.DataAnnotations;

namespace Route_MVC_App.PL.ViewModels
{
	public class ForgetPasswordViewModel
	{
		[Required(ErrorMessage ="Email Is Required")]
		[EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }
    }
}
