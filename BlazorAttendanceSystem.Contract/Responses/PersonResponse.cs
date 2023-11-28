namespace BlazorAttendanceSystem.Contract.Responses
{
    public class PersonResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName {
            get {
                return $"{FirstName}, {LastName}";
            }
        }
    }
}
