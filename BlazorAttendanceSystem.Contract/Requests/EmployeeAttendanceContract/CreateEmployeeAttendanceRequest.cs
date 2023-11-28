using System.ComponentModel.DataAnnotations;

namespace BlazorAttendanceSystem.Contract.Requests.EmployeeAttendanceContract
{
    // Create Request
    public class CreateEmployeeAttendanceRequest
    {
        [Required(ErrorMessage = "Employee ID is required")]
        public Guid EmployeeId { get; set; }
        public Guid? EmployeeLeaveId { get; set; }
        public string TimeIn { get; set; } = string.Empty;
        public string TimeOut { get; set; } = string.Empty;

        [Required(ErrorMessage = "Present status is required")]
        public bool Present { get; set; }
        public string Reason { get; set; } = string.Empty;
        public double TotalWorkedHours { get; set; }
    }
}
