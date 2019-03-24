using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetas.Web.Helpers
{
    public interface IMailHelper
    {
        void SendMail(string to, string subject, string body);
    }
}
