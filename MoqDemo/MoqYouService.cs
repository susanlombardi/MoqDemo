using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqDemo
{
    public class MoqYouService : IMoqYouService
    {
        public bool SendEmail(string to, string from, string subject, string body)
        {
            //actual code to send email would be here
            Console.WriteLine("Sending email to client.");
            return true;
        }

    }
}
