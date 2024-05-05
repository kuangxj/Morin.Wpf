using AutoMapper;
using Morin.Shared.Models;

namespace Morin.Wpf.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        CreateMap<VideoModel, RspXmlVideoModel>().ReverseMap();
        CreateMap<VideoModel, RspJsonVideoModel>().ReverseMap();

        CreateMap<ClassModel, RspJsonClassModel>().ReverseMap();


        CreateMap<RspQryVideoModel, RspVideoListJsonDataModel>().ReverseMap();
        CreateMap<RspQryVideoModel, RspXmlDataModel>().ReverseMap();
    }
}
