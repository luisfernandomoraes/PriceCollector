using System.Collections.Generic;
using System.Threading.Tasks;
using PriceCollector.Api.WebAPI.Responses;
using PriceCollector.Model;

namespace PriceCollector.Api.WebAPI.Products
{
    public interface IProductApi
    {
        /// <summary>
        /// Retorna a lista 
        /// </summary>
        /// <returns></returns>
        Task<ProductResponse> GetProductsToCollect(string url);

        Task<bool> HasImage(string url);
    }
}