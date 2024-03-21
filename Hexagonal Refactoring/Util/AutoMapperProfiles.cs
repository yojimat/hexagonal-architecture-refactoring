using AutoMapper;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Models.DbModels;

namespace Hexagonal_Refactoring.Util;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Partner, DbPartner>();
        CreateMap<Ticket, DbTicket>();
    }
}