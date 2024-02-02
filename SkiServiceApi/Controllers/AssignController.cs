using skiservice.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using skiservice.Services;

namespace skiservice.Controllers
{
    public class AssignController : Controller
    {
        private readonly IAssignService _assignService;
        private readonly IServiceOrderService _serviceOrderService;

        public AssignController(IAssignService assignService, IServiceOrderService serviceOrderService)
        {
            _assignService = assignService;
            _serviceOrderService = serviceOrderService;
        }

        [HttpPost("{Id}/assign/{userId}")]
        [Authorize(Roles = "ADMIN, USER")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignServiceOrderToUser(string Id, string userId)
        {
            try
            {
                await _assignService.AssignServiceOrderToUser(Id, userId);
                var serviceOrderDto = await _serviceOrderService.GetServiceOrderByIdAsync(Id);
                return Ok(serviceOrderDto);
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
                var serviceOrderDto = await _serviceOrderService.GetServiceOrderByIdAsync(Id);
                return Ok(serviceOrderDto);
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
