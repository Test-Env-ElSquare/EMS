using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BLL.ExternalServices.EmailManagements
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _settings;
        public SmtpEmailSender(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.User, _settings.Password),
                EnableSsl = _settings.EnableSsl
            };
            var mail = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(to);
            await client.SendMailAsync(mail);
        }
        public async Task SendWelcomeEmailAsync(UserWelcomeDTO user)
        {
            var body = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f4f4f4;'>
                <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 8px;'>
                    <h2 style='color: #333;'>Welcome to Our System, {user.UserName} 👋</h2>
                    <p>Hi <strong>{user.UserName}</strong>,</p>
                    <p>Your account has been successfully created by an administrator. Below are your login details:</p>
                    <ul>
                        <li><strong>Email:</strong> {user.Email}</li>
                        <li><strong>Username:</strong> {user.UserName}</li>
                        <li><strong>Temporary Password:</strong> {user.Password}</li>
                        <li><strong>Phone:</strong> {user.PhoneNumber}</li>
                        <li><strong>Assigned Role:</strong> {user.RoleName}</li>
                    </ul>
                    <p style='color: #555;'>Please change your password upon first login for security reasons.</p>
                    <br/>
                    <p style='color: #999;'>This is an automated message. Please do not reply.</p>
                </div>
            </div>";

            await SendEmailAsync(user.Email, "Welcome to Our System", body);
        }

        public async Task SendOtpEmail(string email, string otp)
        {
            var html = $@"
        <!DOCTYPE html>
        <html>
        <head>
          <meta charset='UTF-8'>
          <title>Password Reset Code</title>
          <style>
            body {{
              font-family: Arial, sans-serif;
              background-color: #f4f4f4;
              padding: 0;
              margin: 0;
            }}
            .container {{
              max-width: 600px;
              margin: 40px auto;
              background-color: #ffffff;
              border-radius: 8px;
              overflow: hidden;
              box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            }}
            .header {{
              background-color: #007BFF;
              color: white;
              padding: 20px;
              text-align: center;
              font-size: 24px;
            }}
            .content {{
              padding: 30px;
              text-align: center;
            }}
            .otp {{
              font-size: 36px;
              font-weight: bold;
              letter-spacing: 8px;
              margin: 20px 0;
              color: #007BFF;
            }}
            .footer {{
              font-size: 14px;
              color: #888888;
              padding: 20px;
              text-align: center;
            }}
          </style>
        </head>
        <body>
          <div class='container'>
            <div class='header'>
              Password Reset Request
            </div>
            <div class='content'>
              <p>Hello 👋,</p>
              <p>We received a request to reset your password. Use the code below to reset it:</p>
              <div class='otp'>{otp}</div>
              <p>This code will expire in 5 minutes.</p>
              <p>If you didn't request this, you can safely ignore this email.</p>
            </div>
            <div class='footer'>
              &copy; {DateTime.UtcNow.Year} EIPICO System — All rights reserved.
            </div>
          </div>
        </body>
        </html>";

            await SendEmailAsync(email, "Password Reset Code", html);
        }







    }

    public class UserWelcomeDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public string Password { get; set; }
    }
}
