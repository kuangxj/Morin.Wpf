using FlyleafLib.MediaPlayer;
using Morin.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.Wpf.Adapters;
/// <summary>
/// 适配ThinkPhp 接口协议
/// </summary>
public class ThinkPhpSourceProtocolAdapter : ISourceProtocolAdapter
{
    public Dictionary<string, IEnumerable<VideoModel>> GetPlayDict(string url, string from, string lineSpitStr = "$$$", char lineAndEspodeSpitChar = '#', char espodeSpitChar = '$')
    {
        Dictionary<string, IEnumerable<VideoModel>> linesAndEspodes = [];

        var lines = !string.IsNullOrEmpty(url) ? url.Split(lineSpitStr) : [];
        var lineNames = !string.IsNullOrEmpty(from) ? from.Split(lineSpitStr) : [];

        //  分割线路
        for (var i = 0; i < lineNames.Length; i++)
        {
            var espodeList = lines[i].Split(lineAndEspodeSpitChar);
            var videoList = new List<VideoModel>();

            //  排序计数
            var curCount = 0;

            //  分割剧集
            foreach (var espodeItem in espodeList)
            {               
                var newVideo = new VideoModel
                {
                    //  排序使用
                    Sort = curCount++

                };
                //  分割剧集，例子："第一集$https://111.com/11.m3u8"
                var episodeAndUrl = espodeItem.Split(espodeSpitChar);
                if (episodeAndUrl.Length > 1)
                {
                    //  第一集
                    newVideo.Episode = episodeAndUrl[0];
                    //  https://111.com/11.m3u8
                    newVideo.VodPlayUrl = episodeAndUrl[1];
                    videoList.Add(newVideo);
                }
            }

            //  线程名称，也是字典的KEY
            var key = lineNames[i];
            if (!linesAndEspodes.ContainsKey(key))
            {
                linesAndEspodes.Add(key, videoList);
            }
            else
            {
                linesAndEspodes[key] = videoList;
            }
        }
        return linesAndEspodes;
    }
}
