using System.ComponentModel.DataAnnotations;

namespace BlazorAttendanceSystem.Contract.Requests.DepartmentContract
{

    // Update Request
    public class UpdateDepartmentRequest
    {
        public Guid DepartmentId { get; set; }
        public Guid OfficeId { get; set; }
        [Required(ErrorMessage = "Department name is required")]
        public string Name { get; set; } = string.Empty;    
        public string Manager { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
