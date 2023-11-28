namespace BlazorAttendanceSystem.Contract.Responses
{
    // Response
    public class DepartmentResponse
    {
        public Guid Id { get; set; }
        public Guid OfficeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Manager { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
