using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? City { get; set; }                 // 居住地
        public string? Occupation { get; set; }           // 職業
        public int Role { get; set; } = 1;                // 權限（0: 管理者, 1: 使用者）
        public DateTime RegisterDate { get; set; }

        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}
