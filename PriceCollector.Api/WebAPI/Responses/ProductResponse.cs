using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceCollector.Model;

namespace PriceCollector.Api.WebAPI.Responses
{
    public class ProductResponse:ApiResponse<Product>
    {
        public IEnumerable<Model.Product> CollectionResult { get; set; }
    }
}
