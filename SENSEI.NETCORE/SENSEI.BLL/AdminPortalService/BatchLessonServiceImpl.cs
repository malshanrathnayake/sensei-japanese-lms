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
            var batchLesson = await dataTransactionManager.BatchLessonDataManager.RetrieveData("GetBatchLesson", new SqlParameter[] {
                new SqlParameter("@batchLessonId", batchLessonId)
            });
            return batchLesson.FirstOrDefault();
        }

        public async Task<(IEnumerable<BatchLesson>, long)> SearchBatchLessons(long courseId = 0, long batchId = 0, string typeFilter = "all", int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "")
        {
            if (string.IsNullOrEmpty(sortColumn))
            {
                sortColumn = "lessonDateTime";
                sortDirection = "DESC";
            }

            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (allLessons, totalCount) = await dataTransactionManager.BatchLessonDataManager.RetrieveDataWithCount("SearchBatchLessons", new SqlParameter[] {
                new SqlParameter("@courseId", courseId),
                new SqlParameter("@batchId", batchId),
                new SqlParameter("@start", 0), 
                new SqlParameter("@length", 10000), 
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection)
            });

            var filteredList = allLessons.AsQueryable();

            if (typeFilter == "recordings")
            {
                filteredList = filteredList.Where(x => !string.IsNullOrEmpty(x.RecordingUrl));
            }
            else if (typeFilter == "upcoming")
            {
                filteredList = filteredList.Where(x => x.LessonDateTime > DateTime.Now);
            }

            var count = filteredList.Count();
            var pagedList = filteredList.Skip(start).Take(length > 0 ? length : 10000).ToList();

            return (pagedList, count);
        }

        public async Task<IEnumerable<BatchLesson>> GetBatchLessons(long batchId = 0)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var batchLessons = await dataTransactionManager.BatchLessonDataManager.RetrieveData("GetBatchLesson", new SqlParameter[] {
                new SqlParameter("@batchId", batchId),
            });
            return batchLessons;
        }

        public async Task<bool> DeleteBatchLesson(long batchLessonId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var status = await dataTransactionManager.BatchLessonDataManager.DeleteData("DeleteBatchLesson", new SqlParameter[] {
                new SqlParameter("@batchLessonId", batchLessonId),
            });
            return status;
        }

        public async Task<(bool, long)> UpdateBatchLessonReference(BatchLessonReference batchLessonReference)
        {
            string jsonString = JsonConvert.SerializeObject(batchLessonReference);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.BatchLessonReferenceDataManager.UpdateDataReturnPrimaryKey("UpdateBatchLessonReference", jsonString);
            return (status, primaryKey);
        }

        public async Task<bool> DeleteBatchLessonReference(long batchLessonReferenceId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var status = await dataTransactionManager.BatchLessonDataManager.DeleteData("DeleteBatchLessonReference", new SqlParameter[] {
                new SqlParameter("@batchLessonReferenceId", batchLessonReferenceId),
            });
            return status;
        }
    }
}
