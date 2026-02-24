using devspark_core_data_access_layer;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SENSEI.BLL.StudentPortalService.Interfaces;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SENSEI.BLL.StudentPortalService
{
    public class StudentServiceImpl : IStudentService
    {
        private readonly IDatabaseService _databaseService;

        public StudentServiceImpl(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<Student> GetStudentProfile(long userId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var student = await dataTransactionManager.StudentDataManager.RetrieveData("GetStudentProfile", new SqlParameter[] {
                new SqlParameter("@userId", userId)
            });
            return student.FirstOrDefault();
        }

        public async Task<(IEnumerable<Batch>, long)> SearchStudentBatches(long studentId, int start = 0, int length = 10, string searchValue = "", string sortColumn = "batchName", string sortDirection = "ASC")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (batches, count) = await dataTransactionManager.BatchDataManager.RetrieveDataWithCount("SearchStudentBatches", new SqlParameter[] {
                new SqlParameter("@studentId", studentId),
                new SqlParameter("@start", start),
                new SqlParameter("@length", length),
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection)
            });
            return (batches, count);
        }

        public async Task<(IEnumerable<BatchLesson>, long)> SearchStudentBatchLessons(long studentId, long batchId = 0, int start = 0, int length = 10, string searchValue = "", string sortColumn = "lessonDateTime", string sortDirection = "DESC")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (lessons, count) = await dataTransactionManager.BatchLessonDataManager.RetrieveDataWithCount("SearchStudentBatchLessons", new SqlParameter[] {
                new SqlParameter("@studentId", studentId),
                new SqlParameter("@batchId", batchId),
                new SqlParameter("@start", start),
                new SqlParameter("@length", length),
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection)
            });
            return (lessons, count);
        }

        public async Task<(IEnumerable<Course>, long)> SearchStudentCourses(long studentId, int start = 0, int length = 10, string searchValue = "", string sortColumn = "courseName", string sortDirection = "ASC")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (courses, count) = await dataTransactionManager.CourseDataManager.RetrieveDataWithCount("SearchStudentCourses", new SqlParameter[] {
                new SqlParameter("@studentId", studentId),
                new SqlParameter("@start", start),
                new SqlParameter("@length", length),
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection)
            });
            return (courses, count);
        }

        public async Task<(IEnumerable<StudentRegistration>, long)> SearchMyRegistrations(string email, int start = 0, int length = 10)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (registrations, count) = await dataTransactionManager.StudentRegistrationDataManager.RetrieveDataWithCount("SearchMyRegistrations", new SqlParameter[] {
                new SqlParameter("@email", email),
                new SqlParameter("@start", start),
                new SqlParameter("@length", length)
            });
            return (registrations, count);
        }

        public async Task<(IEnumerable<StudentBatchPayment>, long)> SearchStudentBatchPayments(long studentId, long studentBatchId = 0, int start = 0, int length = 10)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (payments, count) = await dataTransactionManager.StudentBatchPaymentDataManager.RetrieveDataWithCount("SearchStudentBatchPayments", new SqlParameter[] {
                new SqlParameter("@studentId", studentId),
                new SqlParameter("@studentBatchId", studentBatchId),
                new SqlParameter("@start", start),
                new SqlParameter("@length", length)
            });
            return (payments, count);
        }

        public async Task<StudentBatchPayment> GetStudentBatchPayment(long paymentId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var payments = await dataTransactionManager.StudentBatchPaymentDataManager.RetrieveData("GetStudentBatchPayment", new SqlParameter[] {
                new SqlParameter("@paymentId", paymentId)
            });
            return payments.FirstOrDefault();
        }

        public async Task<(bool, long)> UpdateStudentBatchPayment(StudentBatchPayment payment)
        {
            string jsonString = JsonConvert.SerializeObject(payment);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            return await dataTransactionManager.StudentBatchPaymentDataManager.UpdateDataReturnPrimaryKey("UpdateStudentBatchPayment", jsonString);
        }

        public async Task<bool> DeleteStudentBatchPayment(long paymentId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            return await dataTransactionManager.StudentBatchPaymentDataManager.ExecuteNonQuery("DeleteStudentBatchPayment", new SqlParameter[] {
                new SqlParameter("@paymentId", paymentId)
            });
        }

        public async Task<IEnumerable<dynamic>> GetStudentBatchesList(long studentId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            return await dataTransactionManager.StudentBatchPaymentDataManager.RetrieveDynamicData("GetStudentBatchesList", new SqlParameter[] {
                new SqlParameter("@studentId", studentId)
            });
        }

        public async Task<IEnumerable<dynamic>> GetStudentPaymentSummary(long studentId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            return await dataTransactionManager.StudentBatchPaymentDataManager.RetrieveDynamicData("GetStudentPaymentSummary", new SqlParameter[] {
                new SqlParameter("@studentId", studentId)
            });
        }
    }
}
