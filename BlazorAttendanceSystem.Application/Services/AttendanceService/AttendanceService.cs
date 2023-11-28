using BlazorAttendanceSystem.Application.Mapping;
using BlazorAttendanceSystem.Application.Repositories.AttendanceRepository;
using BlazorAttendanceSystem.Contract.Requests.AttendanceContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BlazorAttendanceSystem.Application.Services.AttendanceService
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ILogger<AttendanceService> _logger;

        public AttendanceService(IAttendanceRepository attendanceRepository, ILogger<AttendanceService> logger)
        {
            _attendanceRepository = attendanceRepository ?? throw new ArgumentNullException(nameof(attendanceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<AttendanceResponse>> GetAllAttendancesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var attendances = await _attendanceRepository.GetAllAttendancesAsync(cancellationToken);
                var attendanceResponses = attendances.Select(attendance => attendance.MapToResponse());

                _logger.LogInformation("Retrieved {Count} attendances from the database", attendanceResponses.Count());
                return attendanceResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving attendances from the database");
                throw;
            }
        }

        public async Task<AttendanceResponse?> GetAttendanceByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var attendance = await _attendanceRepository.GetAttendanceByIdAsync(id, cancellationToken);

                if (attendance == null)
                {
                    _logger.LogInformation("Attendance with ID {Id} not found", id);
                    return null;
                }

                var attendanceResponse = attendance.MapToResponse();
                _logger.LogInformation("Retrieved attendance with ID {Id} from the database", id);

                return attendanceResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving attendance from the database");
                throw;
            }
        }

        public async Task<Guid> AddAttendanceAsync(CreateAttendanceRequest attendanceRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var attendance = attendanceRequest.MapToAttendance();
                var attendanceId = await _attendanceRepository.AddAttendanceAsync(attendance, cancellationToken);

                _logger.LogInformation("Added a new attendance with ID {Id} to the database", attendanceId);
                return attendanceId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a new attendance to the database");
                throw;
            }
        }

        public async Task<bool> UpdateAttendanceAsync(Guid id, UpdateAttendanceRequest attendanceRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingAttendance = await _attendanceRepository.GetAttendanceByIdAsync(id, cancellationToken);

                if (existingAttendance == null)
                {
                    _logger.LogInformation("Attendance with ID {Id} not found", id);
                    return false;
                }

                var updatedAttendance = attendanceRequest.MapToAttendance(id);
                var result = await _attendanceRepository.UpdateAttendanceAsync(updatedAttendance, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Updated attendance with ID {Id} in the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to update attendance with ID {Id} in the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating attendance in the database");
                throw;
            }
        }

        public async Task<bool> DeleteAttendanceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingAttendance = await _attendanceRepository.GetAttendanceByIdAsync(id, cancellationToken);

                if (existingAttendance == null)
                {
                    _logger.LogInformation("Attendance with ID {Id} not found", id);
                    return false;
                }

                var result = await _attendanceRepository.DeleteAttendanceAsync(id, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Deleted attendance with ID {Id} from the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to delete attendance with ID {Id} from the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting attendance from the database");
                throw;
            }
        }
    }
}
