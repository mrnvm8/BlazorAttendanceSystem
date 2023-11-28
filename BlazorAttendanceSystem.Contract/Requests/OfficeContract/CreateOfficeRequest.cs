using System.ComponentModel.DataAnnotations;

namespace BlazorAttendanceSystem.Contract.Requests.OfficeContract
{
    // Create Request
    public class CreateOfficeRequest
    {
        [Required(ErrorMessage = "Office name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;
    }
}
