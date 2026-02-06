using Abstra.Domain.DTOs;
using MediatR;

namespace Abstra.Domain.Queries.States;

public record GetStateByIdQuery(int Id) : IRequest<StateDto?>;
