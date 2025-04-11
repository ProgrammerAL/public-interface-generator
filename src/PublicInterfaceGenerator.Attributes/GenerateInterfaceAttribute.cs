//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.Attributes;

//[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
//public class GenerateInterfaceAttribute : Attribute
//{
//    /// <summary>
//    /// Set this to override the default interface name. Or leave it null to use the class name with an 'I' prepended to it.
//    /// </summary>
//    public string? InterfaceName { get; set; }

//    /// <summary>
//    /// Set this to override the namespace to generate the interface in. By default, it will be the same as the class.
//    /// </summary>
//    public string? Namespace { get; set; }

//    /// <summary>
//    /// Set this to specify the interfaces the generated interface will inherit from. For example, IDisposable. 
//    /// This should be a syntax-valid list as you would type it out normally because it will be concatenated directly into the interface definition.
//    /// For example: ""MyNamespace.MyInterface1, MyNamespace.MyInterface2""
//    /// </summary>
//    public string? Interfaces { get; set; }

//    /// <summary>
//    /// Set this to specify the generated interface inherits from System.IDisposable.
//    /// This will be appended to the list of interfaces the generated interface inherits from.
//    /// This is in addition to the <see cref="Interfaces"/> property.
//    /// If you are also specifying interfaces with the <see cref="Interfaces"/> property, 
//    ///   either set this to false and include "System.IDisposable" in the <see cref="Interfaces"/> property string, 
//    ///   or set this to true and don't include "System.IDisposable" in the <see cref="Interfaces"/> string.
//    ///   Failure to do this will result in System.IDisposable being appended to the generated interface twice.
//    /// </summary>
//    public bool IsIDisposable { get; set; }

//    /// <summary>
//    /// Set this to specify the generated interface inherits from <see cref="System.IAsyncDisposable"/>.
//    /// This is in addition to the <see cref="Interfaces"/> property.
//    /// If you are also specifying interfaces with the <see cref="Interfaces"/> property, 
//    ///   either set this to false and include "System.IAsyncDisposable" in the <see cref="Interfaces"/> property string, 
//    ///   or set this to true and don't include "System.IAsyncDisposable" in the <see cref="Interfaces"/> string.
//    ///   Failure to do this will result in System.IAsyncDisposable being appended to the generated interface twice.
//    /// </summary>
//    public bool IsIAsyncDisposable { get; set; }
//}

//public static class Constants
//{
//    public const string GenerateInterfaceAttributeName = nameof(GenerateInterfaceAttribute);
//    public static string GenerateInterfaceAttributeNameSpace = typeof(GenerateInterfaceAttribute).Namespace;
//    public static string GenerateInterfaceAttributeFullName = typeof(GenerateInterfaceAttribute).FullName;

//    public const string ExcludeFromGeneratedInterfaceAttributeName = "ExcludeFromGeneratedInterfaceAttribute";

//    public const string AttributeProperty_InterfaceName = nameof(InterfaceName);
//    public const string AttributeProperty_NamespaceName = nameof(Namespace);
//    public const string AttributeProperty_Interfaces = nameof(Interfaces);
//    public const string AttributeProperty_IsIDisposable = nameof(IsIDisposable);
//    public const string AttributeProperty_IsIAsyncDisposable = nameof(IsIAsyncDisposable);
//}
