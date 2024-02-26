using SsmErrorService.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsmErrorService.Core
{
    public class GenerateSms
    {
        public string GenerateError(List<Error> errors, int interval)
        {
            if (errors == null) 
            {
                throw new ArgumentNullException(nameof(errors));
            }

            if (!errors.Any())
                return string.Empty;

            var sms = $"Błędy z ostatnich {interval} minut.";

            foreach (Error error in errors) 
            {
                sms += $@"{error.Message} z {error.Date:dd-MM-yyyy HH:mm}";
            }

            sms += $"Automatyczna wiadomość wysłana z aplikacji SmsErrorService";

            return sms;
        }
    }
}
