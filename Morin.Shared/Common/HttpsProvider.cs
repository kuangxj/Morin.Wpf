#if DEBUG
using System.Diagnostics;
#endif

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

namespace Morin.Shared.Common;

public class HttpsProvider
{
    private static readonly HttpClientHandler httpClientHandler = new()
    {
        AutomaticDecompression = DecompressionMethods.GZip,
        ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true  //忽略掉证书异常
    };
    private static readonly HttpClient _httpClient = new(httpClientHandler)
    {
        //  60秒超时
        Timeout = TimeSpan.FromMinutes(1)
    };
    public static async Task<string?> GetAsync(string url, string token = "")
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        try
        {
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            //返回byte,转换成utf8,防止报错
            var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var str = Encoding.UTF8.GetString(bytes);

            return str;
            //return await response.Content.ReadAsStringAsync().ConfigureAwait(false);//直接返回json,有时候会报utf8不支持的错误
        }
        catch (InvalidOperationException ex)
        {
#if DEBUG
            Debug.WriteLine(ex.Message);
#endif
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
        _httpClient.DefaultRequestHeaders.ConnectionClose = true;
        var jsonContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync(Url, jsonContent).Result.Content.ReadAsStringAsync();
    }

    private static void AddDefaultHeaders(HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Add("x-www-foo", "123");
        httpClient.DefaultRequestHeaders.Add("x-www-bar", "456");
        httpClient.DefaultRequestHeaders.Add("x-www-baz", "789");
    }
}
