using Application.Interfaces.IRepositories;

namespace Application.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IDocumentRepository DocumentRepository { get; }
        public IOrderRepository OrderRepository { get; }

        Task<int> SaveChangesAsync();

    }
}
