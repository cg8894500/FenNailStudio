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

            CreateMap<RegisterDto, Customer>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.RegisterDate, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.RegisterDate, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            CreateMap<UpdateCustomerDto, Customer>()
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.RegisterDate, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            // NailService 映射
            CreateMap<NailService, NailServiceDto>();

            CreateMap<CreateNailServiceDto, NailService>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            CreateMap<UpdateNailServiceDto, NailService>()
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            // Technician 映射
            CreateMap<Technician, TechnicianDto>();

            CreateMap<CreateTechnicianDto, Technician>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

            CreateMap<UpdateTechnicianDto, Technician>()
                .ForMember(dest => dest.Appointments, opt => opt.Ignore());

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
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Domain.Enums.AppointmentStatus.Scheduled))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Technician, opt => opt.Ignore())
                .ForMember(dest => dest.Service, opt => opt.Ignore());

            CreateMap<UpdateAppointmentDto, Appointment>()
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Technician, opt => opt.Ignore())
                .ForMember(dest => dest.Service, opt => opt.Ignore());
        }
    }
}
