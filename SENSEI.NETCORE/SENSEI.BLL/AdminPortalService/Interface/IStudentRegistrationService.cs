using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService.Interface
{
    public interface IStudentRegistrationService
    {
        Task<(bool, long)> UpdateStudentRegistraion(StudentRegistration studentRegistration);
        Task<(IEnumerable<StudentRegistration>, long)> SearchStudentRegistraion(int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "");
        Task<StudentRegistration> GetStudentRegistraion(long studentRegistrationId);
        Task<bool> ApproveStudentRegistraion(long studentRegistrationId, string indexNumber, long batchId, long approvedById);
    }
}
