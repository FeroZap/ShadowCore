﻿// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ShadowBox.Utilities.Localization
{
    /// <summary>
    /// An <see cref="IStringLocalizerFactory"/> that creates instances of <see cref="ResourceManagerStringLocalizer"/>
    /// and will properly handle the resources of ClassLibraries.
    /// </summary>
    public class ClassLibraryStringLocalizerFactory : ResourceManagerStringLocalizerFactory
    {
        private readonly IReadOnlyDictionary<string, string> _resourcePathMappings;
        private readonly ClassLibraryLocalizationOptions _classLibraryLocalizationOptions;

        public ClassLibraryStringLocalizerFactory(
            IOptions<LocalizationOptions> localizationOptions,
            IOptions<ClassLibraryLocalizationOptions> classLibraryLocalizationOptions,
            ILoggerFactory loggerFactory)
                : base(localizationOptions, loggerFactory)
        {
            _resourcePathMappings = classLibraryLocalizationOptions.Value.ResourcePaths;
            _classLibraryLocalizationOptions = classLibraryLocalizationOptions.Value;
        }

        protected override string GetResourcePrefix(TypeInfo typeInfo)
        {
            var assemblyName = typeInfo.Assembly.GetName().Name;
            return GetResourcePrefix(typeInfo, assemblyName, GetResourcePath(assemblyName));
        }

        protected override string GetResourcePrefix(TypeInfo typeInfo, string baseNamespace, string resourcesRelativePath)
        {
            var assemblyName = new AssemblyName(typeInfo.Assembly.FullName);
            var customRelativeResourcePath = $"{GetResourcePath(assemblyName.Name)}{GetSeparateFolderResourcePath(typeInfo)}";

            var b = base.GetResourcePrefix(typeInfo, baseNamespace, GetResourcePath(assemblyName.Name));
            
            var a = $"{baseNamespace}.{customRelativeResourcePath}{typeInfo.Name}";
            return a;
        }

        private string GetResourcePath(string assemblyName)
        {
            if (!_resourcePathMappings.TryGetValue(assemblyName, out var resourcePath))
            {
                throw new KeyNotFoundException("Attempted to access an assembly which doesn't have a resourcePath set.");
            }

            if (string.IsNullOrEmpty(resourcePath))
            {
                return resourcePath;
            }

            resourcePath = resourcePath.Replace(Path.AltDirectorySeparatorChar, '.')
                                       .Replace(Path.DirectorySeparatorChar, '.') + ".";
            return resourcePath;
        }

        private string GetSeparateFolderResourcePath(TypeInfo typeInfo)
        {
            return _classLibraryLocalizationOptions.UseSeparateFolderForEachResource ? $"{typeInfo.Name}." : String.Empty;
        }
    }
}
