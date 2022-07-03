using CSELibrary.Com.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSELibrary.Cse.Com.Mail
{
    public class MailAccesser
    {
        public static string ApiKey { get; set; } = "";
        public static async Task<Response> SendMailAsync(MailSender mailSender)
        {
            var client = new SendGridClient(ApiKey);
            var from = new EmailAddress(mailSender.FromAddress, mailSender.FromName);
            var subject = mailSender.Subject;
            var to = new EmailAddress(mailSender.ToAddress, mailSender.ToName);
            var plainTextContent = mailSender.PlainTextContent;
            var htmlContent = mailSender.HtmlContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            // 添付ファイル
            mailSender.FileDtoList.ForEach(f => {
                var file = Convert.ToBase64String(f.Binary);
                msg.AddAttachment(f.FileName, file, f.DataType, "attachment");
            });
            return response;
        }

    }

    public class MailSender { 
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string ToAddress { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassWord { get; set; }
        public List<FileDto> FileDtoList { get; set; } = new List<FileDto>();
    }
}