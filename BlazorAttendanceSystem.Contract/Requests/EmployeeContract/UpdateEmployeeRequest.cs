using System.ComponentModel.DataAnnotations;

namespace BlazorAttendanceSystem.Contract.Requests.EmployeeContract
{
    // Update Request
    public class UpdateEmployeeRequest
    {
        public Guid EmployeeId { get; set; }
        public Guid PersonId { get; set; }
        public Guid DepartmentId { get; set; }
        [Required(ErrorMessage = "Work email is required")]
        [EmailAddress(ErrorMessage = "Invalid work email address")]
        public string WorkEmail { get; set; } = string.Empty;
    }
}
