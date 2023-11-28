using BlazorAttendanceSystem.Application.Services.EmployeeLeaveService;
using BlazorAttendanceSystem.Contract.Requests.EmployeeLeaveContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAttendanceSystem.Server.Controllers
{
    [ApiController]
    public class EmployeesLeavesController : ControllerBase
    {
        private readonly IEmployeeLeaveService _employeeLeaveService;
        private readonly ILogger<EmployeesLeavesController> _logger;

        public EmployeesLeavesController(IEmployeeLeaveService employeeLeaveService, ILogger<EmployeesLeavesController> logger)
        {
            _employeeLeaveService = employeeLeaveService ?? throw new ArgumentNullException(nameof(employeeLeaveService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ApiEndpoints.EmployeesLeave.GetAll)]
        public async Task<ActionResult<IEnumerable<EmployeeLeaveResponse>>> GetEmployeeLeaves(CancellationToken cancellationToken = default)
        {
            try
            {
                var employeeLeaves = await _employeeLeaveService.GetAllEmployeeLeavesAsync(cancellationToken);
                return Ok(employeeLeaves);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching employee leaves");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet(ApiEndpoints.EmployeesLeave.Get)]
        public async Task<ActionResult<EmployeeLeaveResponse>> GetEmployeeLeaveById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var employeeLeave = await _employeeLeaveService.GetEmployeeLeaveByIdAsync(id, cancellationToken);

                if (employeeLeave == null)
                {
                    return NotFound();
                }

                return Ok(employeeLeave);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching employee leave with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost(ApiEndpoints.EmployeesLeave.Create)]
        public async Task<ActionResult<Guid>> AddEmployeeLeave([FromBody] CreateEmployeeLeaveRequest employeeLeaveRequest, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for AddEmployeeLeave request.");
                return BadRequest(ModelState);
            }
            try
            {
                var employeeLeaveId = await _employeeLeaveService.AddEmployeeLeaveAsync(employeeLeaveRequest, cancellationToken);
                return Ok(employeeLeaveId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding employee leave");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut(ApiEndpoints.EmployeesLeave.Update)]
        public async Task<ActionResult> UpdateEmployeeLeave(Guid id, [FromBody] UpdateEmployeeLeaveRequest employeeLeaveRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _employeeLeaveService.UpdateEmployeeLeaveAsync(id, employeeLeaveRequest, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating employee leave with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete(ApiEndpoints.EmployeesLeave.Delete)]
        public async Task<ActionResult> DeleteEmployeeLeave(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _employeeLeaveService.DeleteEmployeeLeaveAsync(id, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting employee leave with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
