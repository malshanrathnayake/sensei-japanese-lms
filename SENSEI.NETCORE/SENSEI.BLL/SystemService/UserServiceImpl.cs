using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SENSEI.BLL.SystemService
{
    //public class UserServiceImpl : IUserService
    //{
    //    private readonly IDatabaseService _databaseService;

    //    public UserServiceImpl(IDatabaseService databaseService)
    //    {
    //        _databaseService = databaseService;
    //    }

    //    public async Task<bool> InsertUser(EntraIdUser entraIdUser)
    //    {
    //        string userJsonString = JsonConvert.SerializeObject(entraIdUser);
    //        DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
    //        bool status = dataTransactionManager.EntraIdUserDataManager.InsertData("InsertUser", userJsonString);
    //        return status;
    //    }

    //    public async Task<EntraIdUser> GetUserByEntraIdNameIdentifier(string userObjectidentifier = "")
    //    {
    //        DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
    //        return dataTransactionManager.EntraIdUserDataManager.RetrieveData("GetUserByEntraIdNameIdentifier", new SqlParameter[]
    //        {
    //            new SqlParameter("@userObjectidentifier", userObjectidentifier)
    //        }).FirstOrDefault();
    //    }
    //}
}
