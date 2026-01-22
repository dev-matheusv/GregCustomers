using MediatR;

namespace GregCustomers.Application.Clients.Commands.UpdateClient;

public record UpdateClientCommand(
    Guid Id,
    string Name,
    string Email
) : IRequest;