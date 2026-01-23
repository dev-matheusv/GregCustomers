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
    public async Task<IActionResult> Create(ClientViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var client = httpFactory.CreateClient("ApiClient");

        var content = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PostAsync("/api/clients", content);
        response.EnsureSuccessStatusCode();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var client = httpFactory.CreateClient("ApiClient");

        var response = await client.GetAsync($"/api/clients/{id}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var model = JsonSerializer.Deserialize<ClientViewModel>(json, _jsonOptions);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, ClientViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var client = httpFactory.CreateClient("ApiClient");

        var content = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            "application/json"
        );

        var response = await client.PutAsync($"/api/clients/{id}", content);
        response.EnsureSuccessStatusCode();

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