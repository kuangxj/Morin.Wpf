using Morin.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morin.Wpf.Adapters;

public interface ISourceProtocolAdapter
{
    /// <summary>
    ///  得到各线路下的剧集
    /// <para>结果：[线路:剧集]</para>
    /// <para>输入：[线路:剧集]</para>
    /// </summary>
    /// <param name="url"></param>
    /// <param name="from"></param>
    /// <param name="lineSpitStr"></param>
    /// <param name="lineAndEspodeSpitChar"></param>
    /// <param name="espodeSpitChar"></param>
    /// <returns></returns>
    Dictionary<string, IEnumerable<VideoModel>> GetPlayDict(string url, string from, string lineSpitStr = "$$$", char lineAndEspodeSpitChar = '#', char espodeSpitChar = '$');
}
