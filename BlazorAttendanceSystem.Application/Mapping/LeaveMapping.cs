using BlazorAttendanceSystem.Contract.Requests.LeaveContract;
using BlazorAttendanceSystem.Contract.Responses;
using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Mapping
{
    public static class LeaveMapping
    {
        public static Leave MapToLeave(this CreateLeaveRequest request)
        {
            return new Leave
            {
                Id = Guid.NewGuid(),
                LeaveType = request.LeaveType,
                MaxDaysAllowed = request.MaxDaysAllowed,
                RemainingLeaveDays = request.RemainingLeaveDays,
                Description = request.Description
            };
        }

        public static LeaveResponse MapToResponse(this Leave leave)
        {
            return new LeaveResponse
            {
                Id = leave.Id,
                LeaveType = leave.LeaveType,
                MaxDaysAllowed = leave.MaxDaysAllowed,
                RemainingLeaveDays = leave.RemainingLeaveDays,
                Description = leave.Description
            };
        }

        public static Leave MapToLeave(this UpdateLeaveRequest request, Guid leaveId)
        {
            return new Leave
            {
                Id = leaveId,
                LeaveType = request.LeaveType,
                MaxDaysAllowed = request.MaxDaysAllowed,
                RemainingLeaveDays = request.RemainingLeaveDays,
                Description = request.Description
            };
        }
    }
}
