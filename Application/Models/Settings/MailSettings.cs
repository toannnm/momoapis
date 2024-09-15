namespace Application.Models.Settings
{
    public class TemplateParams
    {
        public string Email { get; set; }
    }

    public class MailSection
    {
        public string ServiceId { get; set; }
        public string TemplateId { get; set; }
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public string Title { get; set; }
        public string BaseUrlSend { get; set; }
        public string ServiceName { get; set; }
        public string MediaType { get; set; }
        public TemplateParams TemplateParams { get; set; }
        public string Sign { get; set; }
    }

}
