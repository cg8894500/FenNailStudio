using FenNailStudio.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Application.Interfaces
{
    public interface INailServiceService
    {
        Task<NailServiceDto> GetByIdAsync(int id);
        Task<IEnumerable<NailServiceDto>> GetAllAsync();
        Task<int> CreateAsync(CreateNailServiceDto serviceDto);
        Task UpdateAsync(UpdateNailServiceDto serviceDto);
        Task DeleteAsync(int id);
    }
}
