using Cipher;
using SmsSender;
using SsmErrorService.Core;
using SsmErrorService.Core.Repositories;
using System;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using Twilio.Rest.Accounts.V1;

namespace SmsService
{
    public partial class SmsErrorService : ServiceBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly int _intervalInMinutes;
        private readonly Timer _timer;
        private readonly ErrorRepository _repository = new ErrorRepository();
        private readonly SmsMessage _sms;
        private readonly GenerateSms _generateSms = new GenerateSms();
        private readonly StringCipher _stringCipher = new StringCipher("CA03E51D-9271-4C60-8E50-0EB5AAEA1014");

        public SmsErrorService()
        {
            InitializeComponent();

            try
            {
                _intervalInMinutes = int.Parse(ConfigurationManager.AppSettings["IntervalInMinutes"]);
                _timer = new Timer(_intervalInMinutes * 60000);
                _sms = new SmsMessage(new SmsParams
                { AccountSid = ConfigurationManager.AppSettings["AccountSid"],
                  AuthToken = DecryptSenderSmsPassword(),
                  From = ConfigurationManager.AppSettings["SenderPhoneNumber"],
                  To = ConfigurationManager.AppSettings["ReceiverPhoneNumber"]
            });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }

        }

        private string DecryptSenderSmsPassword()
        {
            var encryptedPassword = ConfigurationManager.AppSettings["AuthToken"];

            if (encryptedPassword.StartsWith("encrypt:"))
            {
                encryptedPassword = _stringCipher.Encrypt(encryptedPassword.Replace("encrypt:", ""));

                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configFile.AppSettings.Settings["AuthToken"].Value = encryptedPassword;
                configFile.Save();
            }

            return _stringCipher.Decrypt(encryptedPassword);
        }

        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += DoWork;
            _timer.Start();
            Logger.Info("Service started ...");
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                SendSms();

            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private void SendSms()
        {

            var errors = _repository.GetLastErrors(_intervalInMinutes);

            if (errors == null || !errors.Any())
                return;

            _sms.Send( _generateSms.GenerateError(errors, _intervalInMinutes));

            Logger.Info("Error sent ...");
        }

        protected override void OnStop()
        {
            Logger.Info("Service stopped ...");
        }
    }
}
