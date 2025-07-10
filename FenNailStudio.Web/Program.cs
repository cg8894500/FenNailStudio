using Microsoft.EntityFrameworkCore;
using FenNailStudio.Application.Interfaces;
using FenNailStudio.Application.Mapping;
using FenNailStudio.Application.Services;
using FenNailStudio.Domain.Interfaces;
using FenNailStudio.Infrastructure.Data;
using FenNailStudio.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

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

// ���U AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// �T�O��Ʈw�w�إ�
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<FenNailStudioDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
