using FenNailStudio.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Application.DTOs
{
    public class AppointmentDto //預約基本資料
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int TechnicianId { get; set; }
        public string TechnicianName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal ServicePrice { get; set; }
        public int ServiceDuration { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string StatusName => Status.ToString();
        public string Notes { get; set; }
    }

    public class CreateAppointmentDto //新增預約基本資料
    {
        public int CustomerId { get; set; }
        public int TechnicianId { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateAppointmentDto //更新預約基本資料
    {
        public int Id { get; set; }
        public int TechnicianId { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string Notes { get; set; }
    }
}
