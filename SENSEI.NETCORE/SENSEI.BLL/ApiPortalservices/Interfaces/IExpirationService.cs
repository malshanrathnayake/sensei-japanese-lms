using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.ApiPortalservices.Interfaces
{
    public interface IExpirationService
    {
        Task<bool> UpdateLessonOnExpirationAsync();
    }
}
