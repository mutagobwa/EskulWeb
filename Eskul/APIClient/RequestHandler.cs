using Eskul.Models;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Nancy;
using Nancy.Json;
using Nancy.Session;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Data;
using System.Drawing;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using JsonSerializer = Microsoft.ApplicationInsights.Extensibility.Implementation.JsonSerializer;

namespace Eskul.APIClient
{
    public class RequestHandler
    {
        private static string Url = "";
        RequestHandler request;
        
        private readonly IConfiguration configuration;
        

        public RequestHandler(IConfiguration configuration)
        {
           
            this.configuration = configuration;
        
        }
       
        public async Task<string> Add<T>(T obj, string relativeUrl) where T : class
        {
            string APIUrl = configuration["Appsettings:Base_Url"];
            string resp = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(APIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var dataAsString = JsonConvert.SerializeObject(obj);
                var dataContent = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(APIUrl + relativeUrl, dataContent);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    ApiResponse resp1 = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                    return resp1.ResponseMessage;
                }
                else
                {
                    resp = response.StatusCode.ToString();
                }
            }
            return resp;
        }
        public async Task<ApiResponse> AddAsync<T>(T obj, string relativeUrl) where T : class
        {
            string APIUrl = configuration["Appsettings:Base_Url"];
            ApiResponse resp1 = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(APIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var dataAsString = JsonConvert.SerializeObject(obj);
                var dataContent = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(APIUrl + relativeUrl, dataContent);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    resp1 = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                    return resp1;
                }
                else
                {
                    return resp1;
                }
            }
        }
        public async Task<ApiResponse> UpdateAsync<T>(T obj, string relativeUrl) where T : class
        {
            string APIUrl = configuration["Appsettings:Base_Url"];
            ApiResponse? resp1 = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(APIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var dataAsString = JsonConvert.SerializeObject(obj);
                var dataContent = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(APIUrl + relativeUrl, dataContent);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    resp1 = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                    return resp1;
                }
                else
                {
                    return resp1;
                }
            }
        }
        public async Task<string> Update<T>(T obj, string relativeUrl) where T : class
        {
            string APIUrl = configuration["Appsettings:Base_Url"];
            string resp = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(APIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var dataAsString = JsonConvert.SerializeObject(obj); 
                var dataContent = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(APIUrl + relativeUrl, dataContent);
                
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();


                    ApiResponse resp1 = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                    if (resp1.PayLoad!=null)
                    {
                        var jsonObject = JsonNode.Parse(resp1.PayLoad.ToString());
                        resp = jsonObject[0]["RESPONSEMESSAGE"].ToString();

                        Uri A2wUrl = response.Headers.Location;
                        //resp = "successfully";
                    }
                    else
                    {
                        resp = resp1.ResponseMessage;
                    }
                }
                else
                {

                    resp = response.StatusCode.ToString();
                }
            }
            return resp;
        }
        public async Task<List<T>> GetAll<T>(string relativeUrl) where T : class
        {

            object model; //= new Currence();


            using (var httpClient = new HttpClient())
            {

                string APIUrl = configuration["Appsettings:Base_Url"];
                using (var response = await httpClient.GetAsync(APIUrl + relativeUrl))
                {
                    try
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        
                        
                        ApiResponse resp = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                       
                        if (resp==null && apiResponse!="")
                        {
                            ApiResponse resp1 = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                            var jsonObject = JsonNode.Parse(resp.PayLoad.ToString());
                            jsonObject[0]["RESPONSEMESSAGE"].ToString();
                        }
                        else if (resp?.PayLoad!=null)
                        {
                            model = JsonConvert.DeserializeObject(resp.PayLoad, typeof(List<T>));
                            if (model != null)
                                return (List<T>)model;

                        }

                        return null;
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
        }
        public async Task<ApiResponse?> Get(string relativeUrl)
        {
            using (var httpClient = new HttpClient())
            {
                string APIUrl = configuration["Appsettings:Base_Url"];
                using (var response = await httpClient.GetAsync(APIUrl + relativeUrl))
                {
                    try
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        ApiResponse? resp = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);

                        return resp;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }
        public async Task<List<T>> Get<T>(string relativeUrl) where T : class
        {
            object model; //= new Currence();
            using (var httpClient = new HttpClient())
            {
                string APIUrl = configuration["Appsettings:Base_Url"];
                using (  var response = await httpClient.GetAsync(APIUrl + relativeUrl))
                {
                    try
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        ApiResponse resp = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                        if (!resp.Success)
                        {

                        }
                        else
                        {
                            model = JsonConvert.DeserializeObject(resp.PayLoad, typeof(List<T>));
                            if (model != null)
                                return (List<T>)model;

                        }

                        return null;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }
        public async Task<T> GetSingle<T>(string relativeUrl) where T : class
        {
            object model; //= new Currence();
            using (var httpClient = new HttpClient())
            {
                string APIUrl = configuration["Appsettings:Base_Url"];
                using (var response = await httpClient.GetAsync(APIUrl + relativeUrl))
                {
                    try
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        ApiResponse resp = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                        if (!resp.Success)
                        {

                        }
                        else
                        {
                            model = JsonConvert.DeserializeObject(resp.PayLoad, typeof(T));
                            if (model != null)
                                return (T)model;

                        }

                        return null;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }
        public async Task<T> GetSingle0<T>(string relativeUrl) where T : class
        {
            object model; //= new Currence();
            using (var httpClient = new HttpClient())
            {
                string APIUrl = configuration["Appsettings:Base_Url"];
                using (var response = await httpClient.GetAsync(APIUrl + relativeUrl))
                {
                    try
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        ApiResponse resp = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                        if (!resp.Success)
                        {

                        }
                        else
                        {
                            model = JsonConvert.DeserializeObject(resp.PayLoad, typeof(T));
                            if (model != null)
                                return (T)model;

                        }

                        return null;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }
        public async Task<string>GetA(string relativeUrl)
        {
            using (var httpClient = new HttpClient())
            {
                string APIUrl = configuration["Appsettings:Base_Url"];
                using (var response = await httpClient.GetAsync(APIUrl + relativeUrl))
                {
                    try
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        ApiResponse? resp = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                        var jsonObject = JsonNode.Parse(resp.PayLoad.ToString());
                        jsonObject[0]["RESPONSEMESSAGE"].ToString();
                        return jsonObject[0]["RESPONSEMESSAGE"].ToString();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }
        public async Task<string> GetB(string relativeUrl)
        {
            using (var httpClient = new HttpClient())
            {
                string APIUrl = configuration["Appsettings:Base_Url"];
                using (var response = await httpClient.GetAsync(APIUrl + relativeUrl))
                {
                    try
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        ApiResponse? resp = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                        var jsonObject = JsonNode.Parse(resp.PayLoad.ToString());
                        
                        return resp.PayLoad.ToString();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }
  
        public async Task<string> Update(string relativeUrl)
        {
            using (var httpClient = new HttpClient())
            {
                string APIUrl = configuration["Appsettings:Base_Url"];
                using (var response = await httpClient.PostAsync(APIUrl + relativeUrl,null))
                {
                    try
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        ApiResponse? resp = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                        var jsonObject = JsonNode.Parse(resp.PayLoad.ToString());

                        return resp.PayLoad.ToString();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }
 public async Task<ApiResponse> GetAsync(string relativeUrl)
{
    using (var httpClient = new HttpClient())
    {
                httpClient.Timeout = TimeSpan.FromSeconds(1000);
                string APIUrl = configuration["Appsettings:Base_Url"];
        using (var response = await httpClient.GetAsync(APIUrl + relativeUrl))
        {
            try
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                ApiResponse resp = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                return resp;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
        public async Task<ApiResponse> DeleteAsync(string relativeUrl)
        {
            using (var httpClient = new HttpClient())
            {
                string APIUrl = configuration["Appsettings:Base_Url"];
                using (var response = await httpClient.DeleteAsync(APIUrl + relativeUrl))
                {
                    try
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        ApiResponse resp = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                        return resp;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
        public async Task<ApiResponse> UpdateAsync(string relativeUrl)
        {
            using (var httpClient = new HttpClient())
            {
                string APIUrl = configuration["Appsettings:Base_Url"];
                using (var response = await httpClient.DeleteAsync(APIUrl + relativeUrl))
                {
                    try
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
                        ApiResponse resp = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                        return resp;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
