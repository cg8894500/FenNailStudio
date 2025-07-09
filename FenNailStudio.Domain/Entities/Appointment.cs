using System;
using FenNailStudio.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int TechnicianId { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string Notes { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Technician Technician { get; set; }
        public virtual NailService Service { get; set; }
    }
}
