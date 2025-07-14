// namespace SettingBinder.SampleApi;
//
// /// <summary>
// /// Provides advanced, modular configuration extension methods for fine-grained setup control.
// /// </summary>
// public static class ConfigurationBuilderAdvancedExtensions
// {
//     /// <summary>
//     /// Adds the base path for configuration files, defaults to the current directory if not specified.
//     /// </summary>
//     public static IConfigurationBuilder WithBasePath(this IConfigurationBuilder builder, string? path = null)
//     {
//         var basePath = string.IsNullOrWhiteSpace(path) ? Directory.GetCurrentDirectory() : path;
//         return builder.SetBasePath(basePath);
//     }
//     
//     /// <summary>
//     /// Adds a JSON configuration file to the configuration sources.
//     /// </summary>
//     public static IConfigurationBuilder WithJsonFile(this IConfigurationBuilder builder, string path = "appsettings.json", bool optional = false, bool reloadOnChange = true) 
//         => builder.AddJsonFile(path, optional, reloadOnChange);
//     
//     /// <summary>
//     /// Adds a development-specific JSON configuration file to the configuration sources.
//     /// </summary>
//     public static IConfigurationBuilder WithEnvironmentJsonFile(this IConfigurationBuilder builder, string environment = "Development", bool optional = true, bool reloadOnChange = true) 
//         => builder.AddJsonFile($"appsettings.{environment}.json", optional, reloadOnChange);
//     
//     /// <summary>
//     /// Adds user secrets to the configuration sources for the specified type.
//     /// </summary>
//     /// <typeparam name="T">A type in the assembly containing the user secrets (typically <c>Program</c>).</typeparam>
//     public static IConfigurationBuilder WithSecrets<T>(this IConfigurationBuilder builder) where T : class 
//         => builder.AddUserSecrets<T>();
//     
//     /// <summary>
//     /// Adds environment variables to the configuration sources.
//     /// </summary>
//     public static IConfigurationBuilder WithEnvironmentVariables(this IConfigurationBuilder builder) 
//         => builder.AddEnvironmentVariables();
// }
//
// /// <summary>
// /// Provides a default configuration pipeline suitable for most applications.
// /// </summary>
// public static class ConfigurationBuilderDefaultsExtensions
// {
//     /// <summary>
//     /// Adds a pre-configured default configuration pipeline including base path, JSON files, user secrets, and environment variables.
//     /// </summary>
//     public static IConfigurationBuilder AddDefaultConfigurationSources<T>(this IConfigurationBuilder builder) where T : class 
//         => builder
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//             .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
//             .AddUserSecrets<T>()
//             .AddEnvironmentVariables();
// }