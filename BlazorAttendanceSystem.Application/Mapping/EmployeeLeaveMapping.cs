using BlazorAttendanceSystem.Contract.Requests.EmployeeLeaveContract;
using BlazorAttendanceSystem.Contract.Responses;
using BlazorAttendanceSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAttendanceSystem.Application.Mapping
{
    public static class EmployeeLeaveMapping
    {
        public static EmployeeLeave MapToEmployeeLeave(this CreateEmployeeLeaveRequest request)
        {
            return new EmployeeLeave
            {
                Id = Guid.NewGuid(),
                LeaveId = request.LeaveId,
                EmployeeId = request.EmployeeId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = request.IsActive,
                IsApproved = request.IsApproved
            };
        }

        public static EmployeeLeaveResponse MapToResponse(this EmployeeLeave employeeLeave)
        {
            return new EmployeeLeaveResponse
            {
                Id = employeeLeave.Id,
                LeaveId = employeeLeave.LeaveId,
                EmployeeId = employeeLeave.EmployeeId,
                StartDate = employeeLeave.StartDate,
                EndDate = employeeLeave.EndDate,
                IsActive = employeeLeave.IsActive,
                IsApproved = employeeLeave.IsApproved
            };
        }

        public static EmployeeLeave MapToEmployeeLeave(this UpdateEmployeeLeaveRequest request, Guid leaveId)
        {
            return new EmployeeLeave
            {
                Id = leaveId,
                LeaveId = request.LeaveId,
                EmployeeId = request.EmployeeId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = request.IsActive,
                IsApproved = request.IsApproved
            };
        }
    }
}
