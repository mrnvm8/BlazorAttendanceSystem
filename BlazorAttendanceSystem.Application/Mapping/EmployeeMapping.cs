using BlazorAttendanceSystem.Contract.Requests.EmployeeContract;
using BlazorAttendanceSystem.Contract.Responses;
using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Mapping
{
    public static class EmployeeMapping
    {
        public static Employee MapToEmployee(this CreateEmployeeRequest request)
        {
            return new Employee
            {
                Id = Guid.NewGuid(),
                PersonId = request.PersonId,
                DepartmentId = request.DepartmentId,
                WorkEmail = request.WorkEmail
            };
        }

        public static EmployeeResponse MapToResponse(this Employee employee)
        {
            return new EmployeeResponse
            {
                Id = employee.Id,
                PersonId = employee.PersonId,
                DepartmentId = employee.DepartmentId,
                WorkEmail = employee.WorkEmail
            };
        }

        public static Employee MapToEmployee(this UpdateEmployeeRequest request, Guid employeeId)
        {
            return new Employee
            {
                Id = employeeId,
                PersonId = request.PersonId,
                DepartmentId = request.DepartmentId,
                WorkEmail = request.WorkEmail
            };
        }
    }
}
