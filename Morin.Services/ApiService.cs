
using AutoMapper;
using Morin.Shared.Common;
using Morin.Shared.Models;
using Morin.Shared.Parameters;
using Morin.Storages;

namespace Morin.Services;


public class ApiService(IMapper mapper, IAppStorage appStorage) : IApiService
{
    private readonly IMapper mapper = mapper;
    private readonly IAppStorage appStorage = appStorage;

    public async Task<VideoModel> ReqQryVideoDetailsAsync(ReqQryVideoDetailPara req)
    {
        var uri = ToUri(req);
        var content = await HttpsProvider.GetAsync(uri);
        if (!string.IsNullOrEmpty(content))
        {
            var jsonModel = JsonProvider.FromContentToObject<RspVideoDetailJsonDataModel>(content);
            if (jsonModel != null)
            {
                var resault = mapper.Map<VideoModel>(jsonModel.Videos[0]);
                //  赋来源ID
                resault.SourceID = req.SourceID;
                return resault;
            }
        }
        return new VideoModel();
    }

    public async Task<RspQryVideoModel> ReqQryVideosAsync(ReqQryVideoPara req)
    {
        var uri = ToUri(req);
        var content = await HttpsProvider.GetAsync(uri);
        if (!string.IsNullOrEmpty(content))
        {
            var jsonModel = JsonProvider.FromContentToObject<RspVideoListJsonDataModel>(content);
            if (jsonModel != null)
            {
                var resault = mapper.Map<RspQryVideoModel>(jsonModel);
                return resault;
            }
        }
        return new RspQryVideoModel();
    }

    /// <summary>
    /// 参数转网址
    /// <para>ac=list</para>
    /// <para>t=类别ID</para>
    /// <para>pg=页码</para>
    /// <para>wd=搜索关键字</para>
    /// <para>h=几小时内的数据</para>
    /// </summary>
    /// <param name="req"></param>

    /// <returns></returns>
    private string ToUri(Parameter req)
    {
        ArgumentNullException.ThrowIfNull(req);

        if (req.SourceID < 0) return "";

        if (appStorage.MediaSourceDict.TryGetValue(req.SourceID, out var baseMediaSource))
        {

            if (baseMediaSource == null) return "";

            var apiString = $"api.php/provide/vod/?ac={req.AcName}";

            //  优先Json
            var baseUri = baseMediaSource.JsonUri ?? baseMediaSource.XmlUri;

            var subUri = new Uri(baseUri);

            var para = "";

            if (!string.IsNullOrEmpty(req.KeyWord))
            {
                para += $"&wd={req.KeyWord}";
            }
            if (req.ClassID > 0)
            {
                para += $"&t={req.ClassID}";
            }
            if (!string.IsNullOrEmpty(req.VodIds))
            {
                para += $"&ids={req.VodIds}";
            }
            if (req.Hour > 0)
            {
                para += $"&h={req.Hour}";
            }
            if (req.PageIndex > 0)
            {
                para += $"&pg={req.PageIndex}";
            }

            var uriString = $"{subUri.Scheme}://{subUri.Host}/{apiString}{para}";

            return uriString;
        }
        return "";
    }
}