using AuthorizationAPI.Models;

namespace AuthorizationAPI.Services
{
    public interface IUserService
    {
        User GetUser(string username, string password);
    }
}
