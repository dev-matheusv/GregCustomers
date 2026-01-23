using System.Text;
using System.Text.Json;
using GregCustomers.WebMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace GregCustomers.WebMvc.Controllers;

public class ClientsController(IHttpClientFactory httpFactory) : Controller
{
    private readonly JsonSerializerOptions _jsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public async Task<IActionResult> Index()
    {
        var client = httpFactory.CreateClient("ApiClient");

        var response = await client.GetAsync("/api/clients");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var clients = JsonSerializer.Deserialize<List<ClientViewModel>>(json, _jsonOptions);

        return View(clients);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ClientViewModel model, IFormFile? logo)
    {
        if (!ModelState.IsValid)
            return View(model);

        var client = httpFactory.CreateClient("ApiClient");

        // 1️⃣ cria cliente
        var content = new StringContent(
            JsonSerializer.Serialize(new { model.Name, model.Email }),
            Encoding.UTF8,
            "application/json"
        );
        var response = await client.PostAsync("/api/clients", content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return View(model);
        }

        // 2️⃣ se tiver logo, faz upload
        if (logo is null || logo.Length <= 0) return RedirectToAction(nameof(Index));
        var location = response.Headers.Location?.ToString();
        var clientId = location?.Split('/').Last();

        using var form = new MultipartFormDataContent();
        form.Add(new StreamContent(logo.OpenReadStream()), "logo", logo.FileName);

        var uploadResponse = await client.PostAsync($"/api/clients/{clientId}/logo", form);
        uploadResponse.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var client = httpFactory.CreateClient("ApiClient");

        var response = await client.GetAsync($"/api/clients/{id}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var model = JsonSerializer.Deserialize<ClientViewModel>(json, _jsonOptions);

        if (model is null)
            return NotFound();

        // carrega addresses do cliente
        var addrResponse = await client.GetAsync($"/api/clients/{id}/addresses");
        if (addrResponse.IsSuccessStatusCode)
        {
            var addrJson = await addrResponse.Content.ReadAsStringAsync();
            var addresses = JsonSerializer.Deserialize<List<AddressViewModel>>(addrJson, _jsonOptions) ?? new();
            model.Addresses = addresses;
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, ClientViewModel model, IFormFile? logo)
    {
        if (!ModelState.IsValid)
            return View(model);

        var client = httpFactory.CreateClient("ApiClient");

        // update dados
        var content = new StringContent(
            JsonSerializer.Serialize(new { model.Name, model.Email }),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PutAsync($"/api/clients/{id}", content);
        response.EnsureSuccessStatusCode();

        // upload logo se enviado
        if (logo is null || logo.Length <= 0) return RedirectToAction(nameof(Index));
        using var form = new MultipartFormDataContent();
        form.Add(new StreamContent(logo.OpenReadStream()), "logo", logo.FileName); 

        var uploadResponse = await client.PostAsync($"/api/clients/{id}/logo", form);

        uploadResponse.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Delete(Guid id)
    {
        var client = httpFactory.CreateClient("ApiClient");

        var response = await client.DeleteAsync($"/api/clients/{id}");
        response.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAddress(Guid id, string street)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            TempData["Error"] = "Informe o logradouro.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        var api = httpFactory.CreateClient("ApiClient");

        var content = new StringContent(
            JsonSerializer.Serialize(new { street }),
            Encoding.UTF8,
            "application/json"
        );

        var response = await api.PostAsync($"/api/clients/{id}/addresses", content);

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Não foi possível adicionar o endereço.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        TempData["Success"] = "Endereço adicionado.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAddress(Guid clientId, Guid addressId, string street)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            TempData["Error"] = "Informe o logradouro.";
            return RedirectToAction(nameof(Edit), new { id = clientId });
        }

        var client = httpFactory.CreateClient("ApiClient");

        var content = new StringContent(
            JsonSerializer.Serialize(new { street }),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PutAsync($"/api/addresses/{addressId}", content);
        if (!response.IsSuccessStatusCode)
    {
        TempData["Error"] = "Não foi possível atualizar o endereço.";
        return RedirectToAction(nameof(Edit), new { clientId });
    }

    TempData["Success"] = "Endereço atualizado.";
        return RedirectToAction(nameof(Edit), new { id = clientId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAddress(Guid clientId, Guid addressId)
    {
        var client = httpFactory.CreateClient("ApiClient");

        var response = await client.DeleteAsync($"/api/addresses/{addressId}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Não foi possível remover o endereço.";
            return RedirectToAction(nameof(Edit), new { clientId });
        }

        TempData["Success"] = "Endereço removido.";
        return RedirectToAction(nameof(Edit), new { id = clientId });
    }
}