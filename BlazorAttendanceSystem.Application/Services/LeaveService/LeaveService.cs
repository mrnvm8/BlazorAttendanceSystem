using BlazorAttendanceSystem.Application.Mapping;
using BlazorAttendanceSystem.Application.Repositories.LeaveRepository;
using BlazorAttendanceSystem.Contract.Requests.LeaveContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BlazorAttendanceSystem.Application.Services.LeaveService
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly ILogger<LeaveService> _logger;

        public LeaveService(ILeaveRepository leaveRepository,
            ILogger<LeaveService> logger)
        {
            _leaveRepository = leaveRepository ?? throw new ArgumentNullException(nameof(leaveRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<LeaveResponse>> GetAllLeavesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var leaves = await _leaveRepository.GetAllLeavesAsync(cancellationToken);
                var leaveResponses = leaves.Select(leave => leave.MapToResponse());

                _logger.LogInformation("Retrieved {Count} leaves from the database", leaveResponses.Count());
                return leaveResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving leaves from the database");
                throw;
            }
        }

        public async Task<LeaveResponse?> GetLeaveByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var leave = await _leaveRepository.GetLeaveByIdAsync(id, cancellationToken);

                if (leave == null)
                {
                    _logger.LogInformation("Leave with ID {Id} not found", id);
                    return null;
                }

                var leaveResponse = leave.MapToResponse();
                _logger.LogInformation("Retrieved leave with ID {Id} from the database", id);

                return leaveResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving leave from the database");
                throw;
            }
        }

        public async Task<Guid> AddLeaveAsync(CreateLeaveRequest leaveRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var leave = leaveRequest.MapToLeave();
                var leaveId = await _leaveRepository.AddLeaveAsync(leave, cancellationToken);

                _logger.LogInformation("Added a new leave with ID {Id} to the database", leaveId);
                return leaveId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a new leave to the database");
                throw;
            }
        }

        public async Task<bool> UpdateLeaveAsync(Guid id, UpdateLeaveRequest leaveRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingLeave = await _leaveRepository.GetLeaveByIdAsync(id, cancellationToken);

                if (existingLeave == null)
                {
                    _logger.LogInformation("Leave with ID {Id} not found", id);
                    return false;
                }

                var updatedLeave = leaveRequest.MapToLeave(id);
                var result = await _leaveRepository.UpdateLeaveAsync(updatedLeave, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Updated leave with ID {Id} in the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to update leave with ID {Id} in the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating leave in the database");
                throw;
            }
        }

        public async Task<bool> DeleteLeaveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingLeave = await _leaveRepository.GetLeaveByIdAsync(id, cancellationToken);

                if (existingLeave == null)
                {
                    _logger.LogInformation("Leave with ID {Id} not found", id);
                    return false;
                }

                var result = await _leaveRepository.DeleteLeaveAsync(id, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Deleted leave with ID {Id} from the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to delete leave with ID {Id} from the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting leave from the database");
                throw;
            }
        }
    }
}
