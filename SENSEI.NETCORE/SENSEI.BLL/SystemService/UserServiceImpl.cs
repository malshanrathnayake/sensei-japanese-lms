using devspark_core_data_access_layer;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SENSEI.BLL.SystemService
{
    public class UserServiceImpl : IUserService
    {
        private readonly IDatabaseService _databaseService;

        public UserServiceImpl(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<User> GetUserByUserId(long userId = 0)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var user = await dataTransactionManager.UserDataManager.RetrieveData("GetUser", [
                new SqlParameter("@userId", userId)
                ]);
            return user.FirstOrDefault();
        }

        public async Task<User> GetUserByPhone(string phoneNo = "")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var user = await dataTransactionManager.UserDataManager.RetrieveData("GetUser", [
                new SqlParameter("@phoneNo", phoneNo)
                ]);
            return user.FirstOrDefault();
        }

        public async Task<User> GetUserByEmail(string email = "")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var user = await dataTransactionManager.UserDataManager.RetrieveData("GetUser", [
                new SqlParameter("@email", email)
                ]);
            return user.FirstOrDefault();
        }

        public async Task<User> GetUserByUserGlobalIdentity(string userGlobalIdentity = "")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var user = await dataTransactionManager.UserDataManager.RetrieveData("GetUser", [
                new SqlParameter("@userGlobalIdentity", userGlobalIdentity)
                ]);
            return user.FirstOrDefault();
        }

        public async Task<bool> UpdateOtpSequence(User user)
        {

            string userJsonString = JsonConvert.SerializeObject(user);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.UserDataManager.UpdateDataReturnPrimaryKey("UpdateUserOTPSequence", userJsonString);
            return status;
        }
    }
}
