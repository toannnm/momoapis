//using Application.Interfaces.IServices;
//using Application.Services;
//using Domain.Tests;

//namespace Application.Tests
//{
//    public class DocumentServiceTests : SetupTest
//    {
//        private readonly IDocumentService _documentService;

//        public DocumentServiceTests() => _documentService = new DocumentService(_unitOfWorkMock.Object, _mapperMock.Object, _claimServiceMock.Object, _uploadServiceMock.Object);

//        [Fact]
//        public async Task GetDocumentsAsync_PaginationUserModel_WhenUsersExist()
//        {
//            // Arrange
//            var documentService = new DocumentService();

//            // Act
//            var result = documentService.GetDocument();

//            // Assert
//            Assert.NotNull(result);
//        }
//    }
//}
