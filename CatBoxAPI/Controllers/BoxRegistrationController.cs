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

        [HttpPatch]
        public async Task<IActionResult> Update(BoxRegistrationEditDTO registration)
        {
            if (registration.Id == Guid.Empty)
                return BadRequest("Cat Box Registration Id is required.");

            if (string.IsNullOrEmpty(registration.BoxType?.Trim()))
                return BadRequest("Box type is required.");
            
            try
            {
                await boxRegistrationService.UpdateAsync(registration);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is UserFriendlyException)
                    return NotFound(ex.Message);

                var errorMessage = webHostEnvironment.IsDevelopment() ? $"\r\n{ex.Message}\r\n{ex.InnerException}" : "";
                return Problem($"Cat box registration request encountered an error{errorMessage}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoxRegistrationListItemDTO>>> GetBoxRegistrationList([FromQuery] bool? isApproved)
        {
            // Using simpler filtering for this endpoint. Making the assumption for now that additional fields won't be useful to filter by
            try
            {
                return Ok(await boxRegistrationService.GetBoxRegistrationListAsync(isApproved));
            }
            catch (Exception ex)
            {
                var errorMessage = webHostEnvironment.IsDevelopment() ? $"\r\n{ex.Message}\r\n{ex.InnerException}" : "";
                return Problem($"Cat box registration request encountered an error{errorMessage}");
            }
        }
    }
}
