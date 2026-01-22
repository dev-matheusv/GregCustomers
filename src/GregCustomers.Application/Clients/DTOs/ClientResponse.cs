namespace GregCustomers.Application.Clients.DTOs;

public record ClientResponse(
    Guid Id,
    string Name,
    string Email,
    IReadOnlyList<AddressResponse> Addresses
);

public record AddressResponse(
    Guid Id,
    string Street
);