using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Repositories.LeaveRepository
{
    public interface ILeaveRepository
    {
        Task<IEnumerable<Leave>> GetAllLeavesAsync(CancellationToken cancellationToken = default);
        Task<Leave?> GetLeaveByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddLeaveAsync(Leave leave, CancellationToken cancellationToken = default);
        Task<bool> UpdateLeaveAsync(Leave leave, CancellationToken cancellationToken = default);
        Task<bool> DeleteLeaveAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
