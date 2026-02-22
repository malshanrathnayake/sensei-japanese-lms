using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SENSEI.BLL.SystemService.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByUserId(long userId = 0);
        Task<User> GetUserByPhone(string phoneNo = "");
        Task<User> GetUserByEmail(string email = "");
        Task<User> GetUserByUserGlobalIdentity(string userGlobalIdentity = "");
        Task<bool> UpdateOtpSequence(User user);
    }
}
