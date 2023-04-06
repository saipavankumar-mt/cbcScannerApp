using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace cbc_testapp.Services
{
    public class WebAPIService : IWebApiService
    {

        public string WebAPIUrl = "http://10.0.2.2:44314/api/";

        public async Task<Trs> Post<TRq, Trs>(TRq request, string relativeUrl) where Trs : class
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WebAPIUrl);
                    if (string.IsNullOrEmpty(App.UserSessionId) == false)
                    {
                        client.DefaultRequestHeaders.Add("session-key", App.UserSessionId);
                        client.Timeout = new TimeSpan(0, 0, 15);
                    }
                    var result = await client.PostAsJsonAsync<TRq>(relativeUrl, request);

                    if (result.IsSuccessStatusCode)
                    {

                        Trs response = await result.Content.ReadFromJsonAsync<Trs>();

                        return response;

                    }
                }
            }
            catch (Exception ex)
            {
                //Suppress Ex;
            }
            return null;

        }

        public async Task<Trs> Get<Trs>(string request) where Trs : class
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(WebAPIUrl);
                    if (string.IsNullOrEmpty(App.UserSessionId) == false)
                    {
                        client.DefaultRequestHeaders.Add("session-key", App.UserSessionId);
                    }
                    client.Timeout = new TimeSpan(0, 0, 15);
                    //HTTP GET
                    var result =await client.GetAsync(request);

                    if (result.IsSuccessStatusCode)
                    {

                        var readTask = result.Content.ReadFromJsonAsync<Trs>();
                        readTask.Wait();

                        Trs response = readTask.Result;

                        return response;

                    }
                }
            }
            catch (Exception ex)
            {
                //Suppress
            }
            return null;
        }
    }
}
