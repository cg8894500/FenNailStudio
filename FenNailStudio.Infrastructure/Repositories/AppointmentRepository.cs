using Microsoft.EntityFrameworkCore;
using FenNailStudio.Domain.Entities;
using FenNailStudio.Domain.Interfaces;
using FenNailStudio.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Infrastructure.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(FenNailStudioDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Appointment>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Include(a => a.Customer)
                .Include(a => a.Technician)
                .Include(a => a.Service)
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByTechnicianIdAsync(int technicianId)
        {
            return await _dbSet
                .Include(a => a.Customer)
                .Include(a => a.Technician)
                .Include(a => a.Service)
                .Where(a => a.TechnicianId == technicianId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _dbSet
                .Include(a => a.Customer)
                .Include(a => a.Technician)
                .Include(a => a.Service)
                .Where(a => a.AppointmentDateTime >= start && a.AppointmentDateTime <= end)
                .ToListAsync();
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Customer)
                .Include(a => a.Technician)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _dbSet
                .Include(a => a.Customer)
                .Include(a => a.Technician)
                .Include(a => a.Service)
                .ToListAsync();
        }
    }
}
