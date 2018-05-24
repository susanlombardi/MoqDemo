using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqDemo
{
    public interface IMoqYouService
    {
        bool SendEmail(string to, string from, string subject, string body);
    }
}
