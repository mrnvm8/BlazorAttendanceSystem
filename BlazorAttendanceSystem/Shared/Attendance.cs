namespace BlazorAttendanceSystem.Shared
{
    public class Attendance
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; } // Foreign key for Department
        public DateTime Date { get; set; }
        // Attendance records for each employee in the department
        public List<EmployeeAttendance> EmployeeAttendances { get; set; } = new List<EmployeeAttendance>();

        // Navigation property for related entities
        public Department? Department { get; set; }
    }
}
