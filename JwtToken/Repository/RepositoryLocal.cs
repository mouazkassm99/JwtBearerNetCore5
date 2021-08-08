using System.Collections.Generic;
using System.Linq;
using JwtToken.Data;

namespace JwtToken.Repository
{
    public class RepositoryLocal : IRepository
    {
        public List<UserModel> Users { get; set; }

        public RepositoryLocal()
        {
            Users = new List<UserModel>();
            Users.Add(new UserModel{UserName = "Mouaz", Password = "12345678"});
        }


        public UserModel GetUser(string username)
        {
            return Users.Find(u => u.UserName == username);
        }

        public List<UserModel> GetAllUsers()
        {
            return Users;
        }
    }
}