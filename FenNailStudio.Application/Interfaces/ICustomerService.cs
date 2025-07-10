using FenNailStudio.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto> GetByIdAsync(int id);
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<CustomerDto> GetByEmailAsync(string email);
        Task<int> CreateAsync(CreateCustomerDto customerDto);
        Task UpdateAsync(UpdateCustomerDto customerDto);
        Task DeleteAsync(int id);
    }
}
