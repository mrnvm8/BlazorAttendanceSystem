namespace BlazorAttendanceSystem.Shared
{
    public class Employee
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; } = Guid.Empty; // Foreign key for Person
        public Guid DepartmentId { get; set; } = Guid.Empty; // Foreign key for Department
        public string WorkEmail { get; set; } = string.Empty;  // Add the work email field

        // Navigation properties for related entities
        // ? Reference to the nullable model
        public Person? Person { get; set; }
        public Department? Department { get; set; }
    }
}
