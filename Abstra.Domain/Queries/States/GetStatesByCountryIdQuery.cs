using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Queries.States;

public record GetStatesByCountryIdQuery(int CountryId) : IRequest<IEnumerable<StateDto>>;
