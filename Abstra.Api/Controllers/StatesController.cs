using Abstra.Domain.Commands.States;
using Abstra.Domain.DTOs;
using Abstra.Domain.Queries.States;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abstra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StatesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StatesController> _logger;

    public StatesController(IMediator mediator, ILogger<StatesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StateDto>>> GetAll()
    {
        var query = new GetAllStatesQuery();
        var states = await _mediator.Send(query);
        return Ok(states);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StateDto>> GetById(int id)
    {
        var query = new GetStateByIdQuery(id);
        var state = await _mediator.Send(query);
        if (state == null)
        {
            return NotFound();
        }
        return Ok(state);
    }

    [HttpGet("country/{countryId}")]
    public async Task<ActionResult<IEnumerable<StateDto>>> GetByCountryId(int countryId)
    {
        var query = new GetStatesByCountryIdQuery(countryId);
        var states = await _mediator.Send(query);
        return Ok(states);
    }

    [HttpPost]
    public async Task<ActionResult<StateDto>> Create([FromBody] CreateStateRequest request)
    {
        try
        {
            var command = new CreateStateCommand(request);
            var state = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = state.Id }, state);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StateDto>> Update(int id, [FromBody] UpdateStateRequest request)
    {
        try
        {
            var command = new UpdateStateCommand(id, request);
            var state = await _mediator.Send(command);
            if (state == null)
            {
                return NotFound();
            }
            return Ok(state);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteStateCommand(id);
        var deleted = await _mediator.Send(command);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
