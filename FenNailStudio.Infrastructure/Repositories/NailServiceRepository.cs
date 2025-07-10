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
    public class NailServiceRepository : Repository<NailService>, INailServiceRepository
    {
        public NailServiceRepository(FenNailStudioDbContext context) : base(context)
        {
        }
    }
}
