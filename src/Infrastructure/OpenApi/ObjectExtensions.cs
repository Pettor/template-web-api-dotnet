﻿using System.Reflection;

namespace WebApiTemplate.Infrastructure.OpenApi;

internal static class ObjectExtensions
{
    public static T? TryGetPropertyValue<T>(this object? obj, string propertyName, T? defaultValue = default) =>
        obj?.GetType().GetRuntimeProperty(propertyName) is PropertyInfo propertyInfo
            ? (T?)propertyInfo.GetValue(obj)
            : defaultValue;
}