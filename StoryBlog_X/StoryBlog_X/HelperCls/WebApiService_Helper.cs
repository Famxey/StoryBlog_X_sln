using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using StoryBlog_X.Exceptions;
using Newtonsoft.Json;
using System.IO;

namespace StoryBlog_X.HelperCls
{
    public class WebApiService_Helper
    {
        //192.168.123.165 localhost
        public static readonly string HttpBaseAddress = "http://localhost:888";

        protected string _baseUrl = "http://www.baidu.com";

        protected HttpClient GetClient()
        {
            return GetClient(_baseUrl);
        }
        protected virtual HttpClient GetClient(string baseUrl)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            return client;
        }

        public static async Task<T> GetConnectHelperAsync<T>(string url)
        {
            WebApiService_Helper api = new WebApiService_Helper();
            using (HttpClient client = api.GetClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(HttpBaseAddress + url);
                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsAsync<TrackSeriesApiError>();
                        var message = error != null ? error.Message : "";
                        throw new TrackSeriesApiException(message, response.StatusCode);
                    }
                    string resultStr = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(resultStr);
                }
                catch (HttpRequestException ex)
                {
                    throw new TrackSeriesApiException("", false, ex);
                }
                catch (UnsupportedMediaTypeException ex)
                {
                    throw new TrackSeriesApiException("", false, ex);
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }

        }

        public static async Task<Uri> CreateConnectHelperAsync<T>(string url, T cls)
        {
            WebApiService_Helper api = new WebApiService_Helper();

            using (HttpClient client = api.GetClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync<T>(
                                   url, cls);
                    response.EnsureSuccessStatusCode();

                    return response.Headers.Location;
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        public static async Task<HttpStatusCode> DeleteConnectHelperAsync(string url)
        {
            WebApiService_Helper api = new WebApiService_Helper();

            using (HttpClient client = api.GetClient())
            {
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync(
                                    url);
                    return response.StatusCode;
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        public static async Task<T> UpdateConnectHelperAsync<T>(string url, T cls)
        {
            WebApiService_Helper api = new WebApiService_Helper();

            using (HttpClient client = api.GetClient())
            {

                try
                {
                    HttpResponseMessage response = await client.PutAsJsonAsync(
                                    url, cls);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsAsync<T>();
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }

        /// <summary>
        /// 第二种Get查询信息的写法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<T> GetConnectHelperAsync2<T>(string url)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(HttpBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {

                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                client.Dispose();
            }
        }

        /// <summary>
        /// 以的方式Post查询信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public static async Task<T> PostConnectHelperAsync<T>(string url, T cls)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(HttpBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {

                HttpResponseMessage response = await client.PostAsJsonAsync(url, cls);

                if (response.IsSuccessStatusCode)
                {

                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                client.Dispose();
            }
        }

        public static async Task<string> PostUpLoadImageHelperAsync(string url, Stream stream)
        {

            //上传图片到服务器
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(HttpBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            #region
            MultipartFormDataContent form = new MultipartFormDataContent();
            StreamContent fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            fileContent.Headers.ContentDisposition.FileName = OptionText_Helper.ReadAllText("Account") + ".jpeg";
            form.Add(fileContent);
            #endregion

            try
            {

                HttpResponseMessage response = await client.PostAsync(url, form);

                if (response.IsSuccessStatusCode)
                {

                    return await response.Content.ReadAsAsync<string>();
                }
                else
                {
                    return "no";
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                client.Dispose();
            }

        }

        public static async Task<string> PostConnectHelperAsync(string url, string str)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(HttpBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {

                HttpResponseMessage response = await client.PostAsJsonAsync(url, str);

                if (response.IsSuccessStatusCode)
                {

                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return "no";
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                client.Dispose();
            }
        }

    }
}
