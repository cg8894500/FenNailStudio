using AutoMapper;
using FenNailStudio.Application.DTOs;
using FenNailStudio.Application.Interfaces;
using FenNailStudio.Domain.Entities;
using FenNailStudio.Domain.Enums;
using FenNailStudio.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly INailServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            INailServiceRepository serviceRepository,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> GetByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByCustomerIdAsync(int customerId)
        {
            var appointments = await _appointmentRepository.GetByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByTechnicianIdAsync(int technicianId)
        {
            var appointments = await _appointmentRepository.GetByTechnicianIdAsync(technicianId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            var appointments = await _appointmentRepository.GetByDateRangeAsync(start, end);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<bool> IsTimeSlotAvailableAsync(int technicianId, DateTime start, DateTime end, int? excludeAppointmentId = null)
        {
            var appointments = await _appointmentRepository.GetByTechnicianIdAsync(technicianId);

            // 排除指定的預約（用於更新預約時檢查）
            if (excludeAppointmentId.HasValue)
            {
                appointments = appointments.Where(a => a.Id != excludeAppointmentId.Value);
            }

            // 只考慮已預約狀態的預約
            appointments = appointments.Where(a => a.Status == AppointmentStatus.Scheduled);

            foreach (var appointment in appointments)
            {
                var appointmentEnd = appointment.AppointmentDateTime.AddMinutes(appointment.Service.DurationMinutes);

                // 檢查是否有時間重疊
                if (start < appointmentEnd && end > appointment.AppointmentDateTime)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<int> CreateAsync(CreateAppointmentDto appointmentDto)
        {
            // 取得服務項目以計算結束時間
            var service = await _serviceRepository.GetByIdAsync(appointmentDto.ServiceId);
            var endTime = appointmentDto.AppointmentDateTime.AddMinutes(service.DurationMinutes);

            // 檢查時間是否可用
            if (!await IsTimeSlotAvailableAsync(appointmentDto.TechnicianId, appointmentDto.AppointmentDateTime, endTime))
            {
                throw new InvalidOperationException("所選時間已被預約，請選擇其他時間。");
            }

            var appointment = _mapper.Map<Appointment>(appointmentDto);
            await _appointmentRepository.AddAsync(appointment);
            return appointment.Id;
        }

        public async Task UpdateAsync(UpdateAppointmentDto appointmentDto)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentDto.Id);

            // 取得服務項目以計算結束時間
            var service = await _serviceRepository.GetByIdAsync(appointmentDto.ServiceId);
            var endTime = appointmentDto.AppointmentDateTime.AddMinutes(service.DurationMinutes);

            // 檢查時間是否可用（排除當前預約）
            if (!await IsTimeSlotAvailableAsync(appointmentDto.TechnicianId, appointmentDto.AppointmentDateTime, endTime, appointmentDto.Id))
            {
                throw new InvalidOperationException("所選時間已被預約，請選擇其他時間。");
            }

            _mapper.Map(appointmentDto, appointment);
            await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task CancelAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            appointment.Status = AppointmentStatus.Cancelled;
            await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task DeleteAsync(int id)
        {
            await _appointmentRepository.DeleteAsync(id);
        }
    }
}
