namespace minipokedex.Models;

/// <summary>
/// View model for the generic error page.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// The current Activity or HTTP trace identifier, used for correlating error logs.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Indicates whether <see cref="RequestId"/> has a value and should be displayed.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
