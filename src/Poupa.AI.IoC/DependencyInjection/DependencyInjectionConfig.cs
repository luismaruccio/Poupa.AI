using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Poupa.AI.Application.DTOs.Users;
using Poupa.AI.Application.Interfaces.Services;
using Poupa.AI.Application.Services;
using Poupa.AI.Application.Validators.Users;
using Poupa.AI.Domain.Interfaces.Repositories;
using Poupa.AI.Infra.Data;
using Poupa.AI.Infra.Repositories;

namespace Poupa.AI.IoC.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddDbContext<PoupaAIDbContext>(options =>
                options.UseNpgsql(GetDatabaseConnectionString(),
                    b => b.MigrationsAssembly("Poupa.AI.Infra")));

            //Validators
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();

            //Services
            services.AddScoped<IHashService, HashService>();
            services.AddScoped<IUserService, UserService>();

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        private static string? GetDatabaseConnectionString()
        {
            var envConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
            return envConnectionString;
        }
    }
}
