namespace BlazorAttendanceSystem.Contract.Responses
{
    // Response
    public class EmployeeAttendanceResponse
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid? EmployeeLeaveId { get; set; }
        public string TimeIn { get; set; } = string.Empty;
        public string TimeOut { get; set; } = string.Empty;
        public bool Present { get; set; }
        public string Reason { get; set; } = string.Empty;
        public double TotalWorkedHours { get; set; }
    }
}
