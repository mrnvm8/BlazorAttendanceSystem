using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Repositories.DepartmentRepository
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default);
        Task<Department?> GetDepartmentByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddDepartmentAsync(Department department, CancellationToken cancellationToken = default);
        Task<bool> UpdateDepartmentAsync(Department department, CancellationToken cancellationToken = default);
        Task<bool> DeleteDepartmentAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
