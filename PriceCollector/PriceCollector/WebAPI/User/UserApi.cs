using System.Net;
using System.Threading.Tasks;
using PriceCollector.WebAPI.Responses;

namespace PriceCollector.WebAPI.User
{
    public class UserApi:IUserApi
    {
        public Task<UserResponse> Login(string userName, string password)
        {
            return Task.Run(() => new UserResponse() {Success = true,HttpStatusCode = HttpStatusCode.OK});
        }
    }
}