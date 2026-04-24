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

        public async Task<(IEnumerable<BatchStudentLessonAccessRequest>, long)> SearchBatchStudentLesson(long courseId = 0, long batchId = 0, string indexNumber = "", int status = -1, int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "")
        {
            if (string.IsNullOrEmpty(sortColumn))
            {
                sortColumn = "requestedDate";
                sortDirection = "DESC";
            }

            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (allRequests, totalCount) = await dataTransactionManager.BatchStudentLessonAccessRequestDataManager.RetrieveDataWithCount("SearchBatchStudentLesson", new[]
            {
                new SqlParameter("@courseId", courseId),
                new SqlParameter("@batchId", batchId),
                new SqlParameter("@indexNumber", indexNumber),
                new SqlParameter("@start", 0), 
                new SqlParameter("@length", 10000), 
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection),
            });

            var filteredList = allRequests.AsQueryable();

            if (status != -1)
            {
                filteredList = filteredList.Where(x => (int)x.ApproveStatusEnum == status);
            }

            var count = filteredList.Count();
            var pagedList = filteredList.Skip(start).Take(length > 0 ? length : 10000).ToList();

            return (pagedList, count);
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

        public async Task<bool> ApproveBatchStudentLessonRequest(long batchStudentLessonRequestId, long userId, DateTime? accessEndDate = null)
        {
            var batchStudentLessonRequest = new BatchStudentLessonAccessRequest()
            {
                BatchStudentLessonAccessRequestId = batchStudentLessonRequestId,
                ApproveStatusEnum = ApproveStatusEnum.Approved,
                RequestEndDate = accessEndDate ?? DateTime.UtcNow.AddDays(1), // Default to 1 day if not provided
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
