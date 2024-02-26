using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SmsSender
{
    public class SmsMessage
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _from;
        private readonly string _to;

        public SmsMessage(SmsParams smsParams)
        {
            _accountSid = smsParams.AccountSid; 
            _authToken = smsParams.AuthToken;
            _from = smsParams.From;
            _to = smsParams.To;
        }

        public void Send(string smsBody)
        {           
            TwilioClient.Init(_accountSid, _authToken);

            var message = MessageResource.Create(
                body: smsBody,
                from: new Twilio.Types.PhoneNumber(_from),
                to: new Twilio.Types.PhoneNumber(_to)
            );

            Console.WriteLine(message.Sid);
        }

    }
}
