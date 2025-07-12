using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Application.DTOs
{
    public class CustomerDto //顧客基本資料
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }                 // 居住地
        public string? Occupation { get; set; }           // 職業
        public DateTime RegisterDate { get; set; }
    }

    public class CreateCustomerDto //新增顧客基本資料
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }                 // 居住地
        public string? Occupation { get; set; }           // 職業
    }

    public class UpdateCustomerDto //更新顧客基本資料
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }                 // 居住地
        public string? Occupation { get; set; }           // 職業
    }
}
