namespace BlazorAttendanceSystem.Shared
{
    public class Department
    {
        public Guid Id { get; set; }
        public Guid OfficeId { get; set; } = Guid.Empty; // Foreign key for Office
        public string Name { get; set; } = string.Empty;
        public string? Manager { get; set; }
        public string Description { get; set; } = string.Empty;
        public Office? DepartmentOffice { get; set; } // Reference to the nullable Office model
      
    }
}
