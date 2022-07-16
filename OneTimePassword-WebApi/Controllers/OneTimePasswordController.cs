using Microsoft.AspNetCore.Mvc;
using OneTimePasswordBusinessLogic;
using OneTimePasswordBusinessLogic.Models;

namespace OneTimePassword_WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OneTimePasswordController : ControllerBase
    {
        private readonly OneTimePasswordApplication oneTimePasswordApplication;
        public OneTimePasswordController()
        {
            var implementation = new AllInOneImplementation();
            oneTimePasswordApplication = new OneTimePasswordApplication(implementation, implementation, implementation);
        }

        [HttpGet("GetValidPassword")]
        public async Task<OneTimePasswordWithExpirationInSeconds?> GetValidPasswordForUser(string userId)
        {
            return await oneTimePasswordApplication.GetPasswordWithExpirationForUser(userId);
        }

        [HttpPost("Generate")]
        public async Task GenerateOneTimePasswordForUser([FromBody] string userId)
        {
            await oneTimePasswordApplication.CreateOneTimePasswordForUser(userId);
        }

        [HttpPost("Validate")]
        public async Task Validate([FromBody] OneTimePassword oneTimePassword)
        {
            var isValid = await oneTimePasswordApplication.IsOneTimePasswordValidForUser(oneTimePassword.UserId, oneTimePassword.Value);
        }
    }
}
