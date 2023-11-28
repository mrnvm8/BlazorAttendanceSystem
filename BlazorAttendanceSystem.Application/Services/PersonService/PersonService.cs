using BlazorAttendanceSystem.Application.Mapping;
using BlazorAttendanceSystem.Application.Repositories.PersonRepository;
using BlazorAttendanceSystem.Contract.Requests.PersonContract;
using BlazorAttendanceSystem.Contract.Responses;
using BlazorAttendanceSystem.Shared;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BlazorAttendanceSystem.Application.Services.PersonService
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonService> _logger;

        public PersonService(IPersonRepository personRepository, ILogger<PersonService> logger)
        {
            _personRepository = personRepository ?? 
                throw new ArgumentNullException(nameof(personRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<PersonResponse>> GetAllPeopleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var people = await _personRepository.GetAllPeopleAsync(cancellationToken);
                var peopleResponses = people.Select(PersonMapping.MapToResponse);
                _logger.LogInformation("Retrieved {Count} offices from the database", peopleResponses.Count());

                return peopleResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all people");
                throw; // Re-throw the exception for higher-level handling
            }
        }

        public async Task<PersonResponse?> GetPersonByIdAsync(Guid personId, CancellationToken cancellationToken = default)
        {
            try
            {
                var person = await _personRepository.GetPersonByIdAsync(personId, cancellationToken);
                if (person is null)
                {
                    _logger.LogInformation("Person with ID {Id} not found", personId);
                    return null;
                }
                var personResponse = PersonMapping.MapToResponse(person);

                _logger.LogInformation("Retrieved office with ID {Id} from the database", personId);
                return personResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving person by ID: {PersonId}", personId);
                throw; // Re-throw the exception for higher-level handling
            }
        }

        public async Task<Guid> CreatePersonAsync(CreatePersonRequest createRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var personEntity = PersonMapping.MapToPerson(createRequest);
                var personId = await _personRepository.AddPersonAsync(personEntity, cancellationToken);

                _logger.LogInformation("Added a new office with ID {Id} to the database", personId);
                return personId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a new person");
                throw; // Re-throw the exception for higher-level handling
            }
        }

        public async Task<bool> UpdatePersonAsync(Guid personId, UpdatePersonRequest updateRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingPerson = await _personRepository.GetPersonByIdAsync (personId, cancellationToken);

                if (existingPerson == null)
                {
                    _logger.LogInformation("Person with ID {Id} not found", personId);
                    return false;
                }

                var personUpdate = updateRequest.MapToPerson(personId);
                var result = await _personRepository.UpdatePersonAsync(personUpdate, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Updated person with ID {Id} in the database", personId);
                }
                else
                {
                    _logger.LogWarning("Failed to update person with ID {Id} in the database", personId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating person with ID: {PersonId}", updateRequest.PersonId);
                throw; // Re-throw the exception for higher-level handling
            }
        }

        public async Task<bool> DeletePersonAsync(Guid personId, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingPerson = await _personRepository.GetPersonByIdAsync(personId);

                if (existingPerson == null)
                {
                    _logger.LogInformation("Person with ID {Id} not found", personId);
                    return false;
                }

                var result = await _personRepository.DeletePersonAsync(personId, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Deleted person with ID {Id} from the database", personId);
                }
                else
                {
                    _logger.LogWarning("Failed to delete person with ID {Id} from the database", personId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting person with ID: {PersonId}", personId);
                throw; // Re-throw the exception for higher-level handling
            }
        }
    }
}
