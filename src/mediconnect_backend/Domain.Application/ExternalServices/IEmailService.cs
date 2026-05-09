using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Application.ExternalServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
