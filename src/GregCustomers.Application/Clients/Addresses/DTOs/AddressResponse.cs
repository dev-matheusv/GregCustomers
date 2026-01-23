namespace GregCustomers.Application.Addresses.DTOs;

public record AddressResponse(
    Guid Id,
    Guid ClientId,
    string Street
);
