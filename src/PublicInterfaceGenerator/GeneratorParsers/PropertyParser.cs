using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.GeneratorParsers;

public static class PropertyParser
{
    public static InterfaceToGenerateInfo.Property? ExtractProperty(IPropertySymbol symbol)
    {
        if (!IsSymbolValid(symbol))
        {
            return null;
        }

        var propertyComments = CommentsBlockParser.ParseCommentsBlock(symbol);

        string? propertyName = symbol.Name;
        string dataType = symbol.Type.ToDisplayString();

        var hasValidGet = symbol.GetMethod?.DeclaredAccessibility is Accessibility.Public;
        var hasValidSet = symbol.SetMethod?.DeclaredAccessibility is Accessibility.Public;

        if (hasValidGet || hasValidSet)
        {
            return new InterfaceToGenerateInfo.Property(propertyName, dataType, hasValidGet, hasValidSet, propertyComments);
        }

        //If neither getter nor setter is public, then don't include this property
        return null;
    }

    private static bool IsSymbolValid(IPropertySymbol symbol)
    {
        if (symbol.IsStatic)
        {
            //Only instance properties
            return false;
        }
        else if (symbol.DeclaredAccessibility != Accessibility.Public)
        {
            //Only public properties
            return false;
        }
        else if (symbol.GetAttributes().Any(x => x.AttributeClass?.Name is AttributeGenerationHelper.GenerateInterfaceAttributeConstants.ExcludeFromGeneratedInterfaceAttributeName))
        {
            //Don't include methods that have the [IgnoreInGeneratedInterface] attribute
            return false;
        }
        else if (IsPropertyFromInheritedInterface(symbol))
        {
            //If the property exists because of an interface it's implementing
            //  don't include the property in the interface we generate
            //Note: If the property from the inherited interface has a different accessibility for the getter or setter, it still won't be included
            //  For example, interface has property `string Name { get; }`, but the concrete class impliments property `public string Name { get; set; }`
            //      The property won't be included in the generated interface
            return false;
        }

        return true;
    }

    private static bool IsPropertyFromInheritedInterface(IPropertySymbol symbol)
    {
        var interfaces = symbol.ContainingType.AllInterfaces;

        foreach (var implementedInterface in interfaces)
        {
            var interfaceProperties = implementedInterface.GetMembers().OfType<IPropertySymbol>();
            foreach (var interfaceProperty in interfaceProperties)
            {
                var containingTypeImplementation = symbol.ContainingType.FindImplementationForInterfaceMember(interfaceProperty);
                if (symbol.Equals(containingTypeImplementation, SymbolEqualityComparer.Default))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
