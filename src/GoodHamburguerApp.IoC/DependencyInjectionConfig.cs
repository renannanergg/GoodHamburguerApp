using System.Reflection;
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

            services.AddMediatR(cfg => 
            {
                // Usa o Behavior para achar o projeto Application
                cfg.RegisterServicesFromAssembly(typeof(ValidationBehavior<,>).Assembly);
                
                // Registra o middleware de validação
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // Validadores FluentValidation e behaviros 
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}