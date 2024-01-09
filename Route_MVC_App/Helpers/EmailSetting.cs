using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Route.DAL.Models;
using Route_MVC_App.PL.Settings;
using System.Net;


namespace Route_MVC_App.PL.Helpers
{
	public  class  EmailSetting:IMailSettings
	{
        private MailSettings _options;

        public EmailSetting(IOptions<MailSettings> options)
		{
				_options = options.Value;	
		}
		//public static void SendEmail(Email email) 
		//{
		//	var client = new SmtpClient("smtp.gmail.com", 587);

		//	client.EnableSsl = true;
		//	client.Credentials = new NetworkCredential("Akrammostafa114@gmail.com", "fhtj	xtnuukpzzhuv");
		//	client.Send("Akrammostafa114@gmail.com", email.To,email.Subject,email.Body);
		//}

        public void SendMail(Email email)
        {
            var mail= new MimeMessage()
			{
				Sender = MailboxAddress.Parse(_options.Email),
				Subject= email.Subject,

			};
			mail.To.Add(MailboxAddress.Parse(email.To));

			var builder = new BodyBuilder();
			builder.TextBody = email.Body;
			mail.Body = builder.ToMessageBody();
			mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));
			using var smtp = new SmtpClient();
			smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
			smtp.Authenticate(_options.Email,_options.Password);
			smtp.Send(mail);
			smtp.Disconnect(true);
        }
    }
}
