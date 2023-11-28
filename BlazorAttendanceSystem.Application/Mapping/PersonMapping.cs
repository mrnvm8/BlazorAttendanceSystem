using BlazorAttendanceSystem.Contract.Requests.PersonContract;
using BlazorAttendanceSystem.Contract.Responses;
using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Mapping
{
    public static class PersonMapping
    {
        public static Person MapToPerson(this CreatePersonRequest request)
        {
            return new Person
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
            };
        }

        public static PersonResponse MapToResponse(this Person person)
        {
            return new PersonResponse
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
            };
        }

        public static Person MapToPerson(this UpdatePersonRequest request, Guid Id)
        {
            return new Person
            {
                Id = Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
            };
        }
    }
}
