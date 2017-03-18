using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PriceCollector.Api.WebAPI.Responses;
using PriceCollector.Model;

namespace PriceCollector.Api.WebAPI.SupermarketCompetitors
{
    public class SupermarketCompetitorApi : ISupermarketCompetitorApi
    {
        readonly HttpClient _client;

        public SupermarketCompetitorApi()
        {
            _client = new HttpClient
            {
                MaxResponseContentBufferSize = 1000000 * 10 //10mb
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));
        }
        public async Task<SupermarketResponse> Post(string url, SupermarketsCompetitors supermarkets)
        {
            SupermarketResponse supermarketResponse = new SupermarketResponse();
            supermarketResponse.Success = true;
            return supermarketResponse;
            //For demonstrarion propouse.

            try
            {

                var resourcePath = "/supermarkets-competitors";
                var json = JsonConvert.SerializeObject(supermarkets, Formatting.None);
                var uri = new Uri(string.Concat(url, resourcePath));
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {

                    var response = await _client.PostAsync(uri, content);

                    supermarketResponse.Success = response.IsSuccessStatusCode;
                    if (response.IsSuccessStatusCode)
                    {

                        var contentResponsed = await response.Content.ReadAsStringAsync();
                        supermarketResponse.HttpStatusCode = HttpStatusCode.OK;
                        supermarketResponse.Result = JsonConvert.DeserializeObject<Model.SupermarketsCompetitors>(contentResponsed);
                    }
                    supermarketResponse.HttpStatusCode = response.StatusCode;
                    supermarketResponse.ErrorMessage = response.ReasonPhrase;
                    return supermarketResponse;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return supermarketResponse;
            }
        }

        public async Task<SupermarketResponse> Put(string url, SupermarketsCompetitors supermarkets)
        {
            SupermarketResponse supermarketResponse = new SupermarketResponse();
            supermarketResponse.Success = true;
            return supermarketResponse;
            try
            {
                
                var json = JsonConvert.SerializeObject(supermarkets, Formatting.None);
                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var urlFormated = $"{url}Clients/{supermarkets.IDSupermarket}";
                    var result = await _client.PutAsync(urlFormated, content);
                    supermarketResponse.Success = result.IsSuccessStatusCode;
                    return supermarketResponse;
                }
            }
            catch (Exception)
            {
                return supermarketResponse;
            }
        }
    }
}