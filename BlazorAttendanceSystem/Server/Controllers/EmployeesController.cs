using BlazorAttendanceSystem.Application.Services.EmployeeService;
using BlazorAttendanceSystem.Contract.Requests.EmployeeContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAttendanceSystem.Server.Controllers
{
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ApiEndpoints.Employees.GetAll)]
        public async Task<ActionResult<IEnumerable<EmployeeResponse>>> GetEmployees(CancellationToken cancellationToken = default)
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync(cancellationToken);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching employees");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet(ApiEndpoints.Employees.Get)]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id, cancellationToken);

                if (employee == null)
                {
                    return NotFound();
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching employee with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost(ApiEndpoints.Employees.Create)]
        public async Task<ActionResult<Guid>> AddEmployee([FromBody] CreateEmployeeRequest employeeRequest, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for AddEmployee request.");
                return BadRequest(ModelState);
            }
            try
            {
                var employeeId = await _employeeService.AddEmployeeAsync(employeeRequest, cancellationToken);
                return Ok(employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding employee");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut(ApiEndpoints.Employees.Update)]
        public async Task<ActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeRequest employeeRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _employeeService.UpdateEmployeeAsync(id, employeeRequest, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating employee with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete(ApiEndpoints.Employees.Delete)]
        public async Task<ActionResult> DeleteEmployee(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(id, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting employee with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
