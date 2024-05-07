using Morin.Shared.Models;
using Morin.Shared.Parameters;
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
    /// <param name="para"></param>
    /// <returns></returns>
    Dictionary<string, IEnumerable<VideoModel>> GetPlayDict(ThinkPhpVideoParsingPara para);
}
