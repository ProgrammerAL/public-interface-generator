using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator;
public class AttributeGenerationHelper
{
    public static string GenerateAttributesCode()
    {
        var builder = new StringBuilder();

        _ = builder.AppendLine(GenerateEmbeddedAttribute());
        _ = builder.AppendLine();
        _ = builder.AppendLine(GenerateExcludeFromGeneratedInterfaceAttribute());
        _ = builder.AppendLine();
        _ = builder.AppendLine(GenerateGenerateInterfaceAttribute());

        return builder.ToString();
    }

    /// <summary>
    /// Creates the Microsoft.CodeAnalysis.EmbeddedAttribute
    /// This is a special attribute the compiler knows about. This allows us to have the same attribute defined in multiple assemblies in a project
    ///     We care about that when a user has multiple projects in a solution and they are using this source generator in each one, meaning the GenerateInterfaceAttribute will be defined in each of their projects
    /// </summary>
    public static string GenerateEmbeddedAttribute()
    {
        return
            """
            namespace Microsoft.CodeAnalysis
            {
                [System.AttributeUsage(System.AttributeTargets.All)]
                internal sealed class EmbeddedAttribute : System.Attribute {}
            }
            """;
    }

    public static string GenerateExcludeFromGeneratedInterfaceAttribute()
    {
        return
            """
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes
            {
                [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Property | System.AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
                [Microsoft.CodeAnalysis.EmbeddedAttribute]
                public sealed class ExcludeFromGeneratedInterfaceAttribute : System.Attribute
                {
                }
            }
            """;
    }

    public static string GenerateGenerateInterfaceAttribute()
    {
        return """
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes
            {
                [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
                [Microsoft.CodeAnalysis.EmbeddedAttribute]
                public sealed class GenerateInterfaceAttribute : System.Attribute
                {
                    /// <summary>
                    /// Set this to override the default interface name. Or leave it null to use the class name with an 'I' prepended to it.
                    /// </summary>
                    public string? InterfaceName { get; set; }

                    /// <summary>
                    /// Set this to override the namespace to generate the interface in. By default, it will be the same as the class.
                    /// </summary>
                    public string? Namespace { get; set; }

                    /// <summary>
                    /// Set this to specify the interfaces the generated interface will inherit from. For example, IDisposable. 
                    /// This should be a syntax-valid list as you would type it out normally because it will be concatenated directly into the interface definition.
                    /// For example: ""MyNamespace.MyInterface1, MyNamespace.MyInterface2""
                    /// </summary>
                    public string? Interfaces { get; set; }

                    /// <summary>
                    /// Set this to specify the generated interface inherits from System.IDisposable.
                    /// This will be appended to the list of interfaces the generated interface inherits from.
                    /// This is in addition to the <see cref="Interfaces"/> property.
                    /// If you are also specifying interfaces with the <see cref="Interfaces"/> property, 
                    ///   either set this to false and include "System.IDisposable" in the <see cref="Interfaces"/> property string, 
                    ///   or set this to true and don't include "System.IDisposable" in the <see cref="Interfaces"/> string.
                    ///   Failure to do this will result in System.IDisposable being appended to the generated interface twice.
                    /// </summary>
                    public bool IsIDisposable { get; set; }

                    /// <summary>
                    /// Set this to specify the generated interface inherits from <see cref="System.IAsyncDisposable"/>.
                    /// This is in addition to the <see cref="Interfaces"/> property.
                    /// If you are also specifying interfaces with the <see cref="Interfaces"/> property, 
                    ///   either set this to false and include "System.IAsyncDisposable" in the <see cref="Interfaces"/> property string, 
                    ///   or set this to true and don't include "System.IAsyncDisposable" in the <see cref="Interfaces"/> string.
                    ///   Failure to do this will result in System.IAsyncDisposable being appended to the generated interface twice.
                    /// </summary>
                    public bool IsIAsyncDisposable { get; set; }
                }
            }
            """;
    }

    public static class GenerateInterfaceAttributeConstants
    {
        public const string GenerateInterfaceAttributeName = "GenerateInterfaceAttribute";
        public static string GenerateInterfaceAttributeNameSpace = "ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes";
        public static string GenerateInterfaceAttributeFullName = $"{GenerateInterfaceAttributeNameSpace}.{GenerateInterfaceAttributeName}";

        public const string ExcludeFromGeneratedInterfaceAttributeName = "ExcludeFromGeneratedInterfaceAttribute";

        public const string AttributeProperty_InterfaceName = "InterfaceName";
        public const string AttributeProperty_NamespaceName = "Namespace";
        public const string AttributeProperty_Interfaces = "Interfaces";
        public const string AttributeProperty_IsIDisposable = "IsIDisposable";
        public const string AttributeProperty_IsIAsyncDisposable = "IsIAsyncDisposable";
    }
}
