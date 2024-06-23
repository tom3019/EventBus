using System.Reflection;

namespace EventBus.RabbitMq;

internal static class ApplicationInformation
{
    /// <summary>
    /// Gets the value of the manifest module name
    /// </summary>
    public static string ManifestModuleName => Assembly.GetEntryAssembly()?
        .ManifestModule
        .Name ?? string.Empty;

    /// <summary>
    /// Gets the value of the application name
    /// </summary>
    public static string ApplicationName => Assembly.GetEntryAssembly().GetName().Name;
}