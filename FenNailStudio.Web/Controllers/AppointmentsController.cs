using Microsoft.AspNetCore.Mvc;
using FenNailStudio.Application.DTOs;
using FenNailStudio.Application.Interfaces;
using FenNailStudio.Web.Models.ViewModels;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FenNailStudio.Web.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ICustomerService _customerService;
        private readonly ITechnicianService _technicianService;
        private readonly INailServiceService _nailServiceService;
        private readonly IAuthService _authService;

        public AppointmentsController(
            IAppointmentService appointmentService,
            ICustomerService customerService,
            ITechnicianService technicianService,
            INailServiceService nailServiceService,
            IAuthService authService)
        {
            _appointmentService = appointmentService;
            _customerService = customerService;
            _technicianService = technicianService;
            _nailServiceService = nailServiceService;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAsync();
            var viewModel = new AppointmentListViewModel
            {
                Appointments = appointments,
                Title = _authService.IsCurrentUserAdmin() ? "所有預約" : "我的預約"
            };
            ViewBag.IsAdmin = _authService.IsCurrentUserAdmin();
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            // 獲取當前時間
            var now = DateTime.Now;

            // 計算下一個 30 分鐘的時間點
            var minutes = now.Minute;
            var minutesToAdd = minutes < 30 ? 30 - minutes : 60 - minutes;
            var defaultTime = now.AddMinutes(minutesToAdd).AddSeconds(-now.Second);

            // 如果是管理員，顯示所有客戶，否則只顯示當前用戶
            var customers = await _customerService.GetAllAsync();

            var viewModel = new CreateAppointmentViewModel
            {
                AppointmentDto = new CreateAppointmentDto
                {
                    AppointmentDateTime = defaultTime,
                    CustomerId = _authService.IsCurrentUserAdmin() ? 0 : _authService.GetCurrentUserId()
                },
                Customers = customers,
                Technicians = await _technicianService.GetAllAsync(),
                Services = await _nailServiceService.GetAllAsync()
            };

            ViewBag.IsAdmin = _authService.IsCurrentUserAdmin();
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
                    // 如果不是管理員，強制使用當前用戶 ID
                    if (!_authService.IsCurrentUserAdmin())
                    {
                        appointmentDto.CustomerId = _authService.GetCurrentUserId();
                    }

                    await _appointmentService.CreateAsync(appointmentDto);
                    return RedirectToAction(nameof(Index));
                }
                catch (UnauthorizedAccessException)
                {
                    return Forbid();
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

            ViewBag.IsAdmin = _authService.IsCurrentUserAdmin();
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
                Notes = appointment.Notes
            };

            var customer = await _customerService.GetByIdAsync(appointment.CustomerId);

            var viewModel = new EditAppointmentViewModel
            {
                AppointmentDto = updateDto,
                Technicians = await _technicianService.GetAllAsync(),
                Services = await _nailServiceService.GetAllAsync(),
                CustomerName = customer?.Name
            };

            ViewBag.IsAdmin = _authService.IsCurrentUserAdmin();
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
                catch (UnauthorizedAccessException)
                {
                    return Forbid();
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var appointment = await _appointmentService.GetByIdAsync(appointmentDto.Id);
            var customer = appointment != null
                ? await _customerService.GetByIdAsync(appointment.CustomerId)
                : null;

            var viewModel = new EditAppointmentViewModel
            {
                AppointmentDto = appointmentDto,
                Technicians = await _technicianService.GetAllAsync(),
                Services = await _nailServiceService.GetAllAsync(),
                CustomerName = customer?.Name
            };

            ViewBag.IsAdmin = _authService.IsCurrentUserAdmin();
            return View(viewModel);
        }

        public async Task<IActionResult> Detail(int id)
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

            ViewBag.IsAdmin = _authService.IsCurrentUserAdmin();
            return View(viewModel);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            ViewBag.IsAdmin = _authService.IsCurrentUserAdmin();
            return View(appointment);
        }

        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            try
            {
                await _appointmentService.CancelAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var appointment = await _appointmentService.GetByIdAsync(id);
                return View("Cancel", appointment);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            ViewBag.IsAdmin = _authService.IsCurrentUserAdmin();
            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _appointmentService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var appointment = await _appointmentService.GetByIdAsync(id);
                return View("Delete", appointment);
            }
        }
    }
}
