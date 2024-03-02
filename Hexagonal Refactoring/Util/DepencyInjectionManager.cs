using Hexagonal_Refactoring.Application.UseCases;
using Hexagonal_Refactoring.Repositories;
using Hexagonal_Refactoring.Services;

namespace Hexagonal_Refactoring.Util;

public static class DependencyInjectionManager
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IPartnerService, PartnerService>();
        services.AddScoped<IEventService, EventService>();
    }

    public static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<CreateCustomerUseCase>();
        services.AddScoped<GetCustomerByIdUseCase>();
        services.AddScoped<CreatePartnerUseCase>();
        services.AddScoped<GetPartnerByIdUseCase>();
        services.AddScoped<CreateEventUseCase>();
        services.AddScoped<SubscribeCustomerToEventUseCase>();
    }
}