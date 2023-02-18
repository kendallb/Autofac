﻿// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Concurrent;
using System.Reflection;
using Autofac.Core;

namespace Autofac.Util.Cache;

/// <summary>
/// Helper class to determine the set of all assemblies referenced by a type, including indirect references via generic arguments and base classes.
/// </summary>
internal static class TypeAssemblyReferenceProvider
{
    public static IEnumerable<Assembly> GetAllReferencedAssemblies(Type inputType)
    {
        var set = new HashSet<Assembly>();

        PopulateAllReferencedAssemblies(inputType, set);

        return set;
    }

    public static void PopulateAllReferencedAssemblies(Type inputType, HashSet<Assembly> holdingSet)
    {
        if (inputType.IsArray && inputType.GetElementType() is Type elementType)
        {
            PopulateAllReferencedAssemblies(elementType, holdingSet);
        }

        var genericArguments = inputType.GenericTypeArguments;

        foreach (var genericArgumentType in genericArguments)
        {
            PopulateAllReferencedAssemblies(genericArgumentType, holdingSet);
        }

        holdingSet.Add(inputType.Assembly);

        if (inputType.BaseType is not null && inputType.BaseType != typeof(object))
        {
            PopulateAllReferencedAssemblies(inputType.BaseType, holdingSet);
        }
    }
}
