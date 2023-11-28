using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Repositories.EmployeeAttendancesRespository
{
    public interface IEmployeeAttendanceRepository
    {
        Task<IEnumerable<EmployeeAttendance>> GetAllEmployeeAttendancesAsync(CancellationToken cancellationToken = default);
        Task<EmployeeAttendance?> GetEmployeeAttendanceByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddEmployeeAttendanceAsync(EmployeeAttendance attendance, CancellationToken cancellationToken = default);
        Task<bool> UpdateEmployeeAttendanceAsync(EmployeeAttendance attendance, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployeeAttendanceAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
