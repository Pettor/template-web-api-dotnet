﻿using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Backend.Infrastructure.Localization;

public class LocalizationMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var cultureKey = context.Request.Headers["Accept-Language"];
        if (!string.IsNullOrEmpty(cultureKey) && CultureExists(cultureKey!))
        {
            var culture = new CultureInfo(cultureKey!);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        if (next is null)
        {
            throw new ArgumentNullException(nameof(next));
        }

        await next(context);
    }

    private static bool CultureExists(string cultureName) =>
        CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .Any(culture =>
                string.Equals(culture.Name, cultureName, StringComparison.OrdinalIgnoreCase)
            );
}
