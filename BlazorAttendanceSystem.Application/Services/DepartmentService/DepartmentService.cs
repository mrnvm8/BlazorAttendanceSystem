using BlazorAttendanceSystem.Application.Mapping;
using BlazorAttendanceSystem.Application.Repositories.DepartmentRepository;
using BlazorAttendanceSystem.Contract.Requests.DepartmentContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BlazorAttendanceSystem.Application.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentService> _logger;
        
        public DepartmentService(IDepartmentRepository departmentRepository, 
            ILogger<DepartmentService> logger)
        {
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var departments = await _departmentRepository.GetAllDepartmentsAsync(cancellationToken);
                var departmentResponses = departments.Select(department => department.MapToResponse());

                _logger.LogInformation("Retrieved {Count} departments from the database", departmentResponses.Count());
                return departmentResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving departments from the database");
                throw;
            }
        }

        public async Task<DepartmentResponse?> GetDepartmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var department = await _departmentRepository.GetDepartmentByIdAsync(id, cancellationToken);

                if (department == null)
                {
                    _logger.LogInformation("Department with ID {Id} not found", id);
                    return null;
                }

                var departmentResponse = department.MapToResponse();
                _logger.LogInformation("Retrieved department with ID {Id} from the database", id);

                return departmentResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving department from the database");
                throw;
            }
        }

        public async Task<Guid> AddDepartmentAsync(CreateDepartmentRequest departmentRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var department = departmentRequest.MapToDepartment();
                var departmentId = await _departmentRepository.AddDepartmentAsync(department, cancellationToken);

                _logger.LogInformation("Added a new department with ID {Id} to the database", departmentId);
                return departmentId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a new department to the database");
                throw;
            }
        }

        public async Task<bool> UpdateDepartmentAsync(Guid id, UpdateDepartmentRequest departmentRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id, cancellationToken);

                if (existingDepartment == null)
                {
                    _logger.LogInformation("Department with ID {Id} not found", id);
                    return false;
                }

                var updatedDepartment = departmentRequest.MapToDepartment(id);
                var result = await _departmentRepository.UpdateDepartmentAsync(updatedDepartment, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Updated department with ID {Id} in the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to update department with ID {Id} in the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating department in the database");
                throw;
            }
        }

        public async Task<bool> DeleteDepartmentAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id, cancellationToken);

                if (existingDepartment == null)
                {
                    _logger.LogInformation("Department with ID {Id} not found", id);
                    return false;
                }

                var result = await _departmentRepository.DeleteDepartmentAsync(id, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Deleted department with ID {Id} from the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to delete department with ID {Id} from the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting department from the database");
                throw;
            }
        }
    }
}
