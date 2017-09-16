using System.Collections.Generic;
using PriceCollector.Model;

namespace PriceCollector.WebAPI.Responses
{
    public class ProductResponse:ApiResponse<Product>
    {
        public IEnumerable<Model.Product> CollectionResult { get; set; }
    }
}
