using System.ComponentModel.DataAnnotations;

namespace BlazorAttendanceSystem.Contract.Requests.DepartmentContract
{
    // Create Request
    public class CreateDepartmentRequest
    {
        public Guid OfficeId { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        public string Name { get; set; } = string.Empty;
        public string? Manager { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
