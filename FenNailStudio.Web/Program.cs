using Microsoft.EntityFrameworkCore;
using FenNailStudio.Application.Interfaces;
using FenNailStudio.Application.Mapping;
using FenNailStudio.Application.Services;
using FenNailStudio.Domain.Interfaces;
using FenNailStudio.Infrastructure.Data;
using FenNailStudio.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 註冊 DbContext
builder.Services.AddDbContext<FenNailStudioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 註冊 Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<INailServiceRepository, NailServiceRepository>();
builder.Services.AddScoped<ITechnicianRepository, TechnicianRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

// 註冊 Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<INailServiceService, NailServiceService>();
builder.Services.AddScoped<ITechnicianService, TechnicianService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// 添加 HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// 添加認證服務
builder.Services.AddScoped<IAuthService, AuthService>();

// 配置 Cookie 認證
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

// 添加授權
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<MappingProfile>();
});

var app = builder.Build();

// 驗證 AutoMapper 配置
app.Lifetime.ApplicationStarted.Register(() => {
    try
    {
        var mapper = app.Services.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
        Console.WriteLine("AutoMapper 配置驗證成功");
    }
    catch (AutoMapperConfigurationException ex)
    {
        Console.WriteLine("===== AutoMapper 配置錯誤 =====");
        Console.WriteLine(ex.Message);

        // 使用反射安全地獲取和顯示類型信息
        var typesProperty = ex.GetType().GetProperty("Types");
        if (typesProperty != null)
        {
            var typesValue = typesProperty.GetValue(ex);
            if (typesValue != null)
            {
                Console.WriteLine($"問題映射類型: {typesValue}");
            }
        }

        // 顯示完整的異常詳細信息
        Console.WriteLine("完整異常詳細信息:");
        Console.WriteLine(ex.ToString());

        // 在開發環境中可以選擇繼續運行
        if (app.Environment.IsDevelopment())
        {
            Console.WriteLine("警告: 繼續運行，但 AutoMapper 配置有錯誤");
        }
        else
        {
            throw;
        }
    }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{Id?}");

// 確保資料庫已建立
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<FenNailStudioDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
