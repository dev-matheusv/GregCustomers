using MediatR;

namespace GregCustomers.Application.Clients.Commands.UpdateClientLogo;

public record UpdateClientLogoCommand(
    Guid ClientId,
    byte[] Logo,
    string ContentType,
    string FileName
) : IRequest;