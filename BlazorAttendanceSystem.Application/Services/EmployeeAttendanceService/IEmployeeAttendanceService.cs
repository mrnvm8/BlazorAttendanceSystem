using BlazorAttendanceSystem.Contract.Requests.EmployeeAttendanceContract;
using BlazorAttendanceSystem.Contract.Responses;

namespace BlazorAttendanceSystem.Application.Services.EmployeeAttendanceService
{
    public interface IEmployeeAttendanceService
    {
        Task<IEnumerable<EmployeeAttendanceResponse>> GetAllEmployeeAttendancesAsync(CancellationToken cancellationToken = default);
        Task<EmployeeAttendanceResponse?> GetEmployeeAttendanceByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddEmployeeAttendanceAsync(CreateEmployeeAttendanceRequest attendanceRequest, CancellationToken cancellationToken = default);
        Task<bool> UpdateEmployeeAttendanceAsync(Guid id, UpdateEmployeeAttendanceRequest attendanceRequest, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployeeAttendanceAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
