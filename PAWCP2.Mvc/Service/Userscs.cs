
using PAWCP2.Core;
using PAWCP2.Models;
using PAWCP2.Repositories;

namespace PAWCP2.Mvc.Service
{
    public class UserService
    {
        private readonly RepositoryUser _repo;

        public UserService(RepositoryUser repo)
        {
            _repo = repo;
        }

        public async Task UpdateUserAsync(Users user)
        {
            await _repo.UpdateAsync(user);
        }
    }
}
