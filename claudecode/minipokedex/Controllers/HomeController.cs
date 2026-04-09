using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using minipokedex.Models;

namespace minipokedex.Controllers;

/// <summary>
/// Handles general-purpose pages: home, privacy, and error.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Renders the application home page.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Renders the privacy policy page.
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Renders the error page. Response is never cached so each error is shown fresh.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
