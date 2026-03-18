using devspark_core_data_access_layer;
using Newtonsoft.Json;
using SENSEI.BLL.ApiPortalservices.Interfaces;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.ApiPortalservices
{
    public class ExpirationServiceImpl : IExpirationService
    {
        private readonly IDatabaseService _databaseService;

        public ExpirationServiceImpl(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> UpdateLessonOnExpirationAsync()
        {
            var data = new
            {
                TodayDate = DateTime.UtcNow
            };

            string jsonString = JsonConvert.SerializeObject(data);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.DbCommonDataManager.UpdateDataReturnPrimaryKey("UpdateLessonOnExpiration", jsonString);
            return status;
        }
    }
}
