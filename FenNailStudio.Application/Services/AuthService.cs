using AutoMapper;
using FenNailStudio.Application.DTOs;
using FenNailStudio.Application.Interfaces;
using FenNailStudio.Domain.Entities;
using FenNailStudio.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FenNailStudio.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AuthService(
            ICustomerRepository customerRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<bool> LoginAsync(LoginDto model)
        {
            // 檢查用戶登入
            var customer = await _customerRepository.GetByEmailAsync(model.Email);
            if (customer != null && VerifyPassword(customer.Password, model.Password))
            {
                // 建立用戶的 Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, customer.Name),
                    new Claim(ClaimTypes.Email, customer.Email),
                    new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                    new Claim(ClaimTypes.Role, customer.Role == 0 ? "Admin" : "Customer")
                };

                await SignInAsync(claims);
                return true;
            }

            return false;
        }

        private async Task SignInAsync(List<Claim> claims)
        {
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task<bool> RegisterAsync(RegisterDto model)
        {
            // 檢查郵箱是否已被使用
            var existingCustomer = await _customerRepository.GetByEmailAsync(model.Email);
            if (existingCustomer != null)
            {
                return false;
            }

            // 創建新用戶
            var customer = new Customer
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password, // 實際應用中應該雜湊密碼
                Phone = model.Phone,
                City = model.City,
                Role = 1, // 默認為普通用戶
                RegisterDate = DateTime.Now
            };

            await _customerRepository.AddAsync(customer);
            return true;
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public int GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId != null ? int.Parse(userId) : 0;
        }

        public bool IsCurrentUserAdmin()
        {
            var role = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
            return role == "Admin";
        }

        private bool VerifyPassword(string storedPassword, string inputPassword)
        {
            // 在實際應用中，這裡應該使用適當的密碼雜湊和驗證方法
            // 這裡簡化為直接比較
            return storedPassword == inputPassword;
        }
    }
}
