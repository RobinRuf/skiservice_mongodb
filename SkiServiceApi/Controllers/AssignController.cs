using skiservice.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace skiservice.Controllers
{
    public class AssignController : Controller
    {
        private readonly IAssignService _assignService;

        public AssignController(IAssignService assignService)
        {
            _assignService = assignService;
        }

        [HttpPost("{Id}/assign/{userId}")]
        [Authorize(Roles = "ADMIN, USER")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignServiceOrderToUser(string Id, string userId)
        {
            try
            {
                string currentUserName = User.Identity?.Name;
                await _assignService.AssignServiceOrderToUser(Id, userId, currentUserName);
                return Ok($"Auftrag {Id} wurde erfolgreich dem User {userId} zugewiesen.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{Id}/assign/{userId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ReAssignServiceOrderToUser(string Id, string userId)
        {
            try
            {
                await _assignService.AssignServiceOrderToUser(Id, userId);
                return Ok($"ServiceOrder {Id} wurde erfolgreich User {userId} zugewiesen.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
