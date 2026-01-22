namespace GregCustomers.Application.Clients.DTOs;

public record UpdateClientRequest(
    string Name,
    string Email
);