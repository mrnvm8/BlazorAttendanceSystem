using BlazorAttendanceSystem.Application.Database;
using BlazorAttendanceSystem.Application.Repositories.AttendanceRepository;
using BlazorAttendanceSystem.Application.Repositories.DepartmentRepository;
using BlazorAttendanceSystem.Application.Repositories.EmployeeAttendancesRespository;
using BlazorAttendanceSystem.Application.Repositories.EmployeeLeaveRepository;
using BlazorAttendanceSystem.Application.Repositories.EmployeeRepository;
using BlazorAttendanceSystem.Application.Repositories.LeaveRepository;
using BlazorAttendanceSystem.Application.Repositories.OfficeRepository;
using BlazorAttendanceSystem.Application.Repositories.PersonRepository;
using BlazorAttendanceSystem.Application.Services.AttendanceService;
using BlazorAttendanceSystem.Application.Services.DepartmentService;
using BlazorAttendanceSystem.Application.Services.EmployeeAttendanceService;
using BlazorAttendanceSystem.Application.Services.EmployeeLeaveService;
using BlazorAttendanceSystem.Application.Services.EmployeeService;
using BlazorAttendanceSystem.Application.Services.LeaveService;
using BlazorAttendanceSystem.Application.Services.OfficeService;
using BlazorAttendanceSystem.Application.Services.PersonService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BlazorAttendanceSystem.Application
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //repository 
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeLeaveRepository, EmployeeLeaveRepository>();
            services.AddScoped<IEmployeeAttendanceRepository, EmployeeAttendanceRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();

            //Services
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IOfficeService, OfficeService>();
            services.AddScoped<ILeaveService, LeaveService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeLeaveService, EmployeeLeaveService>();
            services.AddScoped<IEmployeeAttendanceService, EmployeeAttendanceService>();
            services.AddScoped<IAttendanceService, AttendanceService>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            // Replace "your_connection_string" with your actual database connection string
            services.AddSingleton<INpgsqlDbConnectionFactory>(_ =>
            {
                return new NpgsqlDbConnectionFactory(connectionString);
            });

            services.AddSingleton<DbInitializer>();

            return services;
        }

        public static ILoggingBuilder PAddLogging(this ILoggingBuilder builder)
        {
            builder.AddSerilog();
            
            return builder;
        }
    }
}
