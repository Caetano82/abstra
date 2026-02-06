using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Queries.States;

public record GetAllStatesQuery() : IRequest<IEnumerable<StateDto>>;
