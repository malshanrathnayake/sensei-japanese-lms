using devspark_core_data_access_layer;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService
{
    public class CourseServiceImpl : ICourseService
    {
        private readonly IDatabaseService _databaseService;

        public CourseServiceImpl(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<(bool, long)> UpdateCourse(Course course)
        {
            string userJsonString = JsonConvert.SerializeObject(course);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = dataTransactionManager.CourseDataManager.UpdateDataReturnPrimaryKey("UpdateCourse", userJsonString);
            return (status, primaryKey);
        }

        public async Task<Course> GetCourse(long courseId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var course = dataTransactionManager.CourseDataManager.RetrieveData("GetCourse", [
                new SqlParameter("@CourseId", courseId)
                ]).FirstOrDefault();
            return course;
        }

        public async Task<(IEnumerable<Course>, long)> GetCourses(int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (courses, count) = dataTransactionManager.CourseDataManager.RetrieveDataWithCount("GetCourses", [
                new SqlParameter("@start", start),
                new SqlParameter("@length", length),
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection)
            ]);
            return (courses, count);
        }

        public async Task<bool> DeleteCourse(long courseId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var status = dataTransactionManager.CourseDataManager.DeleteData("DeleteCourse", [
                new SqlParameter("@courseId", courseId),
            ]);
            return status;
        }
    }
}
