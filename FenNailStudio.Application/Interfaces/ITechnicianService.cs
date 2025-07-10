using FenNailStudio.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Application.Interfaces
{
    public interface ITechnicianService
    {
        Task<TechnicianDto> GetByIdAsync(int id);
        Task<IEnumerable<TechnicianDto>> GetAllAsync();
        Task<int> CreateAsync(CreateTechnicianDto technicianDto);
        Task UpdateAsync(UpdateTechnicianDto technicianDto);
        Task DeleteAsync(int id);
    }
}
