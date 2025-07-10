using FenNailStudio.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentDto>> GetAllAsync();
        Task<IEnumerable<AppointmentDto>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<AppointmentDto>> GetByTechnicianIdAsync(int technicianId);
        Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task<bool> IsTimeSlotAvailableAsync(int technicianId, DateTime start, DateTime end, int? excludeAppointmentId = null);
        Task<int> CreateAsync(CreateAppointmentDto appointmentDto);
        Task UpdateAsync(UpdateAppointmentDto appointmentDto);
        Task CancelAsync(int id);
        Task DeleteAsync(int id);
    }
}
