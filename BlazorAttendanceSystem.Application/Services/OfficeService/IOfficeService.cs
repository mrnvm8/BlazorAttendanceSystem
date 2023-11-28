using BlazorAttendanceSystem.Contract.Requests.OfficeContract;
using BlazorAttendanceSystem.Contract.Responses;

namespace BlazorAttendanceSystem.Application.Services.OfficeService
{
    public interface IOfficeService
    {
        Task<IEnumerable<OfficeResponse>> GetAllOfficesAsync(CancellationToken cancellationToken = default);
        Task<OfficeResponse?> GetOfficeByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddOfficeAsync(CreateOfficeRequest officeRequest, CancellationToken cancellationToken = default);
        Task<bool> UpdateOfficeAsync(Guid id, UpdateOfficeRequest officeRequest, CancellationToken cancellationToken = default);
        Task<bool> DeleteOfficeAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
