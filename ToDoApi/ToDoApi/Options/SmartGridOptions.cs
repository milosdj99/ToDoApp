using SendGrid.Helpers.Mail;

namespace ToDoApi
{
    public class SmartGridOptions
    {

        public const string SMOptions = "SMOptions";
        public string Apikey { get; set; }
        public string From { get; set; }
        public string From2 { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string To2 { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }

        public int Interval { get; set; }

    }
}
