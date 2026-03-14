using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService.Interface
{
    public interface IStudentService
    {
        Task<(IEnumerable<Student>, long)> SearchStudent(long courseId = 0, long batchId =0, int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "");
    }
}
