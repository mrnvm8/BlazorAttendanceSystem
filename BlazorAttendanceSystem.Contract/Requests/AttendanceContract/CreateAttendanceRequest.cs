using BlazorAttendanceSystem.Contract.Requests.EmployeeAttendanceContract;

namespace BlazorAttendanceSystem.Contract.Requests.AttendanceContract
{

    // Create Request
    public class CreateAttendanceRequest
    {
        public Guid DepartmentId { get; set; }
        public DateTime Date { get; set; }
        public List<CreateEmployeeAttendanceRequest> EmployeeAttendances { get; set; } 
            = new List<CreateEmployeeAttendanceRequest>();
    }
}
