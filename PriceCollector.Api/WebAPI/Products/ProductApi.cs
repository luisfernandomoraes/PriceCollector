using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PriceCollector.Api.WebAPI.Responses;
using PriceCollector.Model;


namespace PriceCollector.Api.WebAPI.Products
{
    public class ProductApi:IProductApi
    {
        HttpClient _client;
        private List<string> _barCodeListDemo;

        public ProductApi()
        {
            _client = new HttpClient
            {
                MaxResponseContentBufferSize = 1000000 * 10 //10mb
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _barCodeListDemo = new List<string>()
            {
                "7891149103102",
                "7891149101900",
                "7891000315200",
                "7891010560737",
                "7894900011517",
                "7896019201117",
                "7891172432064",
                "7894900010015",
                "7896036094969"
            };
        }

        public async Task<ProductResponse> GetProductsToCollect(string url)
        {

            var result = new ProductResponse();
            var products = new List<Product>();
            var qrcode = "Acats01_36256202000146";
            var matrixdb = "Acats01";

            try
            {

                foreach (var barcode in _barCodeListDemo)
                {
                    
                    var resourcePath = $"products/{barcode}/{qrcode}/{matrixdb}";
                    var uri = new Uri(string.Concat(url, resourcePath));

                    var response = await _client.GetAsync(uri);

                    result.Success = response.IsSuccessStatusCode;
                    if (!response.IsSuccessStatusCode) continue;


                    var content = await response.Content.ReadAsStringAsync();
                    result.HttpStatusCode = HttpStatusCode.OK;
                    var obj = JObject.Parse(content);
                    var product = new Product
                    {
                        ID = Convert.ToInt32(obj["IDProduct"].ToString()),
                        Name = obj["ProductName"].ToString(),
                        BarCode = obj["Barcod"].ToString(),
                        PriceCurrent = Convert.ToDecimal(obj["Value"].ToString())
                    };

                    products.Add(product);
                }

                result.CollectionResult = products;
                return result;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return result;
            }
        }

        public async Task<bool> HasImage(string url)
        {
            const bool failResult = false;
            try
            {
                var uri = new HttpRequestMessage(HttpMethod.Head, url);

                var response = await _client.SendAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return failResult;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return failResult;
            }
        }
    }
}