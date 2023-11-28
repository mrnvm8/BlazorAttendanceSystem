using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Repositories.OfficeRepository
{
    public interface IOfficeRepository
    {
        Task<IEnumerable<Office>> GetAllOfficesAsync(CancellationToken cancellationToken = default);
        Task<Office?> GetOfficeByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddOfficeAsync(Office office, CancellationToken cancellationToken = default);
        Task<bool> UpdateOfficeAsync(Office office, CancellationToken cancellationToken = default);
        Task<bool> DeleteOfficeAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
