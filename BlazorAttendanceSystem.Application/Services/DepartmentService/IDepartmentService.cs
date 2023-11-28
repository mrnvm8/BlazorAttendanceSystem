using BlazorAttendanceSystem.Contract.Requests.DepartmentContract;
using BlazorAttendanceSystem.Contract.Responses;

namespace BlazorAttendanceSystem.Application.Services.DepartmentService
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default);
        Task<DepartmentResponse?> GetDepartmentByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddDepartmentAsync(CreateDepartmentRequest departmentRequest, CancellationToken cancellationToken = default);
        Task<bool> UpdateDepartmentAsync(Guid id, UpdateDepartmentRequest departmentRequest, CancellationToken cancellationToken = default);
        Task<bool> DeleteDepartmentAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
