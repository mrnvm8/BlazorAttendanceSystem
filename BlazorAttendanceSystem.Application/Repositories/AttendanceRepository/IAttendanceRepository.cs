using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Repositories.AttendanceRepository
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAllAttendancesAsync(CancellationToken cancellationToken = default);
        Task<Attendance?> GetAttendanceByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddAttendanceAsync(Attendance attendance, CancellationToken cancellationToken = default);
        Task<bool> UpdateAttendanceAsync(Attendance attendance, CancellationToken cancellationToken = default);
        Task<bool> DeleteAttendanceAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
