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
    public class LessonServiceImpl: ILessonService
    {
        private readonly IDatabaseService _databaseService;

        public LessonServiceImpl(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<(bool, long)> UpdateLesson(Lesson lesson)
        {
            string userJsonString = JsonConvert.SerializeObject(lesson);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = dataTransactionManager.LessonDataManager.UpdateDataReturnPrimaryKey("UpdateLesson", userJsonString);
            return (status, primaryKey);
        }

        public async Task<Lesson> GetLesson(long lessonId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var lesson = dataTransactionManager.LessonDataManager.RetrieveData("GetLesson", [
                new SqlParameter("@lessonId", lessonId)
                ]).FirstOrDefault();
            return lesson;
        }

        public async Task<(IEnumerable<Lesson>, long)> SearchLessons(long courseId = 0, int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "")
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (lessons, count) = dataTransactionManager.LessonDataManager.RetrieveDataWithCount("SearchLessons", [
                new SqlParameter("@courseId", courseId),
                new SqlParameter("@start", start),
                new SqlParameter("@length", length),
                new SqlParameter("@searchValue", searchValue),
                new SqlParameter("@sortColumn", sortColumn),
                new SqlParameter("@sortDirection", sortDirection)
            ]);
            return (lessons, count);
        }

        public async Task<IEnumerable<Lesson>> GetLessons(int courseId = 0)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var lessons = dataTransactionManager.LessonDataManager.RetrieveData("GetLesson", [
                new SqlParameter("@courseId", courseId),
            ]);
            return lessons;
        }

        public async Task<bool> DeleteLesson(long lessonId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var status = dataTransactionManager.LessonDataManager.DeleteData("DeleteLesson", [
                new SqlParameter("@lessonId", lessonId),
            ]);
            return status;
        }
    }
}
