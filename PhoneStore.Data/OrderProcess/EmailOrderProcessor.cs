using PhoneStore.Data.Abstarct;
using PhoneStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Data.OrderProcess
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "gamestore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"D:\PhoneStore";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(ShoppingCart cart, OrderDetails orderInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                        = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("New order has been processed")
                    .AppendLine("---")
                    .AppendLine("Phones:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Phone.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (total: {2:c}",
                        line.Quantity, line.Phone.Name, subtotal);
                }

                body.AppendFormat("Total price: {0:c}", cart.SumTotalValue())
                    .AppendLine("---")
                    .AppendLine("Delivering:")
                    .AppendLine(orderInfo.Name)
                    .AppendLine(orderInfo.Line1)
                    .AppendLine(orderInfo.Line2 ?? "")
                    .AppendLine(orderInfo.Line3 ?? "")
                    .AppendLine(orderInfo.City)
                    .AppendLine(orderInfo.Country)
                    .AppendLine("---")
                    .AppendFormat("Gift: {0}",
                        orderInfo.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(
                                       emailSettings.MailFromAddress,	// От кого
                                       emailSettings.MailToAddress,		// Кому
                                       "New orded has been send!",		// Тема
                                       body.ToString()); 				// Тело письма
                //
                emailSettings.WriteAsFile = true;
                //
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }

                smtpClient.Send(mailMessage);
            }
        }
    }
}
