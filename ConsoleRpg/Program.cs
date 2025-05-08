using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleRpg;

public static class Program
{
 
    private static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        Startup.ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var gameEngine = serviceProvider.GetRequiredService<GameEngine>();
        gameEngine?.Run();
    }
}


