using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator;
public class AttributeGenerationHelper
{
    public static string GenerateAttributesCode()
    {
        var builder = new StringBuilder();

        _ = builder.AppendLine($"#nullable enable");
        _ = builder.AppendLine(GenerateAttributionsSuppressComments());

        //Another line to separate code from rules
        _ = builder.AppendLine();
        _ = builder.AppendLine(GenerateInterfaceAttributes());
        
        //Another line to separate code from rules
        _ = builder.AppendLine();

        _ = builder.Append(GenerateAttributionsSuppressionRestoreComments());

        return builder.ToString();
    }

    public static string GenerateInterfaceAttributes()
    {
        return
            """
            namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes
            {
                [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Property | System.AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
                [Microsoft.CodeAnalysis.EmbeddedAttribute]
                [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
                public sealed class ExcludeFromGeneratedInterfaceAttribute : System.Attribute
                {
                }

                [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
                [Microsoft.CodeAnalysis.EmbeddedAttribute]
                [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
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

    private static string GenerateAttributionsSuppressComments()
    {
        return """
            #pragma warning disable SA1649 // SA1649FileNameMustMatchTypeName
            #pragma warning disable SA1402 // FileMayOnlyContainASingleType
            #pragma warning disable SA1403 // SA1403FileMayOnlyContainASingleNamespace
            #pragma warning disable SA1502 // ElementMustNotBeOnSingleLine
            #pragma warning disable SA1013 // ClosingBracesMustBeSpacedCorrectly
            #pragma warning disable IDE0160 // Use block-scoped namespace 
            #pragma warning disable IDE0161 // Use file-scoped namespace
            """;
    }

    private static string GenerateAttributionsSuppressionRestoreComments()
    {
        return """
            #pragma warning restore SA1649  // SA1649FileNameMustMatchTypeName
            #pragma warning restore SA1402  // FileMayOnlyContainASingleType
            #pragma warning restore SA1403  // SA1403FileMayOnlyContainASingleNamespace
            #pragma warning restore SA1502  // ElementMustNotBeOnSingleLine
            #pragma warning restore SA1013  // ClosingBracesMustBeSpacedCorrectly
            #pragma warning restore IDE0160 // Use block-scoped namespace 
            #pragma warning restore IDE0161 // Use file-scoped namespace
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
