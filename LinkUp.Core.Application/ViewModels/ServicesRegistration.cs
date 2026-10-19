using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LinkUp.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ServicesRegistration).Assembly);

            // Servicio genérico
            services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));

            // Servicios específicos
            services.AddScoped<IFriendshipService, FriendshipService>();
            services.AddScoped<IPlayerShotService, PlayerShotService>();
            services.AddScoped<IPostService, PostService>();       // ✅
            services.AddScoped<IGameService, GameService>();       // ✅
            services.AddScoped<IShipService, ShipService>();       // ✅ si existe
        }
    }
}