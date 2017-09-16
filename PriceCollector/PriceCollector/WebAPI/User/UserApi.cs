using System.Net;
using System.Threading.Tasks;
using PriceCollector.Api.WebAPI.Responses;

namespace PriceCollector.Api.WebAPI.User
{
    public class UserApi:IUserApi
    {
        public Task<UserResponse> Login(string userName, string password)
        {
            return Task.Run(() => new UserResponse() {Success = true,HttpStatusCode = HttpStatusCode.OK});
        }
    }
}