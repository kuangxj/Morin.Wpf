using AutoMapper;
using Morin.Shared.Models;
using Morin.Shared.Parameters;

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

        CreateMap<VideoModel, FavoriteModel>().ReverseMap();
        CreateMap<VideoModel, HistoryViewsModel>().ReverseMap();

        CreateMap<VideoModel, ThinkPhpVideoParsingPara>().ReverseMap();
        
    }
}
