# ⚙️ SettingsBinder

A lightweight, zero-boilerplate Roslyn Source Generator that binds `[Settings]`-annotated classes directly to the .NET Options pattern.

---

## ✨ Features

- ✅ Automatic binding of settings from `appsettings.json`
- ✅ Strongly-typed settings via `[Settings]` attribute
- ✅ Supports `IOptions<T>`, validation, and DI
- ✅ Source-generated `SectionName` and registration logic
- ✅ Modular and customizable configuration pipelines
- ✅ Zero reflection, minimal runtime overhead
- ✅ XML Documentation

---

## 🚀 Quick Start

### 1. Install NuGet Package

```bash
dotnet add package TomRR.SourceGenerator.SettingsBinder
```

### 2. Define a Settings Class

```csharp
using TomRR.SourceGenerator.SettingsBinder;

[Settings]
public sealed partial class LoggingSettings
{
    public string? Level { get; init; }
    public string? Format { get; init; }
}
```

### 3. Configure to appsettings.json
```json
{
  "LoggingSettings": {
    "Level": "Information",
    "Format": "Json"
  }
}
```

### 4. Wire Up in Program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);

// ✅ Default configuration sources
builder.Configuration.AddDefaultConfigurationSources<Program>();

// ✅ Registers all [Settings] classes automatically
builder.AddSettingsOptions();

var app = builder.Build();

```
✅ You're done!

 ## 🧩 Advanced Configuration Pipeline
If you prefer fine-grained control over configuration sources:
```csharp
builder.Configuration
    .WithBasePath()
    .WithJsonFile()
    .WithEnvironmentJsonFile()
    .WithSecrets<Program>()
    .WithEnvironmentVariables();
```
You can mix & match based on your project needs.

## ✅ What Gets Generated?

### ✅ Interface
```csharp
public interface ISettings
{
    static abstract string SectionName { get; }
}
```

### ✅ Partial Class Implementation with SectionName
```csharp
public sealed partial class LoggingSettings : ISettings
{
    public static string SectionName => "LoggingSettings";
}
```

### ✅ Default Configuration Sourcing
```csharp
public static IConfigurationBuilder AddDefaultConfigurationSources<T>(this IConfigurationBuilder builder) where T : class 
    => builder
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
        .AddUserSecrets<T>()
        .AddEnvironmentVariables();
```

### ✅ Automatic Binding with Validation
```csharp
builder.Services
    .AddOptions<LoggingSettings>()
    .Bind(builder.Configuration.GetSection(LoggingSettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

## 🎛️ Customize Section Names
You can explicitly override the section name using the attribute:

```csharp
[Settings("MyCustomSection")]
public sealed partial class AdvancedSettings
{
    public string? Mode { get; init; }
    public bool Enabled { get; init; }
}
```

This generates:
```csharp
public sealed partial class AdvancedSettings : ISettings
{
    public static string SectionName => "MyCustomSection";
}
```

```csharp
{
  "MyCustomSection": {
    "Mode": "Debug",
    "Enabled": true
  }
}
```

🧠 Use this when the config section does not match the class name.

## 🏆 Performance
SettingsBinder:
- ✅ Zero reflection at runtime (everything is compile-time generated).
- ✅ No ActivatorUtilities, no extra runtime DI overhead.
- ✅ Source generation ensures optimal performance with minimal allocations.

[//]: # (ToDo:)
[//]: # (📊 Benchmarks )
[//]: # (
[//]: # (Performance comparisons vs manual IOptions<T> or reflection-based solutions.)


## 📦 Package Info
| Name       | Description                                             |
|------------|---------------------------------------------------------|
| Package ID | `TomRR.SourceGenerator.SettingsBinder`                  |
| License    | Apache 2.0                                              |
| Author     | Tom-Robert Resing                                       |
| Repo       | [GitHub](https://github.com/TomRR/SettingsBinder.Nuget) |
| Nuget      | [Nuget](https://www.nuget.org/packages/TomRR.SourceGenerator.SettingsBinder/)  |


## 📄 License

Licensed under the Apache License 2.0.