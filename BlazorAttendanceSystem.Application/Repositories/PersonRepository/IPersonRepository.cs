using BlazorAttendanceSystem.Shared;

namespace BlazorAttendanceSystem.Application.Repositories.PersonRepository
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllPeopleAsync(CancellationToken cancellationToken = default);
        Task<Person?> GetPersonByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> AddPersonAsync(Person person, CancellationToken cancellationToken = default);
        Task<bool> UpdatePersonAsync(Person person, CancellationToken cancellationToken = default);
        Task<bool> DeletePersonAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
