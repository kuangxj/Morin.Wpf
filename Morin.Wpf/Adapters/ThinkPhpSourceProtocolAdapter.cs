using AutoMapper;
using FlyleafLib.MediaPlayer;
using Morin.Shared.Models;
using Morin.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.Wpf.Adapters;
/// <summary>
/// 适配ThinkPhp 接口协议
/// </summary>
public class ThinkPhpSourceProtocolAdapter(IMapper mapper) : ISourceProtocolAdapter
{
    private readonly IMapper mapper = mapper;


    public Dictionary<string, IEnumerable<VideoModel>> GetPlayDict(ThinkPhpVideoParsingPara para)
    {
        Dictionary<string, IEnumerable<VideoModel>> linesAndEspodes = [];

        var lines = !string.IsNullOrEmpty(para.VodPlayUrl) ? para.VodPlayUrl.Split(para.LineSpitStr) : [];
        var lineNames = !string.IsNullOrEmpty(para.VodPlayFrom) ? para.VodPlayFrom.Split(para.LineSpitStr) : [];

        //  分割线路
        for (var i = 0; i < lineNames.Length; i++)
        {
            var espodeList = lines[i].Split(para.LineAndEspodeSpitChar);
            var videoList = new List<VideoModel>();

            //  排序计数
            var curCount = 0;

            //  分割剧集
            foreach (var espodeItem in espodeList)
            {
                var newVideo = mapper.Map<VideoModel>(para);
                //  排序使用
                newVideo.Sort = curCount++;

                //  分割剧集，例子："第一集$https://111.com/11.m3u8"
                var episodeAndUrl = espodeItem.Split(para.EspodeSpitChar);
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
