using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Infrastructure.Settings
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; }
        public string EmailSender { get; set; }
    }
}
