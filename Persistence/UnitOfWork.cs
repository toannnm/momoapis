using Application.Interfaces.IRepositories;
using Application.Interfaces.IUnitOfWork;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IOrderRepository _orderRepository;

        public UnitOfWork(AppDbContext context, IUserRepository userRepository, IDocumentRepository documentRepository, IOrderRepository orderRepository)
            => (_context, _userRepository, _documentRepository, _orderRepository) = (context, userRepository, documentRepository, orderRepository);

        public IUserRepository UserRepository => _userRepository;

        public IDocumentRepository DocumentRepository => _documentRepository;
        public IOrderRepository OrderRepository => _orderRepository;

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
