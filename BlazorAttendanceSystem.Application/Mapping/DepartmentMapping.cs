using BlazorAttendanceSystem.Contract.Requests.DepartmentContract;
using BlazorAttendanceSystem.Contract.Responses;
using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Mapping
{
    public static class DepartmentMapping
    {
        public static Department MapToDepartment(this CreateDepartmentRequest request)
        {
            return new Department
            {
                Id = Guid.NewGuid(),
                OfficeId = request.OfficeId,
                Name = request.Name,
                Manager = request.Manager,
                Description = request.Description
            };
        }

        public static DepartmentResponse MapToResponse(this Department department)
        {
            return new DepartmentResponse
            {
                Id = department.Id,
                OfficeId = department.OfficeId,
                Name = department.Name,
                Manager = department.Manager,
                Description = department.Description
            };
        }

        public static Department MapToDepartment(this UpdateDepartmentRequest request, Guid departmentId)
        {
            return new Department
            {
                Id = departmentId,
                OfficeId = request.OfficeId,
                Name = request.Name,
                Manager = request.Manager,
                Description = request.Description
            };
        }
    }
}
