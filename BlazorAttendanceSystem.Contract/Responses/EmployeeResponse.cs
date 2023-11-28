namespace BlazorAttendanceSystem.Contract.Responses
{
    // Response
    public class EmployeeResponse
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public Guid DepartmentId { get; set; }
        public string WorkEmail { get; set; } = string.Empty;
    }
}
