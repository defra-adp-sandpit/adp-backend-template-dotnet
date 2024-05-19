using ADP.Backend.Template.Api.Models;
using ADP.Backend.Template.Core.Entities;

using AutoMapper;

namespace ADP.Backend.Template.Api.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ItemRequest, Item>();
    }
}
