using MediatR;

namespace GregCustomers.Application.Clients.Queries.GetClientLogo;

public record GetClientLogoQuery(Guid ClientId) : IRequest<ClientLogoResult?>;

public record ClientLogoResult(
    byte[] Bytes,
    string ContentType,
    string FileName
);