using Microsoft.AspNetCore.Mvc;
using FenNailStudio.Application.DTOs;
using FenNailStudio.Application.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FenNailStudio.Web.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthService _authService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(
            ICustomerService customerService,
            IAuthService authService,
            ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _authService = authService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();
            ViewBag.IsAdmin = _authService.IsCurrentUserAdmin();
            return View(customers);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateCustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                City = customer.City,
                Occupation = customer.Occupation
            };

            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCustomerDto customerDto)
        {
            // 檢查 URL ID 與表單 ID 是否匹配
            if (id != customerDto.Id)
            {
                _logger.LogWarning("URL ID ({UrlId}) 與表單 ID ({FormId}) 不匹配", id, customerDto.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 記錄嘗試更新客戶
                    _logger.LogInformation("嘗試更新客戶: ID={Id}, Name={Name}, Email={Email}",
                        customerDto.Id, customerDto.Name, customerDto.Email);

                    await _customerService.UpdateAsync(customerDto);

                    // 記錄更新成功
                    _logger.LogInformation("客戶 ID={Id} 更新成功", customerDto.Id);

                    return RedirectToAction(nameof(Index));
                }
                catch (UnauthorizedAccessException ex)
                {
                    // 記錄未授權訪問
                    _logger.LogWarning(ex, "未授權的更新嘗試: {Message}", ex.Message);
                    return Forbid();
                }
                catch (Exception ex)
                {
                    // 記錄異常
                    _logger.LogError(ex, "更新客戶 ID={Id} 時發生錯誤", customerDto.Id);
                    ModelState.AddModelError("", $"更新客戶失敗: {ex.Message}");
                }
            }
            else
            {
                // 記錄模型驗證失敗
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("模型驗證失敗: {ErrorMessage}", error.ErrorMessage);
                }
            }

            return View(customerDto);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _customerService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除客戶 ID={Id} 時發生錯誤", id);
                ModelState.AddModelError("", $"刪除客戶失敗: {ex.Message}");
                var customer = await _customerService.GetByIdAsync(id);
                return View("Delete", customer);
            }
        }
    }
}
