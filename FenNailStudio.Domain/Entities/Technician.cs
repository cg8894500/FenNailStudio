using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Domain.Entities
{
    public class Technician
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string WorkingHours { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
