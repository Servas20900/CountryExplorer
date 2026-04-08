using ExplorerApp.WebMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExplorerApp.WebMVC.Controllers;

public class CountriesController : Controller
{
    private readonly IExplorerApiClientService explorerApiClientService;

    public CountriesController(IExplorerApiClientService explorerApiClientService)
    {
        this.explorerApiClientService = explorerApiClientService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Search(string query)
    {
        var country = await explorerApiClientService.SearchCountryAsync(query);
        return View(country);
    }

    [HttpGet]
    public async Task<IActionResult> Saved()
    {
        var countries = await explorerApiClientService.GetSavedCountriesAsync();
        return View(countries);
    }

    [HttpGet]
    public async Task<IActionResult> Archived()
    {
        var countries = await explorerApiClientService.GetArchivedCountriesAsync();
        return View(countries);
    }
}