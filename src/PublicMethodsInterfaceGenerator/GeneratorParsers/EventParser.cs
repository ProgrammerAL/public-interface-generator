﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;

namespace ProgrammerAl.SourceGenerators.PublicMethodsInterfaceGenerator.GeneratorParsers;

public static class EventParser
{
    public static SimpleInterfaceToGenerate.Event? ExtractEvent(IEventSymbol symbol)
    {
        if (!IsSymbolValid(symbol))
        {
            return null;
        }

        string? name = symbol!.Name;
        string dataType = symbol.Type.ToDisplayString();

        return new SimpleInterfaceToGenerate.Event(name, dataType);
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

        return true;
    }
}
