using GregCustomers.Application.Abstractions.Persistence;
using MediatR;

namespace GregCustomers.Application.Clients.Queries.GetClientLogo;

public class GetClientLogoQueryHandler(IClientQueries queries) : IRequestHandler<GetClientLogoQuery, ClientLogoResult?>
{
    public async Task<ClientLogoResult?> Handle(GetClientLogoQuery request, CancellationToken ct)
    {
        var client = await queries.GetByIdAsync(request.ClientId, ct);

        if (client is null || client.Logo is null || string.IsNullOrWhiteSpace(client.LogoContentType))
            return null;

        return new ClientLogoResult(
            client.Logo,
            client.LogoContentType!,
            client.LogoFileName ?? "logo"
        );
    }
}