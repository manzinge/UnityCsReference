// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace UnityEditor.PackageManager
{
    public static partial class Client
    {
        public static ListRequest List(bool offlineMode, bool includeIndirectDependencies)
        {
            long operationId;
            var status = List(out operationId, offlineMode, includeIndirectDependencies);
            return new ListRequest(operationId, status);
        }

        public static ListRequest List(bool offlineMode)
        {
            return List(offlineMode, false);
        }

        public static ListRequest List()
        {
            return List(false, false);
        }

        public static AddRequest Add(string identifier)
        {
            if (string.IsNullOrEmpty(identifier?.Trim()))
                throw new ArgumentException("Package identifier cannot be null, empty or whitespace", nameof(identifier));

            long operationId;
            var status = Add(out operationId, identifier);
            return new AddRequest(operationId, status);
        }

        internal static AddScopedRegistryRequest AddScopedRegistry(string registryName, string url, string[] scopes)
        {
            if (string.IsNullOrEmpty(registryName?.Trim()))
                throw new ArgumentException("Registry name cannot be null, empty or whitespace", nameof(registryName));

            long operationId;
            var status = AddScopedRegistry(out operationId, registryName, url, scopes);
            return new AddScopedRegistryRequest(operationId, status);
        }

        public static EmbedRequest Embed(string packageName)
        {
            if (string.IsNullOrEmpty(packageName?.Trim()))
                throw new ArgumentException("Package name cannot be null, empty or whitespace", nameof(packageName));

            var packageInfo = PackageInfo.GetAll().FirstOrDefault(p => p.name == packageName);
            if (packageInfo == null)
                throw new InvalidOperationException($"Cannot embed package [{packageName}] because it is not registered in the Asset Database.");

            Debug.Assert(packageInfo.entitlements.isAllowed, "Expected [entitlements.isAllowed] flag to be true.");

            long operationId;
            var status = Embed(out operationId, packageName);
            return new EmbedRequest(operationId, status);
        }

        internal static GetRegistriesRequest GetRegistries()
        {
            long operationId;
            var status = GetRegistries(out operationId);
            return new GetRegistriesRequest(operationId, status);
        }

        internal static GetCachedPackagesRequest GetCachedPackages(string registryId)
        {
            if (string.IsNullOrEmpty(registryId?.Trim()))
                throw new ArgumentException("Registry id cannot be null, empty or whitespace", nameof(registryId));

            long operationId;
            var status = GetCachedPackages(out operationId, registryId);
            return new GetCachedPackagesRequest(operationId, status);
        }

        internal static ListBuiltInPackagesRequest ListBuiltInPackages()
        {
            long operationId;
            var status = ListBuiltInPackages(out operationId);
            return new ListBuiltInPackagesRequest(operationId, status);
        }

        public static RemoveRequest Remove(string packageName)
        {
            if (string.IsNullOrEmpty(packageName?.Trim()))
                throw new ArgumentException("Package name cannot be null, empty or whitespace", nameof(packageName));

            long operationId;
            var status = Remove(out operationId, packageName);
            return new RemoveRequest(operationId, status, packageName);
        }

        internal static RemoveScopedRegistryRequest RemoveScopedRegistry(string registryId)
        {
            if (string.IsNullOrEmpty(registryId?.Trim()))
                throw new ArgumentException("Registry id cannot be null, empty or whitespace", nameof(registryId));

            long operationId;
            var status = RemoveScopedRegistry(out operationId, registryId);
            return new RemoveScopedRegistryRequest(operationId, status);
        }

        public static SearchRequest Search(string packageIdOrName, bool offlineMode)
        {
            if (string.IsNullOrEmpty(packageIdOrName?.Trim()))
                throw new ArgumentException("Package id or name cannot be null, empty or whitespace", nameof(packageIdOrName));

            long operationId;
            var status = GetPackageInfo(out operationId, packageIdOrName, offlineMode);
            return new SearchRequest(operationId, status, packageIdOrName);
        }

        public static SearchRequest Search(string packageIdOrName)
        {
            return Search(packageIdOrName, false);
        }

        public static SearchRequest SearchAll(bool offlineMode)
        {
            long operationId;
            var status = GetPackageInfo(out operationId, string.Empty, offlineMode);
            return new SearchRequest(operationId, status, string.Empty);
        }

        public static SearchRequest SearchAll()
        {
            return SearchAll(false);
        }

        internal static PerformSearchRequest Search(SearchOptions options)
        {
            long operationId;
            var status = Search(out operationId, options);
            return new PerformSearchRequest(operationId, status);
        }

        public static ResetToEditorDefaultsRequest ResetToEditorDefaults()
        {
            long operationId;
            var status = ResetToEditorDefaults(out operationId);
            return new ResetToEditorDefaultsRequest(operationId, status);
        }

        public static PackRequest Pack(string packageFolder, string targetFolder)
        {
            if (string.IsNullOrEmpty(packageFolder?.Trim()))
                throw new ArgumentException("Package folder cannot be null, empty or whitespace", nameof(packageFolder));

            if (string.IsNullOrEmpty(targetFolder?.Trim()))
                throw new ArgumentException("Target folder cannot be null, empty or whitespace", nameof(targetFolder));

            long operationId;
            var status = Pack(out operationId, packageFolder, targetFolder);
            return new PackRequest(operationId, status);
        }

        internal static UpdateScopedRegistryRequest UpdateScopedRegistry(string registryId, UpdateScopedRegistryOptions options)
        {
            if (string.IsNullOrEmpty(registryId?.Trim()))
                throw new ArgumentException("Registry id cannot be null, empty or whitespace", nameof(registryId));

            long operationId;
            var status = UpdateScopedRegistry(out operationId, registryId, options);
            return new UpdateScopedRegistryRequest(operationId, status);
        }
    }
}
