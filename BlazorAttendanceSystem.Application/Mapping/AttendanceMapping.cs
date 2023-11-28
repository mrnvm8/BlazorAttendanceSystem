using BlazorAttendanceSystem.Contract.Requests.AttendanceContract;
using BlazorAttendanceSystem.Contract.Responses;
using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Mapping
{
    public static class AttendanceMapping
    {
        public static Attendance MapToAttendance(this CreateAttendanceRequest request)
        {
            return new Attendance
            {
                Id = Guid.NewGuid(),
                DepartmentId = request.DepartmentId,
                Date = request.Date,
                EmployeeAttendances = request.EmployeeAttendances.Select(e => e.MapToEmployeeAttendance()).ToList()
            };
        }

        public static AttendanceResponse MapToResponse(this Attendance attendance)
        {
            return new AttendanceResponse
            {
                Id = attendance.Id,
                DepartmentId = attendance.DepartmentId,
                Date = attendance.Date,
                EmployeeAttendances = attendance.EmployeeAttendances.Select(e => e.MapToResponse()).ToList()
            };
        }

        public static Attendance MapToAttendance(this UpdateAttendanceRequest request, Guid attendanceId)
        {
            return new Attendance
            {
                Id = attendanceId,
                DepartmentId = request.DepartmentId,
                Date = request.Date,
                EmployeeAttendances = request.EmployeeAttendances!
                .Select(e => e?.MapToEmployeeAttendance(attendanceId)!).ToList()
            };
        }
    }
}
