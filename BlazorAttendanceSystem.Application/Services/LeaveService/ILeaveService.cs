using BlazorAttendanceSystem.Contract.Requests.LeaveContract;
using BlazorAttendanceSystem.Contract.Responses;

namespace BlazorAttendanceSystem.Application.Services.LeaveService
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveResponse>> GetAllLeavesAsync(CancellationToken cancellationToken = default);
        Task<LeaveResponse?> GetLeaveByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddLeaveAsync(CreateLeaveRequest leaveRequest, CancellationToken cancellationToken = default);
        Task<bool> UpdateLeaveAsync(Guid id, UpdateLeaveRequest leaveRequest, CancellationToken cancellationToken = default);
        Task<bool> DeleteLeaveAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
