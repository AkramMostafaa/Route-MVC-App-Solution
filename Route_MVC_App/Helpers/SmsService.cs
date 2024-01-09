using Microsoft.Extensions.Options;
using Route.DAL.Models;
using Route_MVC_App.PL.Settings;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Route_MVC_App.PL.Helpers
{
    public class SmsService : ISmsService
    {
        private readonly TwillioSettings _options;

        public SmsService(IOptions<TwillioSettings> options)
        {
            _options = options.Value;
        }
        public MessageResource Send(SmsMessage sms)
        {
            TwilioClient.Init(_options.AccountSID, _options.AuthToken);
            var result = MessageResource.Create(
                body:sms.Body,
                from:new  Twilio.Types.PhoneNumber(_options.TwillioPhoneNumber),
                to: sms.PhoneNumber

                );
            return result;
        }
    }
}
