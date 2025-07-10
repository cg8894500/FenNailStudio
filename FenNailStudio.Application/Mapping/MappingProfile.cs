using AutoMapper;
using FenNailStudio.Application.DTOs;
using FenNailStudio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Customer 映射
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<UpdateCustomerDto, Customer>();

            // NailService 映射
            CreateMap<NailService, NailServiceDto>();
            CreateMap<CreateNailServiceDto, NailService>();
            CreateMap<UpdateNailServiceDto, NailService>();

            // Technician 映射
            CreateMap<Technician, TechnicianDto>();
            CreateMap<CreateTechnicianDto, Technician>();
            CreateMap<UpdateTechnicianDto, Technician>();

            // Appointment 映射
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.TechnicianName, opt => opt.MapFrom(src => src.Technician.Name))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
                .ForMember(dest => dest.ServicePrice, opt => opt.MapFrom(src => src.Service.Price))
                .ForMember(dest => dest.ServiceDuration, opt => opt.MapFrom(src => src.Service.DurationMinutes))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src =>
                    src.AppointmentDateTime.AddMinutes(src.Service.DurationMinutes)));

            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Domain.Enums.AppointmentStatus.Scheduled));

            CreateMap<UpdateAppointmentDto, Appointment>();
        }
    }
}
