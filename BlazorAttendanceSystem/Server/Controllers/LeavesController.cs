using BlazorAttendanceSystem.Application.Services.LeaveService;
using BlazorAttendanceSystem.Contract.Requests.LeaveContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAttendanceSystem.Server.Controllers
{
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly ILogger<LeavesController> _logger;

        public LeavesController(ILeaveService leaveService, ILogger<LeavesController> logger)
        {
            _leaveService = leaveService ?? throw new ArgumentNullException(nameof(leaveService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ApiEndpoints.Leaves.GetAll)]
        public async Task<ActionResult<IEnumerable<LeaveResponse>>> GetLeaves(CancellationToken cancellationToken = default)
        {
            try
            {
                var leaves = await _leaveService.GetAllLeavesAsync(cancellationToken);
                return Ok(leaves);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching leaves");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet(ApiEndpoints.Leaves.Get)]
        public async Task<ActionResult<LeaveResponse>> GetLeaveById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var leave = await _leaveService.GetLeaveByIdAsync(id, cancellationToken);

                if (leave == null)
                {
                    return NotFound();
                }

                return Ok(leave);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching leave with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost(ApiEndpoints.Leaves.Create)]
        public async Task<ActionResult<Guid>> AddLeave([FromBody] CreateLeaveRequest leaveRequest, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for AddLeave request.");
                return BadRequest(ModelState);
            }
            try
            {
                var leaveId = await _leaveService.AddLeaveAsync(leaveRequest, cancellationToken);
                return Ok(leaveId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding leave");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut(ApiEndpoints.Leaves.Update)]
        public async Task<ActionResult> UpdateLeave(Guid id, [FromBody] UpdateLeaveRequest leaveRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _leaveService.UpdateLeaveAsync(id, leaveRequest, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating leave with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete(ApiEndpoints.Leaves.Delete)]
        public async Task<ActionResult> DeleteLeave(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _leaveService.DeleteLeaveAsync(id, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting leave with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
