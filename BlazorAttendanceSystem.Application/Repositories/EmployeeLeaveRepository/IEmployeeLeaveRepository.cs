using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Repositories.EmployeeLeaveRepository
{
    public interface IEmployeeLeaveRepository
    {
        Task<IEnumerable<EmployeeLeave>> GetAllEmployeeLeavesAsync(CancellationToken cancellationToken = default);
        Task<EmployeeLeave?> GetEmployeeLeaveByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddEmployeeLeaveAsync(EmployeeLeave employeeLeave, CancellationToken cancellationToken = default);
        Task<bool> UpdateEmployeeLeaveAsync(EmployeeLeave employeeLeave, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployeeLeaveAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
