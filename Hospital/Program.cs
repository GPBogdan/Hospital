using BLL.Service;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityHospitalContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityHospitalContextConnection' not found.");

builder.Services.AddDbContext<HospitalContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<HospitalContext>();

//Add Service Injection
builder.Services.AddScoped<IRepository<Patient>, PatientRepository>();
builder.Services.AddScoped<PatientService, PatientService>();
builder.Services.AddScoped<IRepository<Doctor>, DoctorRepository>();
builder.Services.AddScoped<DoctorService, DoctorService>();
builder.Services.AddScoped<IRepository<Department>, DepartmentRepository>();
builder.Services.AddScoped<DepartmentService, DepartmentService>();
builder.Services.AddScoped<IRepository<MedicalProcedure>, MedicalProcedureRepository>();
builder.Services.AddScoped<MedicalProcedureService, MedicalProcedureService>();
builder.Services.AddScoped<IRepository<Appointment>, AppointmentRepository>();
builder.Services.AddScoped<AppointmentService, AppointmentService>(); 
builder.Services.AddScoped<HospitalUserService, HospitalUserService>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
