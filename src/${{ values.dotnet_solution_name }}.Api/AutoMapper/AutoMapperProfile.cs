using ${{ values.dotnet_solution_name }}.Api.Models;
using ${{ values.dotnet_solution_name }}.Core.Entities;

using AutoMapper;

namespace ${{ values.dotnet_solution_name }}.Api.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ItemRequest, Item>();
    }
}
