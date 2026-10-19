using LinkUp.Infrastructure.Persistence.Contexts;
using LinkUp.Infrastructure.Persistence.Repositories;
using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkUp.Infrastructure.Persistence  // ✅ NAMESPACE CORRECTO
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(connectionString,
                    m => m.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            // Repositorio Genérico
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Repositorios específicos de Red Social
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IReactionRepository, ReactionRepository>();
            services.AddScoped<IFriendshipRepository, FriendshipRepository>();

            // Repositorios específicos de Battleship
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IShipRepository, ShipRepository>();
            services.AddScoped<IPlayerShotRepository, PlayerShotRepository>();
        }
    }
}