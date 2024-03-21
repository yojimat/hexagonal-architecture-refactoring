using Hexagonal_Refactoring.Application.Repositories;
using Hexagonal_Refactoring.Application.UseCases.Customer;
using Hexagonal_Refactoring.Application.UseCases.Event;
using Hexagonal_Refactoring.Application.UseCases.Partner;
using Hexagonal_Refactoring.Infrastructure.Repositories;

namespace Hexagonal_Refactoring.Util;

public static class DependencyInjectionManager
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IPartnerRepository>();
        //services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ITicketRepository>();
        //services.AddScoped<ITicketRepository, TicketRepository>();
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