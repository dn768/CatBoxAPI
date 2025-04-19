using CatBoxAPI.Models.BoxRegistration;
using CatBoxAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatBoxAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BoxRegistrationController(IBoxRegistrationService boxRegistrationService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(BoxRegistrationCreationDTO registration)
        {
            var id = await boxRegistrationService.CreateAsync(registration);

            return Ok(id.ToString());
        }
    }
}
