using FlyleafLib.MediaFramework.MediaPlaylist;
using Morin.Shared.Models;

namespace Morin.Wpf.Common;

public class VideoUriToEspode
{
    public static Dictionary<string, List<VideoModel>> FromUrlToEspodes(VideoModel video)
    {
        Dictionary<string, List<VideoModel>> LinesAndEspodes = [];
        var spitStr = video.VodPlayNote ?? "$$$";
        var lines = video.VodPlayUrl.Split(spitStr);
        var lineNames = video.VodPlayFrom.Split(spitStr);
        for (var i = 0; i < lineNames.Length; i++)
        {
            var espodeList = lines[i].Split('#');
            var videoList = new List<VideoModel>();
            var curCount = 0;
            foreach (var espodeItem in espodeList)
            {
                //  复制并修改部分信息
                var entity = (VideoModel)video.Clone();
                //  排序使用
                entity.Sort = curCount++;
                var episodeAndUrl = espodeItem.Split('$');
                if (episodeAndUrl.Length > 1)
                {
                    entity.Episode = episodeAndUrl[0];
                    entity.VodPlayUrl = episodeAndUrl[1];
                    videoList.Add(entity);
                }
            }
            var key = lineNames[i];
            if (!LinesAndEspodes.ContainsKey(key))
            {
                LinesAndEspodes.Add(key, videoList);
            }
            else
            {
                LinesAndEspodes[key] = videoList;
            }
        }
        return LinesAndEspodes;
    }
}

