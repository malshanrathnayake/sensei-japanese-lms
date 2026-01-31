using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService.Interface
{
    public interface ICourseService
    {
        Task<(bool, long)> UpdateCourse(Course course);
        Task<Course> GetCourse(long courseId);
        Task<(IEnumerable<Course>, long)> SearchCourses(int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "");
        Task<bool> DeleteCourse(long courseId);
        Task<IEnumerable<Course>> GetCourses();
    }
}
