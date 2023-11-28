using BlazorAttendanceSystem.Contract.Requests.EmployeeAttendanceContract;

namespace BlazorAttendanceSystem.Contract.Requests.AttendanceContract
{
    // Update Request
    public class UpdateAttendanceRequest
    {
        public Guid AttendanceId { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime Date { get; set; }
        public List<UpdateEmployeeAttendanceRequest> EmployeeAttendances { get; set; }
            = new List<UpdateEmployeeAttendanceRequest>();
    }
}
