using System.Reflection;
using Asp.Versioning;
using FluentValidation;
using GoodHamburguerApp.Application.Behaviors;
using GoodHamburguerApp.Application.Mappings;
using GoodHamburguerApp.Domain.Interfaces;
using GoodHamburguerApp.Infra.Context;
using GoodHamburguerApp.Infra.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguerApp.IoC
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services,  IConfiguration configuration)
        {
            services.AddDbContext<GoodHamburguerContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            });

            // Repositories e UoW
            services.AddScoped<IPedidoRepository, PedidosRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // MediatR e Behaviors
            services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssembly(typeof(ValidationBehavior<,>).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version")
                );
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.ConfigureOptions<ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
        }
    }
}