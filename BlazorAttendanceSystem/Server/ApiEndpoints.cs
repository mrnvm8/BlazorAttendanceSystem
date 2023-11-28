namespace BlazorAttendanceSystem.Server
{
    public static class ApiEndpoints
    {
        private const string ApiBase = "api";

        //People/Person
        public static class People
        {
            private const string Base = $"{ApiBase}/people";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";  //{{id:guid}}
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }

        //Attendances
        public static class Attendances
        {
            private const string Base = $"{ApiBase}/attendances";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";  
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }

        //offices
        public static class Offices
        {
            private const string Base = $"{ApiBase}/offices";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }

        //Departments
        public static class Departments
        {
            private const string Base = $"{ApiBase}/departments";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }

        //Leaves
        public static class Leaves
        {
            private const string Base = $"{ApiBase}/leaves";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }

        //Employees
        public static class Employees
        {
            private const string Base = $"{ApiBase}/employees";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }

        //EmployeeLeave
        public static class EmployeesLeave
        {
            private const string Base = $"{ApiBase}/employeesleave";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }

        //Employee Attendance
        public static class EmployeesAttendance
        {
            private const string Base = $"{ApiBase}/employeesattendance";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
    }
}
