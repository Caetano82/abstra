using Abstra.Domain.Commands.Countries;
using Abstra.Domain.DTOs;
using Abstra.Domain.Queries.Countries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abstra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CountriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CountriesController> _logger;

    public CountriesController(IMediator mediator, ILogger<CountriesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CountryDto>>> GetAll()
    {
        var query = new GetAllCountriesQuery();
        var countries = await _mediator.Send(query);
        return Ok(countries);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetById(int id)
    {
        var query = new GetCountryByIdQuery(id);
        var country = await _mediator.Send(query);
        if (country == null)
        {
            return NotFound();
        }
        return Ok(country);
    }

    [HttpPost]
    public async Task<ActionResult<CountryDto>> Create([FromBody] CreateCountryRequest request)
    {
        try
        {
            var command = new CreateCountryCommand(request);
            var country = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = country.Id }, country);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CountryDto>> Update(int id, [FromBody] UpdateCountryRequest request)
    {
        try
        {
            var command = new UpdateCountryCommand(id, request);
            var country = await _mediator.Send(command);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteCountryCommand(id);
        var deleted = await _mediator.Send(command);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
