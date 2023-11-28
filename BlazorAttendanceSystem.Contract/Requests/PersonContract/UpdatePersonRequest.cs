using System.ComponentModel.DataAnnotations;

namespace BlazorAttendanceSystem.Contract.Requests.PersonContract
{
    public class UpdatePersonRequest
    {
        public Guid PersonId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Date, ErrorMessage = "Invalid date of birth")]
        public DateTime DateOfBirth { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;
    }
}
