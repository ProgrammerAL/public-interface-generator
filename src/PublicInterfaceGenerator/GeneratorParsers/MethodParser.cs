using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.GeneratorParsers;

public static class MethodParser
{
    public static InterfaceToGenerateInfo.Method? ExtractMethod(
        IMethodSymbol symbol,
        string extraClassInterfaces,
        bool inheritsFromIDisposable,
        bool inheritsFromIAsyncDisposable)
    {
        if (!IsSymbolValid(symbol, extraClassInterfaces, inheritsFromIDisposable, inheritsFromIAsyncDisposable))
        {
            return null;
        }

        var methodComments = CommentsBlockParser.ParseCommentsBlock(symbol);

        string? methodName = symbol.Name;
        string returnType;
        if (symbol.ReturnsVoid)
        {
            returnType = "void";
        }
        else
        {
            returnType = symbol.ReturnType.ToString();
        }

        var genericParameters = GenericsParser.ParseGenericParameters(symbol.TypeParameters);

        var argumentBuilder = ImmutableArray.CreateBuilder<InterfaceToGenerateInfo.MethodArgument>();

        foreach (var parameter in symbol.Parameters)
        {
            var argName = parameter.ToDisplayString();
            var nullableAnnotation = parameter.NullableAnnotation;
            var attributeStrings = parameter.GetAttributes().Select(x => x.ToString()).ToImmutableArray();
            string defaultValue = "";

            if (parameter.HasExplicitDefaultValue)
            {
                if (parameter.ExplicitDefaultValue is null)
                {
                    defaultValue = "null";
                }
                else if (parameter.ExplicitDefaultValue is string stringValue)
                {
                    defaultValue = $"\"{stringValue}\"";
                }
                else
                {
                    defaultValue = parameter.ExplicitDefaultValue.ToString();
                }
            }

            var interfaceArgument = new InterfaceToGenerateInfo.MethodArgument(argName, nullableAnnotation, attributeStrings, defaultValue);
            argumentBuilder.Add(interfaceArgument);
        }

        return new InterfaceToGenerateInfo.Method(methodName, returnType, argumentBuilder.ToImmutableArray(), genericParameters, methodComments);
    }

    private static bool IsSymbolValid(
        IMethodSymbol symbol,
        string extraClassInterfaces,
        bool inheritsFromIDisposable,
        bool inheritsFromIAsyncDisposable)
    {
        if (string.Equals(".ctor", symbol.Name, StringComparison.Ordinal))
        {
            //Don't make a method for the constructor
            return false;
        }
        else if (symbol.IsStatic)
        {
            //Only instance methods
            return false;
        }
        else if (symbol.DeclaredAccessibility != Accessibility.Public)
        {
            //Only public methods
            return false;
        }
        else if (symbol.Name.StartsWith("get_")
            || symbol.Name.StartsWith("set_"))
        {
            //These are the methods for properties
            //  Don't include those here because properties are handled separately
            return false;
        }
        else if (symbol.Name.StartsWith("add_")
            || symbol.Name.StartsWith("remove_"))
        {
            //These are the methods for events
            //  Don't include those here because events are handled separately
            return false;
        }
        else if (symbol.GetAttributes().Any(x => x.AttributeClass?.Name is AttributeGenerationHelper.GenerateInterfaceAttributeConstants.ExcludeFromGeneratedInterfaceAttributeName))
        {
            //Don't include methods that have the [IgnoreInGeneratedInterface] attribute
            return false;
        }
        else if (IsMethodFromInheritedInterface(symbol))
        {
            //If the method exists because of an interface it's implementing
            //  don't include it in the interface we generate
            return false;
        }
        else if (IsDisposeMethodAndImplementsIDisposable(symbol, extraClassInterfaces, inheritsFromIDisposable))
        {
            //If the code uses an attribute to set that the class implements IDisposable
            //  and this is the Dispose() method, don't include it in the interface
            //  Note: This is different from the method check above, because the concrete class won't have IDisposable in the definition list, it's on the interface
            return false;
        }
        else if (IsIAsyncDisposeMethodAndImplementsIAsyncDisposable(symbol, extraClassInterfaces, inheritsFromIAsyncDisposable))
        {
            //If the code uses an attribute to set that the class implements IAsyncDisposable
            //  and this is the DisposeAsync() method, don't include it in the interface
            //  Note: This is different from the method check above, because the concrete class won't have IAsyncDisposable in the definition list, it's on the interface
            return false;
        }

        return true;
    }

    private static bool IsDisposeMethodAndImplementsIDisposable(IMethodSymbol symbol, string extraClassInterfaces, bool inheritsFromIDisposable)
    {
        if (!symbol.Name.Equals("Dispose"))
        {
            return false;
        }

        if (!symbol.ReturnsVoid)
        {
            return false;
        }

        if (inheritsFromIDisposable)
        {
            return true;
        }

        var interfaces = extraClassInterfaces.
                            Split([','], StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim());
        if (interfaces.Any(x => string.Equals(x, "System.IDisposable") || string.Equals(x, "IDisposable")))
        {
            return true;
        }

        return true;
    }

    private static bool IsIAsyncDisposeMethodAndImplementsIAsyncDisposable(IMethodSymbol symbol, string extraClassInterfaces, bool inheritsFromIAsyncDisposable)
    {
        if (!symbol.Name.Equals("DisposeAsync"))
        {
            return false;
        }

        if (symbol.ReturnType.Name != "ValueTask")
        {
            return false;
        }

        if (inheritsFromIAsyncDisposable)
        {
            return true;
        }

        var interfaces = extraClassInterfaces.
                            Split([','], StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim());
        if (interfaces.Any(x => string.Equals(x, "System.IDisposable") || string.Equals(x, "IDisposable")))
        {
            return true;
        }

        return true;
    }

    private static bool IsMethodFromInheritedInterface(IMethodSymbol symbol)
    {
        var interfaces = symbol.ContainingType.AllInterfaces;

        foreach (var implementedInterface in interfaces)
        {
            var interfaceMethods = implementedInterface.GetMembers().OfType<IMethodSymbol>();
            foreach (var interfaceMethod in interfaceMethods)
            {
                var containingTypeImplementation = symbol.ContainingType.FindImplementationForInterfaceMember(interfaceMethod);
                if (symbol.Equals(containingTypeImplementation, SymbolEqualityComparer.Default))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
