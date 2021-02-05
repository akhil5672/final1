using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Mail;
using finance_trial4.Models;

namespace finance_trial4.Controllers
{
    public class EmailController : ApiController
    {
        private financedbEntities9 db = new financedbEntities9();
        public IHttpActionResult sendmail(Email ec)
        {
            var k = db.Customers.Where(x => x.user_email == ec.To).FirstOrDefault();
            if (k != null)
            {
                string to = ec.To;
                string body = ec.Body;
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("financeproject567@gmail.com", "Finance");
                mail.To.Add(to);
                mail.Subject = "OTP for FORGOT PASSWORD";
                mail.Body = body;
                mail.IsBodyHtml = true;
                client.Port = Convert.ToInt32(587);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("financeproject567@gmail.com", "project@123");
                client.EnableSsl = true;


                client.Send(mail);

                return Ok();
            }
            else
            {
                return BadRequest("No such User exists");
            }
        }
    }
}
