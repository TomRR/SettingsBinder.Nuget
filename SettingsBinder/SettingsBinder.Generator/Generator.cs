
using TomRR.SourceGenerator.SettingsBinder.SourceCode;

namespace TomRR.SourceGenerator.SettingsBinder;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Add the marker attribute to the compilation.
        context.RegisterPostInitializationOutput(ctx 
            => ctx.AddSource(
            $"SettingsAttribute.g.cs",
            SourceText.From(SettingsAttributeExtension.SourceCode, Encoding.UTF8)));
        
        var settingsClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => s is ClassDeclarationSyntax,
                transform: static (ctx, _) => GetSettingClass(ctx))
            .Where(static m => m is not null)
            .Select(static (m, _) => m!.Value)
            .Collect();

        context.RegisterSourceOutput(settingsClasses, Generate);
        

    }

    private static (string Namespace, string ClassName, string SectionName)? GetSettingClass(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classSyntax)
            return null;

        foreach (var attrList in classSyntax.AttributeLists)
        {
            foreach (var attr in attrList.Attributes)
            {
                var symbol = context.SemanticModel.GetSymbolInfo(attr).Symbol;
                if (symbol is IMethodSymbol methodSymbol)
                {
                    var attrName = methodSymbol.ContainingType.ToDisplayString();

                    // Match [Settings] or fully-qualified SettingsAttribute
                    if (attrName.EndsWith(".SettingsAttribute") || attrName.EndsWith(".Settings"))
                    {
                        if (context.SemanticModel.GetDeclaredSymbol(classSyntax) is INamedTypeSymbol classSymbol)
                        {
                            var sectionName = classSymbol.Name;

                            // Check constructor argument for override
                            if (attr.ArgumentList?.Arguments.Count > 0)
                            {
                                var constValue = context.SemanticModel.GetConstantValue(attr.ArgumentList.Arguments[0].Expression);
                                if (constValue.HasValue && constValue.Value is string str)
                                {
                                    sectionName = str;
                                }
                            }
                            
                            return (
                                Namespace: classSymbol.ContainingNamespace.ToDisplayString(),
                                ClassName: classSymbol.Name,
                                SectionName: sectionName
                            );
                        }
                    }
                }
            }
        }

        return null;
    }

    private static void Generate(SourceProductionContext context, ImmutableArray<(string Namespace, string ClassName, string SectionName)> settingsTypes)
    {
        // 1. Interface (once)
        context.AddSource("ISettings.Settings.g.cs", SourceText.From(SettingsInterfaceExtension.SourceCode, Encoding.UTF8));

        // 2. Per-class partials
        foreach (var (ns, className, sectionName) in settingsTypes)
        {
            var partial = SettingsPocoExtension.SourceCode(ns, className, sectionName);
            context.AddSource($"{className}.Settings.g.cs", SourceText.From(partial, Encoding.UTF8));
        }
        context.AddSource($"ConfigurationBuilder.Default.Extensions.g.cs", SourceText.From(DefaultConfigurationExtension.SourceCode, Encoding.UTF8));
        context.AddSource($"ConfigurationBuilder.Advanced.Extensions.g.cs", SourceText.From(AdvancedConfigurationExtension.SourceCode, Encoding.UTF8));

        // 3. Global binding extension method
        var binder = SettingsBinderExtension.SourceCode(settingsTypes);
        context.AddSource($"SettingsBinder.Extensions.g.cs", SourceText.From(binder, Encoding.UTF8));
    }
}