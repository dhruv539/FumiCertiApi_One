namespace FumicertiApi.DTOs
{
    public class SendWpMailDto
    {
        public int SendWpMailId { get; set; }
        public string SendWpMailUserName { get; set; }
        public string SendWpMailWpToken { get; set; }
        public string SendWpMailBalanceToken { get; set; }
        public DateTime? SendWpMailCreated { get; set; }
        public DateTime? SendWpMailUpdated { get; set; }
        public string SendWpMailCreateUid { get; set; }
        public string SendWpMailEditedUid { get; set; }
        public int? SendWpMailCompanyid { get; set; }

    }

    public class SendWpMailAddDto
    {
        public string SendWpMailUserName { get; set; }
        public string SendWpMailWpToken { get; set; }
        public string SendWpMailBalanceToken { get; set; }
        public int? SendWpMailCompanyid { get; set; }

    }
}
