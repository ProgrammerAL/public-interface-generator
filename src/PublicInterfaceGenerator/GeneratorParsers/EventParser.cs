using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.PublicInterfaceGenerator.GeneratorParsers;

public static class EventParser
{
    public static InterfaceToGenerateInfo.Event? ExtractEvent(IEventSymbol symbol)
    {
        if (!IsSymbolValid(symbol))
        {
            return null;
        }

        var comments = CommentsBlockParser.ParseCommentsBlock(symbol);
        string? name = symbol.Name;
        string dataType = symbol.Type.ToDisplayString();

        return new InterfaceToGenerateInfo.Event(name, dataType, comments);
    }

    private static bool IsSymbolValid(IEventSymbol symbol)
    {
        if (symbol.IsStatic)
        {
            //Only instance
            return false;
        }
        else if (symbol.DeclaredAccessibility != Accessibility.Public)
        {
            //Only public
            return false;
        }
        else if (symbol.GetAttributes().Any(x => x.AttributeClass?.Name is AttributeGenerationHelper.GenerateInterfaceAttributeConstants.ExcludeFromGeneratedInterfaceAttributeName))
        {
            //Don't include methods that have the [IgnoreInGeneratedInterface] attribute
            return false;
        }
        else if (IsEventFromInheritedInterface(symbol))
        {
            //If the event exists because of an interface it's implementing
            //  don't include it in the interface we generate
            return false;
        }

        return true;
    }

    private static bool IsEventFromInheritedInterface(IEventSymbol symbol)
    {
        var interfaces = symbol.ContainingType.AllInterfaces;

        foreach (var implementedInterface in interfaces)
        {
            var interfaceEvents = implementedInterface.GetMembers().OfType<IEventSymbol>();
            foreach (var interfaceEvent in interfaceEvents)
            {
                var containingTypeImplementation = symbol.ContainingType.FindImplementationForInterfaceMember(interfaceEvent);
                if (symbol.Equals(containingTypeImplementation, SymbolEqualityComparer.Default))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
