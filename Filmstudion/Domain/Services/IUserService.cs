
using Filmstudion.Models.User;
using System.Collections.Generic;

namespace Filmstudion.Services
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
    }
}
