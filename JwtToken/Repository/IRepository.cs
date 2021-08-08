using System.Collections.Generic;
using JwtToken.Data;

namespace JwtToken.Repository
{
    public interface IRepository
    {
        UserModel GetUser(string username);
        List<UserModel> GetAllUsers();
    }
}