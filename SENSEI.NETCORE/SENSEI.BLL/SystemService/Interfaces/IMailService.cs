using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SENSEI.BLL.SystemService.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendGoogleMail(string receiverMail, string mailSubject, string mailBody);
        Task<bool> SendGoogleMailForMultiple(List<string> receiverMail, string mailSubject, string mailBody);
        Task<bool> SendGraphMail(string receiver, string subject, string body);
    }
}
