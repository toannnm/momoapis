using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IRepositories;
using Domain.Entities;

namespace Persistence.Repositories
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(AppDbContext context, IClaimService claimService) : base(context, claimService)
        {
        }
    }
}
