using System;
using System.Net.Http;
using FinancePlanner.WebClient.Services;
using IdentityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Microsoft.IdentityModel.Tokens;
using FinancePlanner.WebClient.HttpHandlers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Net.Http.Headers;

namespace FinancePlanner.WebClient.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddWebClientServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddServices();
        services.AddSecurity(config);
        services.AddHttpClient(config);
        return services;
    }

    // Services
    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IIncomeInformationService, IncomeInformationService>();
        services.AddScoped<IPayInformationService, PayInformationService>();
    }

    // HTTP Client
    private static void AddHttpClient(this IServiceCollection services, IConfiguration config)
    {
        services.AddTransient<AuthenticationDelegatingHandler>();

        AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError() // HttpRequestException, 5XX and 408
            .WaitAndRetryAsync(int.Parse(config.GetSection("Clients:RetryCount").Value),
                retryAttempt => TimeSpan.FromSeconds(retryAttempt));

        services.AddHttpClient(config.GetSection("Clients:ApiAggregatorClient:ClientName").Value, client =>
        {
            client.BaseAddress = new Uri(config.GetSection("Clients:ApiAggregatorClient:BaseURL").Value);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }).AddPolicyHandler(retryPolicy).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

        services.AddHttpClient(config.GetSection("IdentityServer:IdentityServerId").Value, client =>
        {
            client.BaseAddress = new Uri(config.GetSection("IdentityServer:Url").Value);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        });

        services.AddHttpContextAccessor();
    }

    // Security
    private static void AddSecurity(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = config["IdentityServer:Url"];
                options.ClientId = config["WebClient:ClientId"];
                options.ClientSecret = config["WebClient:ClientSecret"];
                options.ResponseType = config["WebClient:ResponseType"];
                options.Scope.Add("roles");
                options.Scope.Add("email");
                options.Scope.Add("address");
                options.Scope.Add(config["ApiResources:FinanceServices"]);
                options.Scope.Add(config["ApiResources:TaxServices"]);
                options.Scope.Add(config["ApiResources:WageServices"]);
                options.ClaimActions.MapJsonKey("role", "role");
                options.SaveTokens = true;
                options.UsePkce = false;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.GivenName,
                    RoleClaimType = JwtClaimTypes.Role
                };
            });
    }
}