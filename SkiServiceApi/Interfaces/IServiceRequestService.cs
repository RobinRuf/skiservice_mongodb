using skiservice.Dto;
using skiservice.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace skiservice.Interfaces
{
    /// <summary>
    /// Interface for the service order service
    /// </summary>
    public interface IServiceOrderService
    {
        Task<ServiceOrderDto> CreateServiceOrderAsync(CreateServiceOrderDto dto);
        Task<IEnumerable<ServiceOrderDto>> GetAllServiceOrdersAsync();
        Task<List<ServiceOrderDto>> GetAllServiceOrdersByPriorty(string priority);
        Task<ServiceOrderDto> GetServiceOrderByIdAsync(string id);
        Task<ServiceOrderDto> UpdateServiceOrderAsync(string id, UpdateServiceOrderDto dto);
        Task CancelServiceOrderAsync(string serviceOrderId);
        Task DeleteServiceOrderPermanentlyAsync(string id);
    }
}