using FenNailStudio.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto model);
        Task<bool> LoginAsync(LoginDto model);
        Task LogoutAsync();
        int GetCurrentUserId();
        bool IsCurrentUserAdmin();
    }
}
