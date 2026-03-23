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
    public class StudentPaymentServiceImpl : IStudentPaymentService
    {

        private readonly IDatabaseService _databaseService;
        private readonly IRealtimeNotifier _realtimeNotifier;

        public StudentPaymentServiceImpl(IDatabaseService databaseService, IRealtimeNotifier realtimeNotifier)
        {
            _databaseService = databaseService;
            _realtimeNotifier = realtimeNotifier;
        }

        public async Task<(bool, long)> UpdateStudentBatchPayment(StudentBatchPayment studentBatchPayment)
        {
            string jsonString = JsonConvert.SerializeObject(studentBatchPayment);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.StudentBatchPaymentDataManager.UpdateDataReturnPrimaryKey("UpdateStudentBatchPayment", jsonString);

            return (status, primaryKey);
        }

        public async Task<(IEnumerable<StudentBatchPayment>, long)> SearchStudentBatchPayment(long courseId = 0, long batchId = 0, string indexNumber = "", int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (registrations, count) = await dataTransactionManager.StudentBatchPaymentDataManager.RetrieveDataWithCount("SearchStudentPayments", new[]
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

        public async Task<StudentBatchPayment> GetStudentBatchPayment(long studentBatchPaymentId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var studentBatchPayment = await dataTransactionManager.StudentBatchPaymentDataManager.RetrieveData("GetStudentBatchPayment", new[]
            {
                new SqlParameter("@paymentId", studentBatchPaymentId)
            });
            return studentBatchPayment.FirstOrDefault();
        }

        public async Task<bool> ApproveStudentBatchPayment(long studentBatchPaymentId, long approvedById)
        {
            var approveData = new
            {
                StudentBatchPaymentId = studentBatchPaymentId,
                ApprovedById = approvedById
            };

            string jsonString = JsonConvert.SerializeObject(approveData);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.StudentBatchPaymentDataManager.UpdateDataReturnPrimaryKey("ApproveStudentBatchPayment", jsonString);
            return status;
        }

        public async Task<bool> RejectStudentBatchPayment(long studentBatchPaymentId, long rejectedById)
        {
            var rejectData = new
            {
                StudentBatchPaymentId = studentBatchPaymentId,
                RejectedById = rejectedById
            };

            string jsonString = JsonConvert.SerializeObject(rejectData);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.StudentBatchPaymentDataManager.UpdateDataReturnPrimaryKey("RejectStudentBatchPayment", jsonString);
            return status;
        }

        public async Task<IEnumerable<dynamic>> GetStudentBatches(long studentId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            return await dataTransactionManager.StudentBatchPaymentDataManager.RetrieveDynamicData("GetStudentBatchesList", new SqlParameter[] {
                new SqlParameter("@studentId", studentId)
            });
        }

    }
}
