namespace FumicertiApi.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("sendwpmail")]
    public class SendWpMail
    {
        [Key]
        [Column("sendwpmail_id")]
        public int SendWpMailId { get; set; }

        [Column("sendwpmail_username")]
        [MaxLength(45)]
        public string SendWpMailUserName { get; set; }

        [Column("sendwpmail_wptoken")]
        [MaxLength(255)]
        public string SendWpMailWpToken { get; set; }

        [Column("sendwpmail_balancetoken")]
        [MaxLength(255)]
        public string SendWpMailBalanceToken { get; set; }

        [Column("sendwpmail_created")]
        public DateTime? SendWpMailCreated { get; set; }

        [Column("sendwpmail_updated")]
        public DateTime? SendWpMailUpdated { get; set; }

        [Column("sendwpmail_create_uid")]
        [MaxLength(60)]
        public string SendWpMailCreateUid { get; set; }

        [Column("sendwpmail_edited_uid")]
        [MaxLength(60)]
        public string SendWpMailEditedUid { get; set; }

        [Column("sendwpmail_companyid")]
        public int? SendWpMailCompanyid { get; set; }

    }

}
