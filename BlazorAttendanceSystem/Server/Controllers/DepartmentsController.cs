using BlazorAttendanceSystem.Application.Services.DepartmentService;
using BlazorAttendanceSystem.Contract.Requests.DepartmentContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAttendanceSystem.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(IDepartmentService departmentService, ILogger<DepartmentsController> logger)
        {
            _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ApiEndpoints.Departments.GetAll)]
        public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetDepartments(CancellationToken cancellationToken = default)
        {
            try
            {
                var departments = await _departmentService.GetAllDepartmentsAsync(cancellationToken);
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching departments");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet(ApiEndpoints.Departments.Get)]
        public async Task<ActionResult<DepartmentResponse>> GetDepartmentById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var department = await _departmentService.GetDepartmentByIdAsync(id, cancellationToken);

                if (department == null)
                {
                    return NotFound();
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching department with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost(ApiEndpoints.Departments.Create)]
        public async Task<ActionResult<Guid>> AddDepartment([FromBody] CreateDepartmentRequest departmentRequest, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for AddDepartment request.");
                return BadRequest(ModelState);
            }
            try
            {
                var departmentId = await _departmentService.AddDepartmentAsync(departmentRequest, cancellationToken);
                return Ok(departmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding department");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut(ApiEndpoints.Departments.Update)]
        public async Task<ActionResult> UpdateDepartment(Guid id, [FromBody] UpdateDepartmentRequest departmentRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _departmentService.UpdateDepartmentAsync(id, departmentRequest, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating department with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete(ApiEndpoints.Departments.Delete)]
        public async Task<ActionResult> DeleteDepartment(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _departmentService.DeleteDepartmentAsync(id, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting department with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
