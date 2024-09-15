using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IRepositories;
using Domain.Entities;

namespace Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context, IClaimService claimService) : base(context, claimService)
        {
        }
    }
}
