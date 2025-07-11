﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace Backend.Infrastructure.OpenApi;

internal static class Startup
{
    internal static IServiceCollection AddOpenApiDocumentation(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
        if (settings is { Enable: false })
        {
            return services;
        }

        services.AddVersionedApiExplorer(o => o.SubstituteApiVersionInUrl = true);
        services.AddEndpointsApiExplorer();
        services.AddOpenApiDocument(
            (document, serviceProvider) =>
            {
                document.PostProcess = doc =>
                {
                    doc.Info.Title = settings!.Title;
                    doc.Info.Version = settings.Version;
                    doc.Info.Description = settings.Description;
                    doc.Info.Contact = new OpenApiContact
                    {
                        Name = settings.ContactName,
                        Email = settings.ContactEmail,
                        Url = settings.ContactUrl,
                    };
                    doc.Info.License = new OpenApiLicense
                    {
                        Name = settings.LicenseName,
                        Url = settings.LicenseUrl,
                    };
                };

                if (
                    config["SecuritySettings:Provider"]!.Equals(
                        "AzureAd",
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    document.AddSecurity(
                        JwtBearerDefaults.AuthenticationScheme,
                        new OpenApiSecurityScheme
                        {
                            Type = OpenApiSecuritySchemeType.OAuth2,
                            Flow = OpenApiOAuth2Flow.AccessCode,
                            Description = "OAuth2.0 Auth Code with PKCE",
                            Flows = new OpenApiOAuthFlows
                            {
                                AuthorizationCode = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = config[
                                        "SecuritySettings:Swagger:AuthorizationUrl"
                                    ],
                                    TokenUrl = config["SecuritySettings:Swagger:TokenUrl"],
                                    Scopes = new Dictionary<string, string>
                                    {
                                        {
                                            config["SecuritySettings:Swagger:ApiScope"]
                                                ?? string.Empty,
                                            "access the api"
                                        },
                                    },
                                },
                            },
                        }
                    );
                }
                else
                {
                    document.AddSecurity(
                        JwtBearerDefaults.AuthenticationScheme,
                        new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Description = "Input your Bearer token to access this API",
                            In = OpenApiSecurityApiKeyLocation.Header,
                            Type = OpenApiSecuritySchemeType.Http,
                            Scheme = JwtBearerDefaults.AuthenticationScheme,
                            BearerFormat = "JWT",
                        }
                    );
                }

                document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
                document.OperationProcessors.Add(new SwaggerGlobalAuthProcessor());
                document.OperationProcessors.Add(new SwaggerHeaderAttributeProcessor());
            }
        );

        return services;
    }

    internal static IApplicationBuilder UseOpenApiDocumentation(
        this IApplicationBuilder app,
        IConfiguration config
    )
    {
        if (!config.GetValue<bool>("SwaggerSettings:Enable"))
        {
            return app;
        }

        app.UseOpenApi();
        app.UseSwaggerUi(options =>
        {
            options.DefaultModelsExpandDepth = -1;
            options.DocExpansion = "none";
            options.TagsSorter = "alpha";
            if (
                !config["SecuritySettings:Provider"]!.Equals(
                    "AzureAd",
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                return;
            }

            options.OAuth2Client = new OAuth2ClientSettings
            {
                AppName = "[AppName]",
                ClientId = config["SecuritySettings:Swagger:OpenIdClientId"],
                UsePkceWithAuthorizationCodeGrant = true,
                ScopeSeparator = " ",
            };
            options.OAuth2Client.Scopes.Add(config["SecuritySettings:Swagger:ApiScope"]);
        });

        return app;
    }
}
