using Application.Extensions;
using Application.Models.HelperModels;
using Application.Models.UserModels;

namespace Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<Response<UserModel>> DeleteUser(Guid id);
        Task<Response<UserModel>> GetUserByIdAsync(Guid id);
        Task<Response<UserModel>> GetUserProfile();
        Task<Response<Pagination<UserModel>>> GetUsersAsync(int pageIndex = 1, int pageSize = 10);
        Task<Response<string>> LoginAsync(LoginModel model);
        Task<Response<UserModel>> RegisterUserAsync(RegisterModel model);
    }
}