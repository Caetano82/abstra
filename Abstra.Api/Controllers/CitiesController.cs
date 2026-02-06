using Abstra.Domain.Commands.Cities;
using Abstra.Domain.DTOs;
using Abstra.Domain.Queries.Cities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abstra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CitiesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CitiesController> _logger;

    public CitiesController(IMediator mediator, ILogger<CitiesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetAll()
    {
        var query = new GetAllCitiesQuery();
        var cities = await _mediator.Send(query);
        return Ok(cities);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CityDto>> GetById(int id)
    {
        var query = new GetCityByIdQuery(id);
        var city = await _mediator.Send(query);
        if (city == null)
        {
            return NotFound();
        }
        return Ok(city);
    }

    [HttpGet("country/{countryId}")]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetByCountryId(int countryId)
    {
        // Note: This endpoint is kept for backward compatibility but cities are now filtered by state
        // You may want to remove this endpoint or implement it differently
        var query = new GetAllCitiesQuery();
        var allCities = await _mediator.Send(query);
        var cities = allCities.Where(c => c.CountryId == countryId);
        return Ok(cities);
    }

    [HttpGet("state/{stateId}")]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetByStateId(int stateId)
    {
        var query = new GetCitiesByStateIdQuery(stateId);
        var cities = await _mediator.Send(query);
        return Ok(cities);
    }

    [HttpPost]
    public async Task<ActionResult<CityDto>> Create([FromBody] CreateCityRequest request)
    {
        try
        {
            var command = new CreateCityCommand(request);
            var city = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = city.Id }, city);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CityDto>> Update(int id, [FromBody] UpdateCityRequest request)
    {
        try
        {
            var command = new UpdateCityCommand(id, request);
            var city = await _mediator.Send(command);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteCityCommand(id);
        var deleted = await _mediator.Send(command);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
