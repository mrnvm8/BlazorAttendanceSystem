using BlazorAttendanceSystem.Application.Services.PersonService;
using BlazorAttendanceSystem.Contract.Requests.PersonContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAttendanceSystem.Server.Controllers
{
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(IPersonService personService, ILogger<PeopleController> logger)
        {
            _personService = personService
                ?? throw new ArgumentNullException(nameof(personService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ApiEndpoints.People.GetAll)]
        public async Task<ActionResult<IEnumerable<PersonResponse>>> GetPeople(CancellationToken cancellationToken = default)
        {
            try
            {
                var people = await _personService.GetAllPeopleAsync(cancellationToken);
                return Ok(people);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching people");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet(ApiEndpoints.People.Get)]
        public async Task<ActionResult<PersonResponse>> GetPersonById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var person = await _personService.GetPersonByIdAsync(id, cancellationToken);

                if (person == null)
                {
                    return NotFound();
                }

                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching person with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost(ApiEndpoints.People.Create)]
        public async Task<ActionResult<Guid>> AddPerson([FromBody] CreatePersonRequest personRequest, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for AddPerson request.");
                return BadRequest(ModelState);
            }

            try
            {
                var personId = await _personService.CreatePersonAsync(personRequest, cancellationToken);
                return Ok(personId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding person");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut(ApiEndpoints.People.Update)]
        public async Task<ActionResult> UpdatePerson(Guid id, [FromBody] UpdatePersonRequest personRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _personService.UpdatePersonAsync(id, personRequest, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating person with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete(ApiEndpoints.People.Delete)]
        public async Task<ActionResult> DeletePerson(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _personService.DeletePersonAsync(id, cancellationToken);

                if (result)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting person with ID {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
