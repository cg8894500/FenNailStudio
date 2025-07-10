using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Application.DTOs
{
    public class TechnicianDto //美甲技師基本資料
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string WorkingHours { get; set; }
    }

    public class CreateTechnicianDto //新增美甲技師基本資料
    {
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string WorkingHours { get; set; }
    }

    public class UpdateTechnicianDto //更新美甲技師基本資料
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string WorkingHours { get; set; }
    }
}
