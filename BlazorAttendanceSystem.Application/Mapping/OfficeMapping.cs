using BlazorAttendanceSystem.Contract.Requests.OfficeContract;
using BlazorAttendanceSystem.Contract.Responses;
using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Mapping
{
    public static class OfficeMapping
    {
        public static Office MapToOffice(this CreateOfficeRequest request)
        {
            return new Office
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Location = request.Location,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            };
        }

        public static OfficeResponse MapToResponse(this Office office)
        {
            return new OfficeResponse
            {
                Id = office.Id,
                Name = office.Name,
                Location = office.Location,
                PhoneNumber = office.PhoneNumber,
                Email = office.Email
            };
        }

        public static Office MapToOffice(this UpdateOfficeRequest request, Guid officeId)
        {
            return new Office
            {
                Id = officeId,
                Name = request.Name,
                Location = request.Location,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            };
        }
    }
}
