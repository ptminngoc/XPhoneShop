using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Specialized;
using DocumentFormat.OpenXml.Vml;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit;
using System.Net;
using System.Text;

namespace XPhone_Shop_TKPM
{
    public class EMail
    {
        public static string Address = null; //Địa chỉ email của bạn
        public static string Password = null; //Mật khẩu ứng dụng

        public Boolean Send(string sendTo, string OTP)
        {
            //Kiểm tra email có tồn tại không
            //if (VerifyEmail(sendTo) == false)
            //    return false;

            //Nếu có tồn tại
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("XPhone Shop", "pngoc0105@gmail.com"));
                email.To.Add(new MailboxAddress("Receiver Name", sendTo));

                email.Subject = "Xác nhận đăng kí tài khoản XPhone Shop";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = "Mã OTP của bạn là: " + OTP + "\r\n" + "Vui lòng không chia sẻ mã OTP với người khác!"
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);
                    smtp.Authenticate("pngoc0105@gmail.com", "owjhocaamlznldjg");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }

                return true;

            } catch(FormatException)
            {
                return false;
            }
        }

        public bool VerifyEmail(string emailVerify)
        {
            using (WebClient webclient = new WebClient())
            {
                string url = "http://verify-email.org/";
                NameValueCollection formData = new NameValueCollection();
                formData["check"] = emailVerify;
                byte[] responseBytes = webclient.UploadValues(url, "POST", formData);
                string response = Encoding.ASCII.GetString(responseBytes);
                if (response.Contains("Result: Ok"))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
