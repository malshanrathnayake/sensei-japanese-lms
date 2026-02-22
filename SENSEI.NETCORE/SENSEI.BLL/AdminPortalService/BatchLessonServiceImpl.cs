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
    public class BatchLessonServiceImpl : IBatchLessonService
    {
        private readonly IDatabaseService _databaseService;

        public BatchLessonServiceImpl(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<(bool, long)> UpdateBatchLesson(BatchLesson batchLesson)
        {
            string jsonString = JsonConvert.SerializeObject(batchLesson);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.BatchLessonDataManager.UpdateDataReturnPrimaryKey("UpdateBatchLesson", jsonString);
            return (status, primaryKey);
        }

        public async Task<BatchLesson> GetBatchLesson(long batchLessonId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var batchLesson = await dataTransactionManager.BatchLessonDataManager.RetrieveData("GetBatchLesson", [
                new SqlParameter("@batchLessonId", batchLessonId)
            ]);
            return batchLesson.FirstOrDefault();
        }

        public async Task<(IEnumerable<BatchLesson>, long)> SearchBatchLessons(long courseId = 0, long batchId = 0, int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (batchLessons, count) = await dataTransactionManager.BatchLessonDataManager.RetrieveDataWithCount("SearchBatchLessons", [
                new SqlParameter("@courseId", courseId),
                new SqlParameter("@batchId", batchId),
                new SqlParameter("@start", start),
                new SqlParameter("@length", length),
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection)
            ]);
            return (batchLessons, count);
        }

        public async Task<IEnumerable<BatchLesson>> GetBatchLessons(long batchId = 0)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var batchLessons = await dataTransactionManager.BatchLessonDataManager.RetrieveData("GetBatchLesson", [
                new SqlParameter("@batchId", batchId),
            ]);
            return batchLessons;
        }

        public async Task<bool> DeleteBatchLesson(long batchLessonId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var status = await dataTransactionManager.BatchLessonDataManager.DeleteData("DeleteBatchLesson", [
                new SqlParameter("@batchLessonId", batchLessonId),
            ]);
            return status;
        }
    }
}
