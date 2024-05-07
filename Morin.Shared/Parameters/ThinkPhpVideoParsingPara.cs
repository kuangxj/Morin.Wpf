
using Morin.Shared.Models;

namespace Morin.Shared.Parameters;

public class ThinkPhpVideoParsingPara:VideoModel
{
    public string? LineSpitStr { get; set; } = "$$$";
    public char LineAndEspodeSpitChar { get; set; } = '#';
    public char EspodeSpitChar { get; set; } = '$';
}
