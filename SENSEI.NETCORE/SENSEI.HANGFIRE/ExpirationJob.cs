using SENSEI.BLL.ApiPortalservices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.HANGFIRE
{
    public class ExpirationJob
    {
        private readonly IExpirationService _expirationService;

        public ExpirationJob(IExpirationService expirationService)
        {
            _expirationService = expirationService;
        }

        public async Task Execute()
        {
            await _expirationService.UpdateLessonOnExpirationAsync();
        }
    }
}
