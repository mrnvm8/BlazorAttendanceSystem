using BlazorAttendanceSystem.Contract.Requests.EmployeeAttendanceContract;
using BlazorAttendanceSystem.Contract.Responses;
using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Mapping
{
    public static class EmployeeAttendanceMapping
    {
        public static EmployeeAttendance MapToEmployeeAttendance(this CreateEmployeeAttendanceRequest request)
        {
            return new EmployeeAttendance
            {
                Id = Guid.NewGuid(),
                EmployeeId = request.EmployeeId,
                EmployeeLeaveId = request.EmployeeLeaveId,
                TimeIn = request.TimeIn,
                TimeOut = request.TimeOut,
                Present = request.Present,
                Reason = request.Reason,
                TotalWorkedHours = request.TotalWorkedHours
            };
        }

        public static EmployeeAttendanceResponse MapToResponse(this EmployeeAttendance employeeAttendance)
        {
            return new EmployeeAttendanceResponse
            {
                Id = employeeAttendance.Id,
                EmployeeId = employeeAttendance.EmployeeId,
                EmployeeLeaveId = employeeAttendance.EmployeeLeaveId,
                TimeIn = employeeAttendance.TimeIn!,
                TimeOut = employeeAttendance.TimeOut!,
                Present = employeeAttendance.Present,
                Reason = employeeAttendance.Reason!,
                TotalWorkedHours = employeeAttendance.TotalWorkedHours
            };
        }

        public static EmployeeAttendance MapToEmployeeAttendance(this UpdateEmployeeAttendanceRequest request, Guid attendanceId)
        {
            return new EmployeeAttendance
            {
                Id = attendanceId,
                EmployeeId = request.EmployeeId,
                EmployeeLeaveId = request.EmployeeLeaveId,
                TimeIn = request.TimeIn,
                TimeOut = request.TimeOut,
                Present = request.Present,
                Reason = request.Reason,
                TotalWorkedHours = request.TotalWorkedHours
            };
        }
    }
}
