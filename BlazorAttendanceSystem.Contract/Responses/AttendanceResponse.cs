using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAttendanceSystem.Contract.Responses
{
    // Response
    public class AttendanceResponse
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime Date { get; set; }
        public List<EmployeeAttendanceResponse> EmployeeAttendances { get; set; }
            = new List<EmployeeAttendanceResponse>();
    }
}
