using BlazorAttendanceSystem.Contract.Requests.AttendanceContract;
using BlazorAttendanceSystem.Contract.Responses;

namespace BlazorAttendanceSystem.Application.Services.AttendanceService
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceResponse>> GetAllAttendancesAsync(CancellationToken cancellationToken = default);
        Task<AttendanceResponse?> GetAttendanceByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddAttendanceAsync(CreateAttendanceRequest attendanceRequest, CancellationToken cancellationToken = default);
        Task<bool> UpdateAttendanceAsync(Guid id, UpdateAttendanceRequest attendanceRequest, CancellationToken cancellationToken = default);
        Task<bool> DeleteAttendanceAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
