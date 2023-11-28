using BlazorAttendanceSystem.Contract.Requests.EmployeeContract;
using BlazorAttendanceSystem.Contract.Responses;

namespace BlazorAttendanceSystem.Application.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync(CancellationToken cancellationToken = default);
        Task<EmployeeResponse?> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddEmployeeAsync(CreateEmployeeRequest employeeRequest, CancellationToken cancellationToken = default);
        Task<bool> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequest employeeRequest, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default);
    }

}
