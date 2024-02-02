using skiservice.Dto;
using skiservice.Dtos;
using skiservice.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace skiservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  
    public class ServiceOrdersController : ControllerBase
    {
        private readonly IServiceOrderService _serviceOrderService;

        public ServiceOrdersController(IServiceOrderService serviceOrderService)
        {
            _serviceOrderService = serviceOrderService;
        }
        /// <summary>
        /// Retrieves all service requests matching a specific priority level.
        /// </summary>
        /// <param name="sort">The priority level to filter service requests.</param>
        /// <returns>A list of serviceOrderDto objects.</returns>
        [HttpGet]        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof (List<ServiceOrderDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllServiceOrders()
        {
            var serviceOrderDtos = await _serviceOrderService.GetAllServiceOrdersAsync();
            return Ok(serviceOrderDtos);
        }

        /// <summary>
        /// Retrieves a specific service request by its ID
        /// </summary>
        /// <param name="id">The ID of the service request</param>
        /// <returns>A serviceOrderDto object if found; otherwise, a 404 Not Found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof (ServiceOrderDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetServiceOrderById(string id)
        {
            var serviceOrderDto = await _serviceOrderService.GetServiceOrderByIdAsync(id);
            if (serviceOrderDto == null)
            {
                return NotFound();
            }
            return Ok(serviceOrderDto);
        }
        /// <summary>
        /// Retrieves all service requests matching a specific priority level
        /// </summary>
        /// <param name="priority">The priority level to filter service requests</param>
        /// <returns>A list of serviceOrderDto objects</returns>
        [HttpGet("priorities/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceOrderDto))]
        public async Task<IActionResult> GetAllServiceOrdersByPriorty(string id)
        {
            var serviceOrderDtos = await _serviceOrderService.GetAllServiceOrdersByPriorty(id);
            return Ok(serviceOrderDtos);
        }

        /// <summary>
        /// Creates a new service request from the provided DTO
        /// </summary>
        /// <param name="createServiceOrderDto">The DTO containing service request data</param>
        /// <returns>A newly created serviceOrderDto object</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateServiceOrderDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> CreateServiceOrder([FromBody] CreateServiceOrderDto createServiceOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceOrderDto = await _serviceOrderService.CreateServiceOrderAsync(createServiceOrderDto);
            return CreatedAtAction(nameof(GetServiceOrderById), new { id = serviceOrderDto.Id }, serviceOrderDto);
        }

        /// <summary>
        /// Updates an existing service request identified by its ID with the provided DTO data.
        /// </summary>
        /// <param name="id">The ID of the service request to update.</param>
        /// <param name="updateServiceOrderDTO">The DTO containing updated data for the service request.</param>
        /// <returns>The updated serviceOrderDto object if found; otherwise, a 404 Not Found.</returns>       
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateServiceOrderDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<IActionResult> UpdateServiceOrder(string id, [FromBody] UpdateServiceOrderDto updateServiceOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
  

            var updatedServiceOrder = await _serviceOrderService.UpdateServiceOrderAsync(id, updateServiceOrderDto);
            if (updatedServiceOrder == null)
            {
                return NotFound();
            }
            return Ok(updatedServiceOrder);
        }

        /// <summary>
        /// Setzt den Status eines ServiceOrder auf "Storniert" basierend auf der angegebenen ID.
        /// </summary>
        /// <param name="id">Die ID des zu stornierenden ServiceOrder.</param>
        /// <returns>Eine 200 OK-Antwort bei Erfolg; andernfalls eine 404 Nicht Gefunden.</returns>
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "ADMIN,USER")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceOrderDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelServiceOrder(string id)
        {
            var serviceOrderDto = await _serviceOrderService.GetServiceOrderByIdAsync(id);
            if (serviceOrderDto == null)
            {
                return NotFound();
            }

            await _serviceOrderService.CancelServiceOrderAsync(id);
            return Ok();
        }

        /// <summary>
        /// Löscht einen ServiceOrder dauerhaft basierend auf der angegebenen ID.
        /// </summary>
        /// <param name="id">Die ID des zu löschenden ServiceOrder.</param>
        /// <returns>Eine 200 OK-Antwort bei Erfolg; andernfalls eine 404 Nicht Gefunden.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceOrderDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteServiceOrderPermanently(string id)
        {
            var serviceOrderDto = await _serviceOrderService.GetServiceOrderByIdAsync(id);
            if (serviceOrderDto == null)
            {
                return NotFound();
            }

            await _serviceOrderService.DeleteServiceOrderPermanentlyAsync(id);
            return Ok();
        }


    }
}
