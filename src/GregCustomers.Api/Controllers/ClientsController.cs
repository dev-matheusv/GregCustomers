using GregCustomers.Application.Clients.DTOs;
using GregCustomers.Application.Clients.Commands.CreateClient;
using GregCustomers.Application.Clients.Commands.UpdateClient;
using GregCustomers.Application.Clients.Commands.DeleteClient;
using GregCustomers.Application.Clients.Commands.UpdateClientLogo;
using GregCustomers.Application.Clients.Queries.GetClientById;
using GregCustomers.Application.Clients.Queries.GetClients;
using GregCustomers.Application.Clients.Queries.GetClientLogo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GregCustomers.Api.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request, CancellationToken ct)
    {
        var id = await mediator.Send(new CreateClientCommand(request.Name, request.Email), ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var client = await mediator.Send(new GetClientByIdQuery(id), ct);
        return client is null ? NotFound() : Ok(client);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var clients = await mediator.Send(new GetClientsQuery(page, pageSize), ct);
        return Ok(clients);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientRequest request, CancellationToken ct)
    {
        await mediator.Send(new UpdateClientCommand(id, request.Name, request.Email), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteClientCommand(id), ct);
        return NoContent();
    }
    
    [HttpPost("{id:guid}/logo")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadLogo(Guid id, IFormFile? logo, CancellationToken ct)
    {
        if (logo is null || logo.Length == 0)
            return BadRequest("Logo file is required.");

        // opcional: limitar tamanho pra evitar bomba no DB
        const long maxBytes = 2 * 1024 * 1024; // 2MB
        if (logo.Length > maxBytes)
            return BadRequest("Logo file too large (max 2MB).");

        using var ms = new MemoryStream();
        await logo.CopyToAsync(ms, ct);

        await mediator.Send(new UpdateClientLogoCommand(
            id,
            ms.ToArray(),
            logo.ContentType,
            logo.FileName
        ), ct);

        return NoContent();
    }

    [HttpGet("{id:guid}/logo")]
    public async Task<IActionResult> DownloadLogo(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetClientLogoQuery(id), ct);

        if (result is null)
            return NotFound();

        return File(result.Bytes, result.ContentType, result.FileName);
    }
}