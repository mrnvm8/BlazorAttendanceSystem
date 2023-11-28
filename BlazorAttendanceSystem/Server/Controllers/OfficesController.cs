using BlazorAttendanceSystem.Application.Services.OfficeService;
using BlazorAttendanceSystem.Contract.Requests.OfficeContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAttendanceSystem.Server.Controllers
{
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        private readonly ILogger<OfficesController> _logger;

        public OfficesController(IOfficeService officeService, ILogger<OfficesController> logger)
        {
            _officeService = officeService ?? throw new ArgumentNullException(nameof(officeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ApiEndpoints.Offices.GetAll)]
        public async Task<ActionResult<IEnumerable<OfficeResponse>>> GetOffices(CancellationToken cancellationToken = default)
        {
            try
            {
                var offices = await _officeService.GetAllOfficesAsync(cancellationToken);
                return Ok(offices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching offices");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet(ApiEndpoints.Offices.Get)]
        public async Task<ActionResult<OfficeResponse>> GetOfficeById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var office = await _officeService.GetOfficeByIdAsync(id, cancellationToken);

                if (office == null)
                {
                    return NotFound();
                }

                return Ok(office);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching office with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost(ApiEndpoints.Offices.Create)]
        public async Task<ActionResult<Guid>> AddOffice([FromBody] CreateOfficeRequest officeRequest, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for AddOffice request.");
                return BadRequest(ModelState);
            }

            try
            {
                var officeId = await _officeService.AddOfficeAsync(officeRequest, cancellationToken);
                return Ok(officeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding office");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut(ApiEndpoints.Offices.Update)]
        public async Task<ActionResult> UpdateOffice(Guid id, [FromBody] UpdateOfficeRequest officeRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _officeService.UpdateOfficeAsync(id, officeRequest, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating office with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete(ApiEndpoints.Offices.Delete)]
        public async Task<ActionResult> DeleteOffice(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _officeService.DeleteOfficeAsync(id, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting office with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
