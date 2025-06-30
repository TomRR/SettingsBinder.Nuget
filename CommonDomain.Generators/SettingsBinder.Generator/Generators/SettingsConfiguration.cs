// namespace SettingsBinder.Generator.Generators;
//
// /// <summary>
// /// A sample source generator that creates a custom report based on class properties. The target class should be annotated with the 'Generators.ReportAttribute' attribute.
// /// When using the source code as a baseline, an incremental source generator is preferable because it reduces the performance overhead.
// /// </summary>
// [Generator]
// public class EntitiesGenerator : IIncrementalGenerator
// {
//     public void Initialize(IncrementalGeneratorInitializationContext context)
//     {
//         // Filter classes annotated with the [Report] attribute. Only filtered Syntax Nodes can trigger code generation.
//         var provider = context.SyntaxProvider
//             .CreateSyntaxProvider(
//                 (s, _) => s is ClassDeclarationSyntax,
//                 (ctx, _) => GetClassDeclarationForSourceGen(ctx))
//             .Where(t => t.reportAttributeFound)
//             .Select((t, _) => t.Item1);
//
//         // Generate the source code.
//         context.RegisterSourceOutput(context.CompilationProvider.Combine(provider.Collect()),
//             ((ctx, t) => GenerateCode(ctx, t.Left, t.Right)));
//     }
//
//     /// <summary>
//     /// Checks whether the Node is annotated with the [Report] attribute and maps syntax context to the specific node type (ClassDeclarationSyntax).
//     /// </summary>
//     /// <param name="context">Syntax context, based on CreateSyntaxProvider predicate</param>
//     /// <returns>The specific cast and whether the attribute was found.</returns>
//     private static (ClassDeclarationSyntax, bool reportAttributeFound) GetClassDeclarationForSourceGen(
//         GeneratorSyntaxContext context)
//     {
//         var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
//
//         // Go through all attributes of the class.
//         foreach (AttributeListSyntax attributeListSyntax in classDeclarationSyntax.AttributeLists)
//         foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
//         {
//             if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
//             {
//                 continue; // if we can't get the symbol, ignore it
//             } 
//
//             string attributeName = attributeSymbol.ContainingType.ToDisplayString();
//
//             // filter
//             if (attributeName.Contains("Settings"))
//             {
//                 return (classDeclarationSyntax, true);
//             }
//         }
//
//         return (classDeclarationSyntax, false);
//     }
//
//     /// <summary>
//     /// Generate code action.
//     /// It will be executed on specific nodes (ClassDeclarationSyntax annotated with the [Report] attribute) changed by the user.
//     /// </summary>
//     /// <param name="context">Source generation context used to add source files.</param>
//     /// <param name="compilation">Compilation used to provide access to the Semantic Model.</param>
//     /// <param name="classDeclarations">Nodes annotated with the [Report] attribute that trigger the generate action.</param>
//     private void GenerateCode(SourceProductionContext context, Compilation compilation,
//         ImmutableArray<ClassDeclarationSyntax> classDeclarations)
//     {
//         // Go through all filtered class declarations.
//         foreach (var classDeclarationSyntax in classDeclarations)
//         {
//             // We need to get semantic model of the class to retrieve metadata.
//             var semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
//
//             // Symbols allow us to get the compile-time information.
//             if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
//             {
//                 continue;
//             }
//
//             var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
//
//             // 'Identifier' means the token of the node. Get class name from the syntax node.
//             var className = classDeclarationSyntax.Identifier.Text;
//
//             // Build up the source code
//             var bindingCode = GetSettingsBindingCode(namespaceName, className);
//             var interfaceCode = GetInterfaceCode();
//             var settingsCode = GetSettingsCode(className);
//
//             // Add the source code to the compilation.
//             context.AddSource($"{className}Base.g.cs", SourceText.From(settingsCode, Encoding.UTF8));
//             
//                          context.AddSource($"{className}_ISettings.g.cs", SourceText.From(interfaceCode, Encoding.UTF8));
//              context.AddSource($"{className}_Partial.g.cs", SourceText.From(settingsCode, Encoding.UTF8));
//              context.AddSource($"{className}_Binding.g.cs", SourceText.From(bindingCode, Encoding.UTF8));
//         }
//     }
//
//     private static string GetSettingsBindingCode(string namespaceName, string className) => $@"// <auto-generated/>
// #nullable enable
// using SettingsBinder.Generator.Generators.Entities;
//
// namespace {namespaceName};
//
// public static class SettingsConfiguration
// {{
//     private {className}({className}Id id) : base(id) {{ }}
// }}
// ";
//     
//     
//     private static string GetInterfaceCode() => $@"
//
// public partial interface ISettings
// {{
//     public static string SettingsSection {{ get; }}
// }}";
//
//     private static string GetSettingsCode(string className) => $@"
//
// public sealed partial class {{className}} : ISettings
// {{
//         public static string SettingsSection => {className}
// }}";
//
// }
//
//
//
