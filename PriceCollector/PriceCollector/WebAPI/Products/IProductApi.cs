using System.Collections.Generic;
using System.Threading.Tasks;
using PriceCollector.WebAPI.Responses;

namespace PriceCollector.WebAPI.Products
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

        /// <summary>
        /// Envia ao servidor os registros coletados.
        /// </summary>
        /// <param name="url">url base do endpoint de syncronização.</param>
        /// <param name="productCollecteds"></param>
        /// <returns></returns>
        Task<bool> PostCollectedProducts(string url, List<Model.ProductCollected> productCollecteds);
    }
}