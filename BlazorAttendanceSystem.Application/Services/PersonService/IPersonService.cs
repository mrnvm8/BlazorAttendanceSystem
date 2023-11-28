using BlazorAttendanceSystem.Contract.Requests.PersonContract;
using BlazorAttendanceSystem.Contract.Responses;

namespace BlazorAttendanceSystem.Application.Services.PersonService
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonResponse>> GetAllPeopleAsync(CancellationToken cancellationToken = default);
        Task<PersonResponse?> GetPersonByIdAsync(Guid personId, CancellationToken cancellationToken = default);
        Task<Guid> CreatePersonAsync(CreatePersonRequest createRequest, CancellationToken cancellationToken = default);
        Task<bool> UpdatePersonAsync(Guid personId, UpdatePersonRequest updateRequest, CancellationToken cancellationToken = default);
        Task<bool> DeletePersonAsync(Guid personId, CancellationToken cancellationToken = default);
    }
}
