using System;
using System.Net.Http;
using FinancePlanner.WebClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace FinancePlanner.WebClient.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddWebClientServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddServices();
            services.AddSecurity(config);
            services.AddHealthChecks(config);
            services.AddHttpClient(config);
            return services;
        }

        // Services
        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPayService, PayService>();
        }

        // HTTP Client
        private static void AddHttpClient(this IServiceCollection services, IConfiguration config)
        {
            AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError() // HttpRequestException, 5XX and 408
                .WaitAndRetryAsync(int.Parse(config.GetSection("Clients:RetryCount").Value),
                    retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            services.AddHttpClient(config.GetSection("Clients:ApiGatewayClient:ClientName").Value, client =>
            {
                client.BaseAddress = new Uri(config.GetSection("Clients:ApiGatewayClient:BaseURL").Value);
            }).AddPolicyHandler(retryPolicy);
        }

        // Health Check
        private static void AddHealthChecks(this IServiceCollection services, IConfiguration config)
        {
        }

        // Security
        private static void AddSecurity(this IServiceCollection services, IConfiguration config)
        {
        }
    }
}
