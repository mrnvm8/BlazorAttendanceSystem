namespace BlazorAttendanceSystem.Contract.Responses
{
    // Response
    public class EmployeeLeaveResponse
    {
        public Guid Id { get; set; }
        public Guid LeaveId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
    }
}
