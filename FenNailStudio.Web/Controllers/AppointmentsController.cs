using Microsoft.AspNetCore.Mvc;
using FenNailStudio.Application.DTOs;
using FenNailStudio.Application.Interfaces;
using FenNailStudio.Web.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace FenNailStudio.Web.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ICustomerService _customerService;
        private readonly ITechnicianService _technicianService;
        private readonly INailServiceService _nailServiceService;

        public AppointmentsController(
            IAppointmentService appointmentService,
            ICustomerService customerService,
            ITechnicianService technicianService,
            INailServiceService nailServiceService)
        {
            _appointmentService = appointmentService;
            _customerService = customerService;
            _technicianService = technicianService;
            _nailServiceService = nailServiceService;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAsync();
            var viewModel = new AppointmentListViewModel
            {
                Appointments = appointments,
                Title = "所有預約"
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateAppointmentViewModel
            {
                AppointmentDto = new CreateAppointmentDto(),
                Customers = await _customerService.GetAllAsync(),
                Technicians = await _technicianService.GetAllAsync(),
                Services = await _nailServiceService.GetAllAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentDto appointmentDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _appointmentService.CreateAsync(appointmentDto);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var viewModel = new CreateAppointmentViewModel
            {
                AppointmentDto = appointmentDto,
                Customers = await _customerService.GetAllAsync(),
                Technicians = await _technicianService.GetAllAsync(),
                Services = await _nailServiceService.GetAllAsync()
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateAppointmentDto
            {
                Id = appointment.Id,
                TechnicianId = appointment.TechnicianId,
                ServiceId = appointment.ServiceId,
                AppointmentDateTime = appointment.AppointmentDateTime,
                Status = appointment.Status,
                Notes = appointment.Notes
            };

            var customer = await _customerService.GetByIdAsync(appointment.CustomerId);

            var viewModel = new EditAppointmentViewModel
            {
                AppointmentDto = updateDto,
                Technicians = await _technicianService.GetAllAsync(),
                Services = await _nailServiceService.GetAllAsync(),
                CustomerName = customer.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAppointmentDto appointmentDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _appointmentService.UpdateAsync(appointmentDto);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var appointment = await _appointmentService.GetByIdAsync(appointmentDto.Id);
            var customer = await _customerService.GetByIdAsync(appointment.CustomerId);

            var viewModel = new EditAppointmentViewModel
            {
                AppointmentDto = appointmentDto,
                Technicians = await _technicianService.GetAllAsync(),
                Services = await _nailServiceService.GetAllAsync(),
                CustomerName = customer.Name
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var viewModel = new AppointmentDetailsViewModel
            {
                Appointment = appointment
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            await _appointmentService.CancelAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _appointmentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
