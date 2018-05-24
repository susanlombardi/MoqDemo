using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqDemo
{
    public class WorkingClass
    {
        private IMoqYouService _Service;

        public WorkingClass(IMoqYouService service)
        {
            _Service = service;
        }

        public void NotifyNiceUsers(string userEmail, UserType userType)
        {
            if (userType == UserType.Nice)
            {
                var result = _Service.SendEmail(userEmail, "test@abc.com", "Thanks", "Thanks for being so nice!");
                if (!result)
                {
                    throw new ApplicationException("Unable to send email.");
                }
            }
        }
    }

    public enum UserType
    {
        Annoying = 0,
        Nice = 1,
        Intelligent = 2,
        NobodyHome = 3
    }
}
