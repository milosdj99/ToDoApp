using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;
using ToDoInfrastructure;

namespace ToDoApi
{
    public class RemainderService : IHostedService
    {

        private readonly IServiceScopeFactory _scopeFactory;

        private readonly IConfiguration _configuration;

        private Timer _timer;

        public RemainderService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            Log.Debug("RemainderService started!");

            var smartGridOptions = new SmartGridOptions();

            _configuration.GetSection(SmartGridOptions.SMOptions).Bind(smartGridOptions);

            _timer = new Timer(SendEmail, null, TimeSpan.Zero, TimeSpan.FromSeconds(smartGridOptions.Interval));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);

            Log.Debug("RemainderService ended execution!");

            return Task.CompletedTask;
        }

        private void SendEmail(object? source)
        {
            var scope = _scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();

            dbContext.ToDoLists.Where(x => x.ReminderDate < DateTime.Now && !x.Notified).Select(x => x.Id).ToList().ForEach(x => Send(x));

            int size = dbContext.ToDoLists.Where(x => x.ReminderDate < DateTime.Now && !x.Notified).ToList().Count;

            Log.Debug($"RemainderService found {size} expired remainders");


        }

        private void Send(Guid id)
        {
            var smartGridOptions = new SmartGridOptions();

            _configuration.GetSection(SmartGridOptions.SMOptions).Bind(smartGridOptions);

            Environment.SetEnvironmentVariable("SENDGRID_API_KEY", smartGridOptions.Apikey);

            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(smartGridOptions.From, smartGridOptions.From2);
            var subject = smartGridOptions.Subject;
            var to = new EmailAddress(smartGridOptions.To, smartGridOptions.To2);
            var plainTextContent = String.Format(smartGridOptions.PlainTextContent, id);
            var htmlContent = smartGridOptions.HtmlContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            client.SendEmailAsync(msg);
        }
    }
}
