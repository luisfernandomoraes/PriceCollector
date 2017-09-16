using System.Threading.Tasks;

namespace PriceCollector.WebAPI.SupermarketCompetitors
{
    public interface ISupermarketCompetitorApi
    {
        Task<Responses.SupermarketResponse> Post(string url, Model.SupermarketsCompetitors supermarkets);
        Task<Responses.SupermarketResponse> Put(string url, Model.SupermarketsCompetitors supermarkets);
    }
}