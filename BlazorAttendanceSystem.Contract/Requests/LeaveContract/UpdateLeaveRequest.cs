using System.ComponentModel.DataAnnotations;

namespace BlazorAttendanceSystem.Contract.Requests.LeaveContract
{
    // Update Request
    public class UpdateLeaveRequest
    {
        public Guid LeaveId { get; set; }
        [Required(ErrorMessage = "Leave type is required")]
        public string LeaveType { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Max days allowed must be greater than 0")]
        public int MaxDaysAllowed { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Remaining leave days must be 0 or greater")]
        public int RemainingLeaveDays { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
