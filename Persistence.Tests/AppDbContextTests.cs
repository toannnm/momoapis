using Domain.Entities;
using Domain.Tests;

namespace Persistence.Tests
{
    public class AppDbContextTests : SetupTest
    {
        [Fact]
        public async Task GetUserById_ReturnsCorrectUser()
        {
            var userId = Guid.NewGuid();
            await _context.User.AddAsync(new User { Id = userId, FullName = "Toan", Email = "hello", Password = "1", Username = "hihi", Phone = "2" });
            await _context.SaveChangesAsync();

            var user = await _context.User.FindAsync(userId);
            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);
        }
        [Fact]
        public async Task UpdateUser_ChangesAreSaved()
        {
            var user = new User { Id = Guid.NewGuid(), FullName = "Toan", Email = "hello", Password = "1", Username = "hihi", Phone = "2" };
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            user.FullName = "Updated Name";
            _context.User.Update(user);
            await _context.SaveChangesAsync();

            var updatedUser = await _context.User.FindAsync(user.Id);
            Assert.Equal("Updated Name", updatedUser.FullName);
        }
        [Fact]
        public async Task DeleteUser_UserIsRemoved()
        {
            var user = new User { Id = Guid.NewGuid(), FullName = "Toan", Email = "hello", Password = "1", Username = "hihi", Phone = "2" };
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            var deletedUser = await _context.User.FindAsync(user.Id);
            Assert.Null(deletedUser);
        }


    }
}
