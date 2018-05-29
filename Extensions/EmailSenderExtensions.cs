using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FiveDevsShop.Services;

namespace FiveDevsShop.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Patvirtinkite registraciją",
                $"Norėdami baigti registraciją elektroninėje parduotuvėj, patvirtinkite savo elektroninį laišką.<a href='{HtmlEncoder.Default.Encode(link)}'>Patvirtinti.</a>");
        }
    }
}
