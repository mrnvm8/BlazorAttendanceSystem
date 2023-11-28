using BlazorAttendanceSystem.Application.Mapping;
using BlazorAttendanceSystem.Application.Repositories.EmployeeLeaveRepository;
using BlazorAttendanceSystem.Contract.Requests.EmployeeLeaveContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BlazorAttendanceSystem.Application.Services.EmployeeLeaveService
{
    public class EmployeeLeaveService : IEmployeeLeaveService
    {
        private readonly IEmployeeLeaveRepository _employeeLeaveRepository;
        private readonly ILogger<EmployeeLeaveService> _logger;

        public EmployeeLeaveService(IEmployeeLeaveRepository employeeLeaveRepository, 
            ILogger<EmployeeLeaveService> logger)
        {
            _employeeLeaveRepository = employeeLeaveRepository ??
                throw new ArgumentNullException(nameof(employeeLeaveRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<EmployeeLeaveResponse>> GetAllEmployeeLeavesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var employeeLeaves = await _employeeLeaveRepository.GetAllEmployeeLeavesAsync(cancellationToken);
                var employeeLeaveResponses = employeeLeaves.Select(employeeLeave => employeeLeave.MapToResponse());

                _logger.LogInformation("Retrieved {Count} employee leaves from the database", employeeLeaveResponses.Count());
                return employeeLeaveResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving employee leaves from the database");
                throw;
            }
        }

        public async Task<EmployeeLeaveResponse?> GetEmployeeLeaveByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var employeeLeave = await _employeeLeaveRepository.GetEmployeeLeaveByIdAsync(id, cancellationToken);

                if (employeeLeave == null)
                {
                    _logger.LogInformation("Employee leave with ID {Id} not found", id);
                    return null;
                }

                var employeeLeaveResponse = employeeLeave.MapToResponse();
                _logger.LogInformation("Retrieved employee leave with ID {Id} from the database", id);

                return employeeLeaveResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving employee leave from the database");
                throw;
            }
        }

        public async Task<Guid> AddEmployeeLeaveAsync(CreateEmployeeLeaveRequest employeeLeaveRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var employeeLeave = employeeLeaveRequest.MapToEmployeeLeave();
                var employeeLeaveId = await _employeeLeaveRepository.AddEmployeeLeaveAsync(employeeLeave, cancellationToken);

                _logger.LogInformation("Added a new employee leave with ID {Id} to the database", employeeLeaveId);
                return employeeLeaveId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a new employee leave to the database");
                throw;
            }
        }

        public async Task<bool> UpdateEmployeeLeaveAsync(Guid id, UpdateEmployeeLeaveRequest employeeLeaveRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingEmployeeLeave = await _employeeLeaveRepository.GetEmployeeLeaveByIdAsync(id, cancellationToken);

                if (existingEmployeeLeave == null)
                {
                    _logger.LogInformation("Employee leave with ID {Id} not found", id);
                    return false;
                }

                var updatedEmployeeLeave = employeeLeaveRequest.MapToEmployeeLeave(id);
                var result = await _employeeLeaveRepository.UpdateEmployeeLeaveAsync(updatedEmployeeLeave, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Updated employee leave with ID {Id} in the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to update employee leave with ID {Id} in the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating employee leave in the database");
                throw;
            }
        }

        public async Task<bool> DeleteEmployeeLeaveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingEmployeeLeave = await _employeeLeaveRepository.GetEmployeeLeaveByIdAsync(id, cancellationToken);

                if (existingEmployeeLeave == null)
                {
                    _logger.LogInformation("Employee leave with ID {Id} not found", id);
                    return false;
                }

                var result = await _employeeLeaveRepository.DeleteEmployeeLeaveAsync(id, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Deleted employee leave with ID {Id} from the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to delete employee leave with ID {Id} from the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting employee leave from the database");
                throw;
            }
        }
    }
}
