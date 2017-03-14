using System.Threading.Tasks;

namespace PriceCollector.Api.WebAPI.SupermarketCompetitors
{
    public interface ISupermarketCompetitorApi
    {
        Task<Responses.SupermarketResponse> Post(string url, Model.SupermarketsCompetitors supermarkets);
    }
}