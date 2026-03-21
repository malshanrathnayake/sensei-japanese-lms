using devspark_core_data_access_layer;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using SENSEI.SIGNALR.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService
{
    public class BatchStudentLessonServiceImpl : IBatchStudentLessonService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IRealtimeNotifier _realtimeNotifier;

        public BatchStudentLessonServiceImpl(IDatabaseService databaseService, IRealtimeNotifier realtimeNotifier)
        {
            _databaseService = databaseService;
            _realtimeNotifier = realtimeNotifier;
        }

        public async Task<(IEnumerable<BatchStudentLessonAccessRequest>, long)> SearchBatchStudentLesson(long courseId = 0, long batchId = 0, string indexNumber = "", int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (registrations, count) = await dataTransactionManager.BatchStudentLessonAccessRequestDataManager.RetrieveDataWithCount("SearchBatchStudentLesson", new[]
            {
                new SqlParameter("@courseId", courseId),
                new SqlParameter("@batchId", batchId),
                new SqlParameter("@indexNumber", indexNumber),
                new SqlParameter("@start", start),
                new SqlParameter("@length", length),
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection),
            });
            return (registrations, count);
        }

        public async Task<BatchStudentLessonAccessRequest> GetBatchStudentLessonRequest(long batchStudentLessonRequestId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var studentBatchPayment = await dataTransactionManager.BatchStudentLessonAccessRequestDataManager.RetrieveData("GetBatchStudentLessonRequest", new[]
            {
                new SqlParameter("@batchStudentLessonRequestId", batchStudentLessonRequestId)
            });
            return studentBatchPayment.FirstOrDefault();
        }

        public async Task<bool> ApproveBatchStudentLessonRequest(long batchStudentLessonRequestId, long userId)
        {
            var batchStudentLessonRequest = new BatchStudentLessonAccessRequest()
            {
                BatchStudentLessonAccessRequestId = batchStudentLessonRequestId,
                ApproveStatusEnum = ApproveStatusEnum.Approved,
                ChangeById = userId,
                ChangedDate = DateTime.UtcNow
            };

            string jsonString = JsonConvert.SerializeObject(batchStudentLessonRequest);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.BatchStudentLessonAccessRequestDataManager.UpdateDataReturnPrimaryKey("ApproveBatchStudentLessonRequest", jsonString);

            return status;
        }

    }
}
