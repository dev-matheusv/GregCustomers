namespace GregCustomers.Application.Clients.DTOs;

public record CreateClientRequest(
    string Name,
    string Email
);