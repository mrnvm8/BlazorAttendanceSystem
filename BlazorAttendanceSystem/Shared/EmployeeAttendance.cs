namespace BlazorAttendanceSystem.Shared
{
    public class EmployeeAttendance
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; } // Foreign key for Employee
        public Guid? EmployeeLeaveId { get; set; } // Foreign key for EmployeeLeave
        public string? TimeIn { get; set; } // Format: "HH:mm", nullable
        public string? TimeOut { get; set; } // Format: "HH:mm", nullable
        public bool Present { get; set; } // Indicates whether the employee is physically present
        public string? Reason { get; set; } // Reason for being present (When Time in and Out are null)
       
        public double TotalWorkedHours { get; set; } // Total worked hours for the day

        // Navigation properties for related entities
        public Employee? Employee { get; set; }
        public EmployeeLeave? EmployeeLeave { get; set; }
    }
}
