using GregCustomers.Application.Addresses.DTOs;
using GregCustomers.Application.Clients.Addresses.Commands.CreateAddress;
using GregCustomers.Application.Clients.Addresses.Commands.DeleteAddress;
using GregCustomers.Application.Clients.Addresses.Commands.UpdateAddress;
using GregCustomers.Application.Clients.Addresses.Queries.GetAddressById;
using GregCustomers.Application.Clients.Addresses.Queries.GetAddressesByClientId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GregCustomers.Api.Controllers;

[ApiController]
[Route("api")]
public class AddressesController(IMediator mediator) : ControllerBase
{
    // GET /api/clients/{clientId}/addresses
    [HttpGet("clients/{clientId:guid}/addresses")]
    public async Task<IActionResult> GetByClientId(Guid clientId)
    {
        var addresses = await mediator.Send(new GetAddressesByClientIdQuery(clientId));
        return Ok(addresses);
    }

    // GET /api/addresses/{id}
    [HttpGet("addresses/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var address = await mediator.Send(new GetAddressByIdQuery(id));
        return address is null ? NotFound() : Ok(address);
    }

    // POST /api/clients/{clientId}/addresses
    [Authorize(Roles="admin")]
    [HttpPost("clients/{clientId:guid}/addresses")]
    public async Task<IActionResult> Create(Guid clientId, [FromBody] CreateAddressRequest request)
    {
        var id = await mediator.Send(new CreateAddressCommand(clientId, request.Street));
        return Created($"/api/addresses/{id}", new { id });
    }

    // PUT /api/addresses/{id}
    [Authorize(Roles="admin")]
    [HttpPut("addresses/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAddressRequest request)
    {
        await mediator.Send(new UpdateAddressCommand(id, request.Street));
        return NoContent();
    }

    // DELETE /api/addresses/{id}
    [Authorize(Roles="admin")]
    [HttpDelete("addresses/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await mediator.Send(new DeleteAddressCommand(id));
        return NoContent();
    }
}
