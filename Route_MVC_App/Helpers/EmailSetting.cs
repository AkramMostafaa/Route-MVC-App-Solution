using Route.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Route_MVC_App.PL.Helpers
{
	public static class EmailSetting
	{
		public static void SendEmail(Email email) 
		{
			var client = new SmtpClient("smtp.gmail.com", 587);

			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("Akrammostafa114@gmail.com", "fhtj	xtnuukpzzhuv");
			client.Send("Akrammostafa114@gmail.com", email.To,email.Subject,email.Body);
		}
	}
}
