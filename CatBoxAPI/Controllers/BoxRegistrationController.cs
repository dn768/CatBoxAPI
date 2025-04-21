using CatBoxAPI.Models.BoxRegistration;
using CatBoxAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatBoxAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BoxRegistrationController(IBoxRegistrationService boxRegistrationService, IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(BoxRegistrationCreationDTO registration)
        {
            if (registration.CatId == Guid.Empty)
                return BadRequest("Cat Id is required.");

            if (string.IsNullOrEmpty(registration.BoxType?.Trim()))
                return BadRequest("Box type is required.");

            try
            {
                var id = await boxRegistrationService.CreateAsync(registration);

                return Ok(id.ToString());
            }
            catch (Exception ex)
            {
                if (ex is UserFriendlyException)
                    return BadRequest(ex.Message);

                var errorMessage = webHostEnvironment.IsDevelopment() ? $"\r\n{ex.Message}\r\n{ex.InnerException}" : "";
                return Problem($"Cat box registration request encountered an error{errorMessage}");
            }
        }
    }
}
