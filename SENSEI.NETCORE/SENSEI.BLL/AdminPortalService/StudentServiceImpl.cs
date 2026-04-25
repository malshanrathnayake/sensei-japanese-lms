using devspark_core_data_access_layer;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SENSEI.BLL.AdminPortalService
{
    public class StudentServiceImpl : IStudentService
    {
        private readonly IDatabaseService _databaseService;

        public StudentServiceImpl(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<(IEnumerable<Student>, long)> SearchStudent(long courseId = 0, long batchId = 0, int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (registrations, count) = await dataTransactionManager.StudentDataManager.RetrieveDataWithCount("SearchStudent", new SqlParameter[] {
                new SqlParameter("@courseId", courseId),
                new SqlParameter("@batchId", batchId),
                new SqlParameter("@start", start),
                new SqlParameter("@length", length),
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection)
            });
            return (registrations, count);
        }

        public async Task<Student> GetStudent(long studentId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var student = await dataTransactionManager.StudentDataManager.RetrieveData("GetStudentById", new SqlParameter[] {
                new SqlParameter("@studentId", studentId)
            });
            return student.FirstOrDefault();
        }

        public async Task<bool> UpdateStudent(Student student)
        {
            string jsonString = JsonConvert.SerializeObject(student);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.StudentDataManager.UpdateDataReturnPrimaryKey("UpdateStudent", jsonString);
            return status;
        }
    }
}
