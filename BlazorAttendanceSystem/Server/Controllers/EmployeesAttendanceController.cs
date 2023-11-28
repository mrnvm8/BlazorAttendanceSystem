using BlazorAttendanceSystem.Application.Services.EmployeeAttendanceService;
using BlazorAttendanceSystem.Contract.Requests.EmployeeAttendanceContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAttendanceSystem.Server.Controllers
{
    [ApiController]
    public class EmployeesAttendanceController : ControllerBase
    {
        private readonly IEmployeeAttendanceService _employeeAttendanceService;
        private readonly ILogger<EmployeesAttendanceController> _logger;

        public EmployeesAttendanceController(IEmployeeAttendanceService employeeAttendanceService, ILogger<EmployeesAttendanceController> logger)
        {
            _employeeAttendanceService = employeeAttendanceService ??
                throw new ArgumentNullException(nameof(employeeAttendanceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ApiEndpoints.EmployeesAttendance.GetAll)]
        public async Task<ActionResult<IEnumerable<EmployeeAttendanceResponse>>> GetEmployeeAttendances(CancellationToken cancellationToken = default)
        {
            try
            {
                var employeeAttendances = await _employeeAttendanceService.GetAllEmployeeAttendancesAsync(cancellationToken);
                return Ok(employeeAttendances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching employee attendances");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet(ApiEndpoints.EmployeesAttendance.Get)]
        public async Task<ActionResult<EmployeeAttendanceResponse>> GetEmployeeAttendanceById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var employeeAttendance = await _employeeAttendanceService.GetEmployeeAttendanceByIdAsync(id, cancellationToken);

                if (employeeAttendance == null)
                {
                    return NotFound();
                }

                return Ok(employeeAttendance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching employee attendance with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost(ApiEndpoints.EmployeesAttendance.Create)]
        public async Task<ActionResult<Guid>> AddEmployeeAttendance([FromBody] CreateEmployeeAttendanceRequest employeeAttendanceRequest, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for AddEmployeeAttendance request.");
                return BadRequest(ModelState);
            }
            try
            {
                var employeeAttendanceId = await _employeeAttendanceService.AddEmployeeAttendanceAsync(employeeAttendanceRequest, cancellationToken);
                return Ok(employeeAttendanceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding employee attendance");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut(ApiEndpoints.EmployeesAttendance.Update)]
        public async Task<ActionResult> UpdateEmployeeAttendance(Guid id, [FromBody] UpdateEmployeeAttendanceRequest employeeAttendanceRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _employeeAttendanceService.UpdateEmployeeAttendanceAsync(id, employeeAttendanceRequest, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating employee attendance with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete(ApiEndpoints.EmployeesAttendance.Delete)]
        public async Task<ActionResult> DeleteEmployeeAttendance(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _employeeAttendanceService.DeleteEmployeeAttendanceAsync(id, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting employee attendance with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
