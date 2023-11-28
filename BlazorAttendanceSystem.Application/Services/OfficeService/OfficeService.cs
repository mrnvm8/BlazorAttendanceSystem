using BlazorAttendanceSystem.Application.Mapping;
using BlazorAttendanceSystem.Application.Repositories.OfficeRepository;
using BlazorAttendanceSystem.Contract.Requests.OfficeContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BlazorAttendanceSystem.Application.Services.OfficeService
{
    public class OfficeService : IOfficeService
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly ILogger<OfficeService> _logger;

        public OfficeService(IOfficeRepository officeRepository, 
            ILogger<OfficeService> logger)
        {
            _officeRepository = officeRepository ??
                throw new ArgumentNullException(nameof(officeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<OfficeResponse>> GetAllOfficesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var offices = await _officeRepository.GetAllOfficesAsync(cancellationToken);
                var officeResponses = offices.Select(office => office.MapToResponse());

                _logger.LogInformation("Retrieved {Count} offices from the database", officeResponses.Count());
                return officeResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving offices from the database");
                throw;
            }
        }

        public async Task<OfficeResponse?> GetOfficeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var office = await _officeRepository.GetOfficeByIdAsync(id, cancellationToken);

                if (office == null)
                {
                    _logger.LogInformation("Office with ID {Id} not found", id);
                    return null;
                }

                var officeResponse = office.MapToResponse();
                _logger.LogInformation("Retrieved office with ID {Id} from the database", id);

                return officeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving office from the database");
                throw;
            }
        }

        public async Task<Guid> AddOfficeAsync(CreateOfficeRequest officeRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var office = officeRequest.MapToOffice();
                var officeId = await _officeRepository.AddOfficeAsync(office, cancellationToken);

                _logger.LogInformation("Added a new office with ID {Id} to the database", officeId);
                return officeId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a new office to the database");
                throw;
            }
        }

        public async Task<bool> UpdateOfficeAsync(Guid id, UpdateOfficeRequest officeRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingOffice = await _officeRepository.GetOfficeByIdAsync(id, cancellationToken);

                if (existingOffice == null)
                {
                    _logger.LogInformation("Office with ID {Id} not found", id);
                    return false;
                }

                var updatedOffice = officeRequest.MapToOffice(id);
                var result = await _officeRepository.UpdateOfficeAsync(updatedOffice, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Updated office with ID {Id} in the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to update office with ID {Id} in the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating office in the database");
                throw;
            }
        }

        public async Task<bool> DeleteOfficeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingOffice = await _officeRepository.GetOfficeByIdAsync(id, cancellationToken);

                if (existingOffice == null)
                {
                    _logger.LogInformation("Office with ID {Id} not found", id);
                    return false;
                }

                var result = await _officeRepository.DeleteOfficeAsync(id, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Deleted office with ID {Id} from the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to delete office with ID {Id} from the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting office from the database");
                throw;
            }
        }
    }
}
