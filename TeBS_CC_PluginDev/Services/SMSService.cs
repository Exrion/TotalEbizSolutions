using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TeBS_CC_PluginDev.Services
{
    public class SMSService
    {
        private readonly string ACCOUNT_SID = "";
        private readonly string AUTH_TOKEN = "";
        private readonly PhoneNumber FROM_NO = new Twilio.Types.PhoneNumber("+15017122661");

        public void SendSMS(string body, string to)
        {
            TwilioClient.Init(ACCOUNT_SID, AUTH_TOKEN);

            var message = MessageResource.Create(
                body: body,
                from: FROM_NO,
                to: new Twilio.Types.PhoneNumber(to)
            );
        }
    }
}
