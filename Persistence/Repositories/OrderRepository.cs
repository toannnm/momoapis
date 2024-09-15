using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IRepositories;
using Domain.Entities;

namespace Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context, IClaimService claimService) : base(context, claimService)
        {
        }
    }
}
