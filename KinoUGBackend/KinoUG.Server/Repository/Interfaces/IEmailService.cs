namespace KinoUG.Server.Repository.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string html, string text);
    }
}
