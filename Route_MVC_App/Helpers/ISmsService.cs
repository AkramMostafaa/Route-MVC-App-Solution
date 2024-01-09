using Route.DAL.Models;
using Twilio.Rest.Api.V2010.Account;

namespace Route_MVC_App.PL.Helpers
{
    public interface ISmsService
    {
        public MessageResource Send(SmsMessage sms);
    }
}
