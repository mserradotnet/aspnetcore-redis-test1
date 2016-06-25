using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace RedisWebApi
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            var config = new ConfigurationOptions();
            config.EndPoints.Add("localhost", 6379);
            var db = ConnectionMultiplexer.Connect(config).GetDatabase(0);
            services.AddSingleton<IDatabase>(db);
            return services;
        }
    }
}
