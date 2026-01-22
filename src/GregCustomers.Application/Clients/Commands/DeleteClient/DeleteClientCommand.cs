using MediatR;

namespace GregCustomers.Application.Clients.Commands.DeleteClient;

public record DeleteClientCommand(Guid Id) : IRequest;