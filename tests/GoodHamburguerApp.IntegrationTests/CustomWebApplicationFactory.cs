using GoodHamburguerApp.Domain.Entities;
using GoodHamburguerApp.Domain.Enums;
using GoodHamburguerApp.Infra.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GoodHamburguerApp.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program> 
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
               
                services.RemoveAll(typeof(DbContextOptions<GoodHamburguerContext>));

                var internalServiceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                var root = new InMemoryDatabaseRoot();

                services.AddDbContext<GoodHamburguerContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting", root);
                    options.UseInternalServiceProvider(internalServiceProvider);
                });

                services.AddSingleton<IAuthorizationHandler, AllowAnonymousHandler>();

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                    options.DefaultScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
            });

        }

        
    }

    public class AllowAnonymousHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var requirement in context.PendingRequirements.ToList())
            {
                context.Succeed(requirement); 
            }
            return Task.CompletedTask;
        }
    }
}
