namespace BLL.ExternalServices.EmailManagements
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendWelcomeEmailAsync(UserWelcomeDTO user);
        Task SendOtpEmail(string email, string otp);

    }
}
