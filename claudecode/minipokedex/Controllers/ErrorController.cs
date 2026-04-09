using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using minipokedex.Models;

namespace minipokedex.Controllers;

/// <summary>
/// Handles application-level error pages. Mapped as the global exception handler
/// so that unhandled exceptions are redirected here in non-development environments.
/// </summary>
public class ErrorController : Controller
{
    /// <summary>
    /// Renders the generic error page with the current request's trace identifier.
    /// Response is never cached so each error is shown fresh.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index()
    {
        return View("~/Views/Shared/Error.cshtml",
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
