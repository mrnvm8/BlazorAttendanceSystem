namespace BlazorAttendanceSystem.Shared
{
    public class EmployeeLeave
    {
        public Guid Id { get; set; }
        public Guid LeaveId { get; set; } // Foreign key for Leave
        public Guid EmployeeId { get; set; } // Foreign key for Employee
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; } = false;

        // Navigation properties for related entities
        // ? Reference to the nullable model
        public Leave? Leave { get; set; }
        public Employee? Employee { get; set; }
    }
}
