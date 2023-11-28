using System.ComponentModel.DataAnnotations;

namespace BlazorAttendanceSystem.Contract.Requests.EmployeeLeaveContract
{
    // Create Request
    public class CreateEmployeeLeaveRequest
    {
        public Guid LeaveId { get; set; }
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
    }
}
