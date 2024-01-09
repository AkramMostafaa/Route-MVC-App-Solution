using Route.DAL.Models;

namespace Route_MVC_App.PL.Helpers
{
    public interface IMailSettings
    {
        public void SendMail(Email email);

    }
}
