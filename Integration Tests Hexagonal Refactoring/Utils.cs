using Hexagonal_Refactoring.Infrastructure.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace Integration_Tests_Hexagonal_Refactoring;

public static class Utils
{
    public static async Task ResetDb(WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EventContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}