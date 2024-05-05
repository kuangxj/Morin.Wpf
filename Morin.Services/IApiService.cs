
using Morin.Shared.Models;
using Morin.Shared.Parameters;

namespace Morin.Services;

public interface IApiService
{
    Task<RspQryVideoModel> ReqQryVideosAsync(ReqQryVideoPara req);
    Task<VideoModel> ReqQryVideoDetailsAsync(ReqQryVideoDetailPara req);
}