using skiservice.Data;
using skiservice.Dto;
using skiservice.Dtos;
using skiservice.Interfaces;
using skiservice.Models;
using MongoDB.Driver;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace skiservice.Services
{
    /// <summary>
    /// Service for managing service requests using MongoDB.
    /// </summary>
    public class ServiceOrderService : IServiceOrderService
    {
        private readonly IMongoCollection<ServiceOrderModel> _serviceOrders;
        private readonly IMongoCollection<ServiceModel> _services;
        private readonly IMongoCollection<PriorityModel> _priorities;
        private readonly IMongoCollection<StatusModel> _statuses;
        private readonly IMapper _mapper;

        public ServiceOrderService(IMongoDatabase database, IMapper mapper)
        {
            _serviceOrders = database.GetCollection<ServiceOrderModel>("ServiceOrders");
            _services = database.GetCollection<ServiceModel>("Services");
            _priorities = database.GetCollection<PriorityModel>("Priorities");
            _statuses = database.GetCollection<StatusModel>("Statuses");
            _mapper = mapper;
        }

        public async Task<ServiceOrderDto> GetServiceOrderByIdAsync(string id)
        {
            var filter = Builders<ServiceOrderModel>.Filter.Eq(sr => sr.Id, id);
            var serviceOrder = await _serviceOrders.Find(filter).FirstOrDefaultAsync();

            if (serviceOrder == null)
            {
                return null;
            }

            // Laden der verknüpften Priority, Service und Status Daten
            if (!string.IsNullOrEmpty(serviceOrder.PriorityId))
            {
                serviceOrder.Priority = await _priorities.Find(p => p.Id == serviceOrder.PriorityId).FirstOrDefaultAsync();
            }

            if (!string.IsNullOrEmpty(serviceOrder.ServiceId))
            {
                serviceOrder.Service = await _services.Find(s => s.Id == serviceOrder.ServiceId).FirstOrDefaultAsync();
            }

            if (!string.IsNullOrEmpty(serviceOrder.StatusId))
            {
                serviceOrder.Status = await _statuses.Find(st => st.Id == serviceOrder.StatusId).FirstOrDefaultAsync();
            }

            return _mapper.Map<ServiceOrderDto>(serviceOrder);
        }


        public async Task<ServiceOrderDto> CreateServiceOrderAsync(CreateServiceOrderDto createDto)
        {
            var serviceOrder = _mapper.Map<ServiceOrderModel>(createDto);

            var service = await _services.Find(s => s.Id == serviceOrder.ServiceId).FirstOrDefaultAsync();
            var priority = await _priorities.Find(p => p.Id == serviceOrder.PriorityId).FirstOrDefaultAsync();

            serviceOrder.TotalPrice_CHF = service.Price + priority.Price;

            var openStatus = await _statuses.Find(st => st.StatusName == "Offen").FirstOrDefaultAsync();
            serviceOrder.StatusId = openStatus?.Id ?? string.Empty;

            await _serviceOrders.InsertOneAsync(serviceOrder);
            return _mapper.Map<ServiceOrderDto>(serviceOrder);
        }

        public async Task<ServiceOrderDto> UpdateServiceOrderAsync(string id, UpdateServiceOrderDto updateDto)
        {
            var filter = Builders<ServiceOrderModel>.Filter.Eq(sr => sr.Id, id);
            var serviceOrder = await _serviceOrders.Find(filter).FirstOrDefaultAsync();

            if (serviceOrder == null)
            {
                return null; // Oder entsprechende Fehlerbehandlung
            }

            // Aktualisieren des ServiceOrder-Objekts mit den Daten aus dem DTO
            _mapper.Map(updateDto, serviceOrder);

            // Speichern der Änderungen in der Datenbank
            await _serviceOrders.ReplaceOneAsync(filter, serviceOrder);

            // Laden der verknüpften Priority, Service und Status Daten
            serviceOrder.Priority = await _priorities.Find(p => p.Id == serviceOrder.PriorityId).FirstOrDefaultAsync();
            serviceOrder.Service = await _services.Find(s => s.Id == serviceOrder.ServiceId).FirstOrDefaultAsync();
            serviceOrder.Status = await _statuses.Find(st => st.Id == serviceOrder.StatusId).FirstOrDefaultAsync();

            // Rückgabe des aktualisierten ServiceOrder-DTOs
            return _mapper.Map<ServiceOrderDto>(serviceOrder);
        }



        public async Task CancelServiceOrderAsync(string serviceOrderId)
        {
            // Abrufen der ObjectId für den Status "Storniert"
            var cancelledStatus = await _statuses.Find(status => status.StatusName == "Storniert").FirstOrDefaultAsync();
            if (cancelledStatus == null)
            {
                throw new Exception("Der Status 'Storniert' wurde nicht in der Datenbank gefunden.");
            }

            // Aktualisieren des StatusId-Felds des ServiceOrders
            var filter = Builders<ServiceOrderModel>.Filter.Eq(sr => sr.Id, serviceOrderId);
            var update = Builders<ServiceOrderModel>.Update.Set(sr => sr.StatusId, cancelledStatus.Id);
            await _serviceOrders.UpdateOneAsync(filter, update);
        }

        public async Task DeleteServiceOrderPermanentlyAsync(string id)
        {
            var filter = Builders<ServiceOrderModel>.Filter.Eq(sr => sr.Id, id);
            await _serviceOrders.DeleteOneAsync(filter);
        }




        public async Task<List<ServiceOrderDto>> GetAllServiceOrdersByPriorty(string priority)
        {
            var filter = Builders<ServiceOrderModel>.Filter.Eq(sr => sr.PriorityId, priority);
            var serviceOrders = await _serviceOrders.Find(filter).ToListAsync();

            return _mapper.Map<List<ServiceOrderDto>>(serviceOrders);
        }

        public async Task<List<ServiceOrderDto>> GetAllServiceOrdersByStatus(string status)
        {
            var filter = Builders<ServiceOrderModel>.Filter.Eq(sr => sr.StatusId, status);
            var serviceOrders = await _serviceOrders.Find(filter).ToListAsync();

            return _mapper.Map<List<ServiceOrderDto>>(serviceOrders);
        }

        public async Task<IEnumerable<ServiceOrderDto>> GetAllServiceOrdersAsync()
        {
            var serviceOrdersList = await _serviceOrders.Find(_ => true).ToListAsync();

            foreach (var serviceOrder in serviceOrdersList)
            {
                if (!string.IsNullOrEmpty(serviceOrder.PriorityId))
                {
                    serviceOrder.Priority = await _priorities.Find(p => p.Id == serviceOrder.PriorityId).FirstOrDefaultAsync();
                }

                if (!string.IsNullOrEmpty(serviceOrder.ServiceId))
                {
                    serviceOrder.Service = await _services.Find(s => s.Id == serviceOrder.ServiceId).FirstOrDefaultAsync();
                }

                if (!string.IsNullOrEmpty(serviceOrder.StatusId))
                {
                    serviceOrder.Status = await _statuses.Find(s => s.Id == serviceOrder.StatusId).FirstOrDefaultAsync();
                }
            }

            return _mapper.Map<IEnumerable<ServiceOrderDto>>(serviceOrdersList);
        }

    }
}
