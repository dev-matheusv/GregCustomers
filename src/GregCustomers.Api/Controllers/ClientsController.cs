using GregCustomers.Application.Clients.DTOs;
using GregCustomers.Application.Clients.Commands.CreateClient;
using GregCustomers.Application.Clients.Commands.UpdateClient;
using GregCustomers.Application.Clients.Commands.DeleteClient;
using GregCustomers.Application.Clients.Queries.GetClientById;
using GregCustomers.Application.Clients.Queries.GetClients;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GregCustomers.Api.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request, CancellationToken ct)
    {
        var id = await _mediator.Send(new CreateClientCommand(request.Name, request.Email), ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var client = await _mediator.Send(new GetClientByIdQuery(id), ct);
        return client is null ? NotFound() : Ok(client);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var clients = await _mediator.Send(new GetClientsQuery(page, pageSize), ct);
        return Ok(clients);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientRequest request, CancellationToken ct)
    {
        await _mediator.Send(new UpdateClientCommand(id, request.Name, request.Email), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteClientCommand(id), ct);
        return NoContent();
    }
}