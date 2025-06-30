# ⚙️ SettingsBinder

A lightweight, zero-boilerplate Roslyn Source Generator that binds `[Settings]`-annotated classes directly to the .NET Options pattern.

---

## ✨ Features

- ✅ Automatic binding of settings from `appsettings.json`
- ✅ Strongly-typed configuration via `[Settings]` attribute
- ✅ Supports `IOptions<T>`, validation, and DI
- ✅ Source-generated `SectionName` and registration logic
- ✅ Zero reflection, minimal runtime overhead

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
public sealed partial class LoggingSettings : ISettings
{
    public static string SectionName => "LoggingSettings";

    public string? Level { get; init; }
    public string? Format { get; init; }
}

```

### 3. Add to appsettings.json
```json
{
  "LoggingSettings": {
    "Level": "Information",
    "Format": "Json"
  }
}
```

### 4. Register with DI in Program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add base configuration files
builder.Configuration.AddConfigurationSources();

// Automatically registers all [Settings] classes
builder.AddSettingsOptions();

var app = builder.Build();
```


## ✅ What Gets Generated?

### ✅ Interface
```csharp
public interface ISettings
{
    static abstract string SectionName { get; }
}
```

### ✅ Partial Class Implementation
```csharp
public sealed partial class LoggingSettings : ISettings
{
    public static string SectionName => "LoggingSettings";
}
```

### ✅ Binding Extension
```csharp
builder.Services
    .AddOptions<LoggingSettings>()
    .Bind(builder.Configuration.GetSection(LoggingSettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
```
## 🧩 Customize Section Names
You can explicitly override the section name using the attribute:
```csharp
[Settings("MyCustomSection")]
public sealed partial class AdvancedSettings : ISettings
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

## ➕ Adding More Settings
Just define more [Settings]-annotated classes:
```csharp
[Settings]
public sealed partial class ApiKeySettings : ISettings
{
    public string Key { get; init; } = default!;
}
```
All [Settings] classes are automatically picked up and registered.

## 📦 Package Info
| Name       | Description                                             |
| ---------- | ------------------------------------------------------- |
| Package ID | `TomRR.SourceGenerator.SettingsBinder`                  |
| License    | Apache 2.0                                              |
| Author     | Tom-Robert Resing                                       |
| Repo       | [GitHub](https://github.com/TomRR/SettingsBinder.Nuget) |

## 📄 License

Licensed under the Apache License 2.0.