namespace SettingBinder.SampleApi;

public static class ConfigurationBuilderAdvancedExtensions
{
    public static IConfigurationBuilder WithBasePath(this IConfigurationBuilder builder, string? path = null) => builder
        .SetBasePath(path ?? Directory.GetCurrentDirectory());
    
    public static IConfigurationBuilder WithJsonFile(this IConfigurationBuilder builder, string path = "appsettings.json", bool optional = true, bool reloadOnChange = true) 
        => builder.AddJsonFile(path, optional, reloadOnChange);
    
    public static IConfigurationBuilder WithDevJsonFile(this IConfigurationBuilder builder, string path = "appsettings.Development.json", bool optional = true, bool reloadOnChange = true) 
        => builder.AddJsonFile(path, optional, reloadOnChange);
    
    public static IConfigurationBuilder WithSecrets<T>(this IConfigurationBuilder builder) where T : class
        => builder.AddUserSecrets<T>();
    
    public static IConfigurationBuilder WithEnvironmentVariables(this IConfigurationBuilder builder) 
        => builder.AddEnvironmentVariables();
}

public static class ConfigurationBuilderDefaultsExtensions
{
    public static IConfigurationBuilder AddDefaultConfigurationSources(this IConfigurationBuilder builder)
    {
        builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();
                  
        return builder;
    }
}