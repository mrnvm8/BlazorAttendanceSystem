using BlazorAttendanceSystem.Application.Services.AttendanceService;
using BlazorAttendanceSystem.Contract.Requests.AttendanceContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAttendanceSystem.Server.Controllers
{
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ILogger<AttendancesController> _logger;

        public AttendancesController(IAttendanceService attendanceService, ILogger<AttendancesController> logger)
        {
            _attendanceService = attendanceService 
                ?? throw new ArgumentNullException(nameof(attendanceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ApiEndpoints.Attendances.GetAll)]
        public async Task<ActionResult<IEnumerable<AttendanceResponse>>> GetAttendances(CancellationToken cancellationToken = default)
        {
            try
            {
                var attendances = await _attendanceService.GetAllAttendancesAsync(cancellationToken);
                return Ok(attendances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching attendances");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet(ApiEndpoints.Attendances.Get)]
        public async Task<ActionResult<AttendanceResponse>> GetAttendanceById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var attendance = await _attendanceService.GetAttendanceByIdAsync(id, cancellationToken);

                if (attendance == null)
                {
                    return NotFound();
                }

                return Ok(attendance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching attendance with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost(ApiEndpoints.Attendances.Create)]
        public async Task<ActionResult<Guid>> AddAttendance([FromBody] CreateAttendanceRequest attendanceRequest, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for AddAttendance request.");
                return BadRequest(ModelState);
            }

            try
            {
                var attendanceId = await _attendanceService.AddAttendanceAsync(attendanceRequest, cancellationToken);
                return Ok(attendanceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding attendance");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut(ApiEndpoints.Attendances.Update)]
        public async Task<ActionResult> UpdateAttendance(Guid id, [FromBody] UpdateAttendanceRequest attendanceRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _attendanceService.UpdateAttendanceAsync(id, attendanceRequest, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating attendance with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete(ApiEndpoints.Attendances.Delete)]
        public async Task<ActionResult> DeleteAttendance(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _attendanceService.DeleteAttendanceAsync(id, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting attendance with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
