using AutoMapper;
using backend.DTO;
using backend.Models;

namespace backend.Profiles;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDetailDTO>();
        CreateMap<User, UserWithRolesDTO>()
                    .ForMember(des => des.Roles, opt => opt.MapFrom(src => src.Roles));

        CreateMap<AssetCreateDTO, Asset>();
        CreateMap<Asset, AssetInfoDTO>();
        
        CreateMap<Usage, UsageDetailDTO>();
        CreateMap<UsageCreateDTO, Usage>();

        CreateMap<RoleCreateDTO, Role>()
                    .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name));
        CreateMap<Role, RoleInfoDTO>();
    }
}