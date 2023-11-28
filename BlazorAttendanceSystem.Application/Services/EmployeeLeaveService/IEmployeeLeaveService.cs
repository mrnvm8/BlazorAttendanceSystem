using BlazorAttendanceSystem.Contract.Requests.EmployeeLeaveContract;
using BlazorAttendanceSystem.Contract.Responses;

namespace BlazorAttendanceSystem.Application.Services.EmployeeLeaveService
{
    public interface IEmployeeLeaveService
    {
        Task<IEnumerable<EmployeeLeaveResponse>> GetAllEmployeeLeavesAsync(CancellationToken cancellationToken = default);
        Task<EmployeeLeaveResponse?> GetEmployeeLeaveByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddEmployeeLeaveAsync(CreateEmployeeLeaveRequest leaveRequest, CancellationToken cancellationToken = default);
        Task<bool> UpdateEmployeeLeaveAsync(Guid id, UpdateEmployeeLeaveRequest leaveRequest, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployeeLeaveAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
