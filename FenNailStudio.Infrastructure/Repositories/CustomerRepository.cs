using FenNailStudio.Domain.Entities;
using FenNailStudio.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using FenNailStudio.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(FenNailStudioDbContext context) : base(context)
        {
        }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
