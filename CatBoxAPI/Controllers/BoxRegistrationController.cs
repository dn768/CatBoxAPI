using CatBoxAPI.Models.BoxRegistration;
using CatBoxAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        public async Task<ActionResult<IEnumerable<BoxRegistrationListItemDTO>>> GetBoxRegistrationList([FromQuery] string? filter)
        {
            // Updated filter to be consistent with CatProfileController, since I'm letting the endpoint consumer use these 4 filter states:
            // 1. No filter (all BoxResistrations returned)
            // 2. Null filter (undecided BoxResistrations returned)
            // 3. True filter (approved BoxResistrations returned)
            // 4. False filter (rejected BoxResistrations returned)

            // TODO: Consider using two seperate filters, in case the consumer wants to get all decided BoxResistration requests,
            // whether approved or rejected

            bool filterByApproval = false;
            bool? approvalFilter = null;

            if (!string.IsNullOrEmpty(filter?.Trim()))
            {
                filterByApproval = true;
                if (!filter.Trim().StartsWith("approved", StringComparison.CurrentCultureIgnoreCase))
                    return BadRequest($"Filter feature is provided only for approved state.");

                if (filter.Trim().EndsWith("null", StringComparison.CurrentCultureIgnoreCase))
                    approvalFilter = null;
                else if (filter.Trim().EndsWith("false", StringComparison.CurrentCultureIgnoreCase))
                    approvalFilter = false;
                // Defaulting to true since the filter name is "approved", enabling the consumer to use /?filter=approved directly and intuitively
                else
                    approvalFilter = true;
            }

            try
            {
                return Ok(await boxRegistrationService.GetBoxRegistrationListAsync(filterByApproval, approvalFilter));
            }
            catch (Exception ex)
            {
                var errorMessage = webHostEnvironment.IsDevelopment() ? $"\r\n{ex.Message}\r\n{ex.InnerException}" : "";
                return Problem($"Cat box registration request encountered an error{errorMessage}");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> ApplyDecision(
            [FromQuery]
            [Required]
            Guid boxRegistrationId,
            
            [FromQuery]
            [Required]
            bool isApproved,
            
            [FromQuery]
            string? decisionReason)
        {
            try
            {
                await boxRegistrationService.SaveRegistrationApproval(boxRegistrationId, isApproved, decisionReason);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is UserFriendlyException)
                    return NotFound(ex.Message);

                var errorMessage = webHostEnvironment.IsDevelopment() ? $"\r\n{ex.Message}\r\n{ex.InnerException}" : "";
                return Problem($"Applying approval decision to the Cat Box registration request encountered an error{errorMessage}");
            }
        }
    }
}
