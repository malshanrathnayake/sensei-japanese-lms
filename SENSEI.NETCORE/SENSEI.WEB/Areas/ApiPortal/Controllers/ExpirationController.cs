using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.ApiPortalservices.Interfaces;

namespace SENSEI.WEB.Areas.ApiPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpirationController : ControllerBase
    {
        private readonly IExpirationService _expirationService;

        public ExpirationController(IExpirationService expirationService)
        {
            _expirationService = expirationService;
        }

        //https://www.senseijapanesecenter.com/api/Expiration/UpdateLessonOnExpiration
        [HttpGet("UpdateLessonOnExpiration")]
        public async Task<JsonResult> UpdateLessonOnExpiration()
        {
            var status = await _expirationService.UpdateLessonOnExpirationAsync();
            return new JsonResult(new { success = status, message = status ? "Lesson expiration updated successfully." : "Failed to update lesson expiration." });
        }
    }
}
