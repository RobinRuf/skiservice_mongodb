using AutoMapper;
using skiservice.Dto;
using skiservice.Dtos;
using skiservice.Models;

namespace skiservice.Mappings
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            // Automate the mapping of DTOs to models and vice versa
            CreateMap<ServiceOrderModel, CreateServiceOrderDto>();
            CreateMap<CreateServiceOrderDto, ServiceOrderModel>();

            CreateMap<ServiceOrderModel, ServiceOrderDto>();
            CreateMap<ServiceOrderDto, ServiceOrderModel>();

            CreateMap<ServiceOrderModel, UpdateServiceOrderDto>();
            CreateMap<UpdateServiceOrderDto, ServiceOrderModel>();

            CreateMap<UserModel, UserDto>();
            CreateMap<UserDto, UserModel>();

            CreateMap<UserModel, CreateUserDto>();
            CreateMap<CreateUserDto, UserModel>();

            CreateMap<UserModel, LoginUserDto>();
            CreateMap<LoginUserDto, UserModel>();

            // Mappings für Priority, Service und Status
        CreateMap<PriorityModel, PriorityDto>();
        CreateMap<ServiceModel, ServiceDto>();
        CreateMap<StatusModel, StatusDto>();

        // Erweitertes Mapping für ServiceOrders
        CreateMap<ServiceOrderModel, ServiceOrderDto>()
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
            .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.Service))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<ServiceOrderDto, ServiceOrderModel>()
            .ForMember(dest => dest.Priority, opt => opt.Ignore())
            .ForMember(dest => dest.Service, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore());


            CreateMap<UpdateServiceOrderDto, ServiceOrderModel>()
            .ForMember(dest => dest.Firstname, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Firstname) && src.Firstname != "string"))
            .ForMember(dest => dest.Lastname, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Lastname) && src.Lastname != "string"))
            .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Email) && src.Email != "user@example.com"))
            .ForMember(dest => dest.Phone, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Phone) && src.Phone != "+048-9540099185"))
            .ForMember(dest => dest.PriorityId, opt => opt.Condition(src => !string.IsNullOrEmpty(src.PriorityId) && src.PriorityId != "string"))
            .ForMember(dest => dest.CreateDate, opt => opt.Condition(src => src.CreateDate != default))
            .ForMember(dest => dest.PickupDate, opt => opt.Condition(src => src.PickupDate != default))
            .ForMember(dest => dest.ServiceId, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ServiceId) && src.ServiceId != "string"))
            .ForMember(dest => dest.TotalPrice_CHF, opt => opt.Condition(src => src.Price != 0))
            .ForMember(dest => dest.StatusId, opt => opt.Condition(src => !string.IsNullOrEmpty(src.StatusId) && src.StatusId != "string"))
            .ForMember(dest => dest.Comment, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Comment)));

        }
    }
}
