using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Repositories.EmployeeRepository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken = default);
        Task<Employee?> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddEmployeeAsync(Employee employee, CancellationToken cancellationToken = default);
        Task<bool> UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
