#if DEBUG
using System.Diagnostics;
#endif

using System.Net;
using System.Text;

namespace Morin.Shared.Common;

public class HttpsProvider
{
    private static readonly HttpClientHandler httpClientHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true  //忽略掉证书异常
    };
    private static readonly HttpClient httpClient = new(httpClientHandler)
    {
        //  60秒超时
        Timeout = TimeSpan.FromMinutes(1)
    };
    public static async Task<string?> GetAsync(string? url)
    {
        try
        {
            if (!string.IsNullOrEmpty(url))
            {
                return await httpClient.GetStringAsync(url);
            }
            return null;
        }
        catch (TaskCanceledException ex)
        {
#if DEBUG
            Debug.WriteLine(ex.Message);
#endif
            return null;
        }
        catch (HttpRequestException ex)
        {
#if DEBUG
            Debug.WriteLine(ex.Message);
#endif
            return null;
        }
    }
    public static async Task<string?> PostAsync(string? Url, string? jsonData)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        httpClient.DefaultRequestHeaders.ConnectionClose = true;
        var jsonContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
        return await httpClient.PostAsync(Url, jsonContent).Result.Content.ReadAsStringAsync();
    }
}
