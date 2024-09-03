﻿//HintName: SimpleInterfaceAttribute.g.cs

namespace ProgrammerAl.SourceGenerators.InterfaceGenerator.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class GenerateInterfaceAttribute : System.Attribute
    {
        public string? InterfaceName { get; set; }
        public string? Namespace { get; set; }
    }

    [System.AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
    public class ExcludeFromGeneratedInterfaceAttribute : System.Attribute
    {
    }
}
