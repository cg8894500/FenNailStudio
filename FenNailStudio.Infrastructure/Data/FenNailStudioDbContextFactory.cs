using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenNailStudio.Infrastructure.Data
{
    public class FenNailStudioDbContextFactory : IDesignTimeDbContextFactory<FenNailStudioDbContext>
    {
        public FenNailStudioDbContext CreateDbContext(string[] args)
        {
            // 獲取當前目錄
            string projectDir = Directory.GetCurrentDirectory();

            // 嘗試找到解決方案根目錄
            while (!File.Exists(Path.Combine(projectDir, "FenNailStudio.sln")) &&
                   Directory.GetParent(projectDir) != null)
            {
                projectDir = Directory.GetParent(projectDir).FullName;
            }

            // 構建配置
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(projectDir, "FenNailStudio.Web")) // 假設配置文件在Web項目中
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            // 創建DbContext選項
            var optionsBuilder = new DbContextOptionsBuilder<FenNailStudioDbContext>();

            // 獲取連接字符串
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            // 如果找不到連接字符串，使用硬編碼的默認值
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "(localdb)\\ProjectModels;Database=FenNailStudio;Trusted_Connection=True;MultipleActiveResultSets=true";
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new FenNailStudioDbContext(optionsBuilder.Options);
        }
    }
}
