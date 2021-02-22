using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.AggregateRoots;
using Domain.IRepositories;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private static readonly IList<User> Users = new List<User>();
        
        public async Task AddAsync(User user)
        {
            Users.Add(user);
        }

        public async Task<User> GetAsync(Guid id)
        {
            return Users.FirstOrDefault(p => p.Id == id);
        }

        public async Task<IList<User>> GetAllAsync()
        {
            return Users;
        }
    }
}