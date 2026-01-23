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
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return NotFound();

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var model = JsonSerializer.Deserialize<ClientViewModel>(json, _jsonOptions);

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
}