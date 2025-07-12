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

// ���U DbContext
builder.Services.AddDbContext<FenNailStudioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���U Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<INailServiceRepository, NailServiceRepository>();
builder.Services.AddScoped<ITechnicianRepository, TechnicianRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

// ���U Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<INailServiceService, NailServiceService>();
builder.Services.AddScoped<ITechnicianService, TechnicianService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// �K�[ HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// �K�[�{�ҪA��
builder.Services.AddScoped<IAuthService, AuthService>();

// �t�m Cookie �{��
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

// �K�[���v
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<MappingProfile>();
});

var app = builder.Build();

// ���� AutoMapper �t�m
app.Lifetime.ApplicationStarted.Register(() => {
    try
    {
        var mapper = app.Services.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
        Console.WriteLine("AutoMapper �t�m���Ҧ��\");
    }
    catch (AutoMapperConfigurationException ex)
    {
        Console.WriteLine("===== AutoMapper �t�m���~ =====");
        Console.WriteLine(ex.Message);

        // �ϥΤϮg�w���a����M��������H��
        var typesProperty = ex.GetType().GetProperty("Types");
        if (typesProperty != null)
        {
            var typesValue = typesProperty.GetValue(ex);
            if (typesValue != null)
            {
                Console.WriteLine($"���D�M�g����: {typesValue}");
            }
        }

        // ��ܧ��㪺���`�ԲӫH��
        Console.WriteLine("���㲧�`�ԲӫH��:");
        Console.WriteLine(ex.ToString());

        // �b�}�o���Ҥ��i�H����~��B��
        if (app.Environment.IsDevelopment())
        {
            Console.WriteLine("ĵ�i: �~��B��A�� AutoMapper �t�m�����~");
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

// �T�O��Ʈw�w�إ�
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<FenNailStudioDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
