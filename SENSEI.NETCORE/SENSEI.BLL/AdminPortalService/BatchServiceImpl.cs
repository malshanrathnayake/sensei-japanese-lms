using devspark_core_data_access_layer;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService
{
    public class BatchServiceImpl : IBatchService
    {
        private readonly IDatabaseService _databaseService;

        public BatchServiceImpl(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<(bool, long)> UpdateBatch(Batch batch)
        {
            string userJsonString = JsonConvert.SerializeObject(batch);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.BatchDataManager.UpdateDataReturnPrimaryKey("UpdateBatch", userJsonString);
            return (status, primaryKey);
        }

        public async Task<Batch> GetBatch(long batchId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var batch = await dataTransactionManager.BatchDataManager.RetrieveData("GetBatch", [
                new SqlParameter("@batchId", batchId)
            ]);
            return batch.FirstOrDefault();
        }

        public async Task<(IEnumerable<Batch>, long)> SearchBatches(long courseId = 0, int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (batches, count) = await dataTransactionManager.BatchDataManager.RetrieveDataWithCount("SearchBatches", [
                new SqlParameter("@courseId", courseId),
                new SqlParameter("@start", start),
                new SqlParameter("@length", length),
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection)
            ]);
            return (batches, count);
        }

        public async Task<IEnumerable<Batch>> GetBatches(int courseId = 0)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var batches = await dataTransactionManager.BatchDataManager.RetrieveData("GetBatch", [
                new SqlParameter("@courseId", courseId),
            ]);
            return batches;
        }

        public async Task<bool> DeleteBatch(long batchId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var status = await dataTransactionManager.BatchDataManager.DeleteData("DeleteBatch", [
                new SqlParameter("@batchId", batchId),
            ]);
            return status;
        }
    }
}
