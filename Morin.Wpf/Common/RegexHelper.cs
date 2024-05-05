using System.Text.RegularExpressions;

namespace Morin.Wpf.Common
{
    public static partial class RegexInfo
    {
        [GeneratedRegex("[0-9]+", RegexOptions.IgnoreCase, "zh-CN")]
        public static partial Regex RegexNumber();

        [GeneratedRegex("[\u4e00-\u9fa5]+", RegexOptions.IgnoreCase, "zh-CN")]
        public static partial Regex RegexNotNumber();
        /// <summary>
        /// 匹配模式：中国电影09.mp4
        /// 结果：gourp[0]=中国电影,gourp[1]=09,gourp[2]=mp4
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex("(.+)(\\d+)\\.(.+)", RegexOptions.IgnoreCase, "zh-CN")]
        public static partial Regex RegexFirstChinaLastNumber();

    }
}
