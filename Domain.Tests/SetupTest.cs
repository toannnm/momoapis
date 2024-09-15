using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Interfaces.IUnitOfWork;
using AutoFixture;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;

namespace Domain.Tests
{
    public class SetupTest : IDisposable
    {
        protected readonly Fixture _fixture;
        protected readonly Mock<IMapper> _mapperMock;

        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly AppDbContext _context;
        protected readonly Mock<AppDbContext> _mockContext;
        protected readonly Mock<IUserRepository> _userRepositoryMock;
        protected readonly Mock<IDocumentRepository> _documentRepositoryMock;

        protected readonly Mock<IUserService> _userServiceMock;
        protected readonly Mock<IDocumentService> _documentServiceMock;

        protected readonly Mock<IClaimService> _claimServiceMock;
        protected readonly Mock<IJwtService> _jwtServiceMock;
        protected readonly Mock<IUploadImageService> _uploadServiceMock;

        public SetupTest()
        {
            _fixture = new Fixture();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _documentRepositoryMock = new Mock<IDocumentRepository>();

            _userServiceMock = new Mock<IUserService>();
            _documentServiceMock = new Mock<IDocumentService>();

            _claimServiceMock = new Mock<IClaimService>();
            _jwtServiceMock = new Mock<IJwtService>();
            _uploadServiceMock = new Mock<IUploadImageService>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ToanNguyenManh".ToString())
            .Options;
            _context = new AppDbContext(options);
        }

        public void Dispose() => _context.Dispose();

    }
}
