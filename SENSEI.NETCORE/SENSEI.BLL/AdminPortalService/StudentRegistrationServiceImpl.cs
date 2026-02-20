using devspark_core_data_access_layer;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using SENSEI.SignalR.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService
{
    public class StudentRegistrationServiceImpl : IStudentRegistrationService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IRealtimeNotifier _realtimeNotifier;

        public StudentRegistrationServiceImpl(IDatabaseService databaseService, IRealtimeNotifier realtimeNotifier)
        {
            _databaseService = databaseService;
            _realtimeNotifier = realtimeNotifier;
        }

        public async Task<(bool, long)> UpdateStudentRegistraion(StudentRegistration studentRegistration)
        {
            string jsonString = JsonConvert.SerializeObject(studentRegistration);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.StudentRegistrationDataManager.UpdateDataReturnPrimaryKey("UpdateStudentRegistraion", jsonString);

            var userNotification = new UserNotification
            {
                UserNotificationId = 0,
                NotificationType = "Student Register",
                Message = $"A new Student registration has been updated.",
                Icon = "check-circle",
                CreatedAt = DateTime.UtcNow
            };

            await _realtimeNotifier.NotifyAll(new
            {
                id = userNotification.UserNotificationId,
                title = userNotification.NotificationType,
                message = userNotification.Message,
                icon = userNotification.Icon,
                createdAt = userNotification.CreatedAt
            });

            return (status, primaryKey);
        }

        public async Task<(IEnumerable<StudentRegistration>, long)> SearchStudentRegistraion(long courseId = 0, int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (registrations, count) = await dataTransactionManager.StudentRegistrationDataManager.RetrieveDataWithCount("SearchStudentRegistrations", [
                new SqlParameter("@courseId", courseId),
                new SqlParameter("@start", start),
                new SqlParameter("@length", length),
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection)
            ]);
            return (registrations, count);
        }

        public async Task<StudentRegistration> GetStudentRegistraion(long studentRegistrationId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var studentRegistration = await dataTransactionManager.StudentRegistrationDataManager.RetrieveData("GetStudentRegistration", [
                new SqlParameter("@studentRegistrationId", studentRegistrationId)
            ]);
            return studentRegistration.FirstOrDefault();
        }

        public async Task<bool> ApproveStudentRegistraion(long studentRegistrationId, string indexNumber, long batchId, long approvedById)
        {
            var approveData = new
            {
                StudentRegistrationId = studentRegistrationId,
                IndexNumber = indexNumber,
                BatchId = batchId,
                ApprovedById = approvedById
            };

            string jsonString = JsonConvert.SerializeObject(approveData);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.StudentRegistrationDataManager.UpdateDataReturnPrimaryKey("ApproveStudentRegistraion", jsonString);
            return status;
        }
        
    }
}
