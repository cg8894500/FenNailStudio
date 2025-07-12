using FenNailStudio.Application.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FenNailStudio.Web.Models.ViewModels
{
    public class AppointmentListViewModel
    {
        public IEnumerable<AppointmentDto> Appointments { get; set; }
        public string Title { get; set; }
    }

    public class CreateAppointmentViewModel
    {
        public CreateAppointmentDto AppointmentDto { get; set; }
        public IEnumerable<CustomerDto> Customers { get; set; }
        public IEnumerable<TechnicianDto> Technicians { get; set; }
        public IEnumerable<NailServiceDto> Services { get; set; }
    }

    public class EditAppointmentViewModel
    {
        public UpdateAppointmentDto AppointmentDto { get; set; }
        public IEnumerable<TechnicianDto> Technicians { get; set; }
        public IEnumerable<NailServiceDto> Services { get; set; }
        public string CustomerName { get; set; }
    }

    public class AppointmentDetailsViewModel
    {
        public AppointmentDto Appointment { get; set; }
    }
}
