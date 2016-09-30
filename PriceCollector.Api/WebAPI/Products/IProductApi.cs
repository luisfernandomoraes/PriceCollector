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

        /// <summary>
        /// Retorna um produto a partir do codigo de barras.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        Task<Responses.ProductResponse> GetProduct(string url, string barcode);


        /// <summary>
        /// Verifica se o produto tem imagem.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<bool> HasImage(string url);
    }
}