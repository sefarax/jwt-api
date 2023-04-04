using AuthorizationAPI.Models;
using System.Collections.Generic;

namespace AuthorizationAPI.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>
        {
            new User { Username = "user1", Password = "password1" },
            new User { Username = "user2", Password = "password2" }
        };

        public User GetUser(string username, string password)
        {
            return _users.Find(u => u.Username == username && u.Password == password);
        }
    }
}
