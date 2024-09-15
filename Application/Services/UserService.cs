using Application.Extensions;
using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IServices;
using Application.Interfaces.IUnitOfWork;
using Application.Models.HelperModels;
using Application.Models.UserModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IClaimService _claimService;

        public UserService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IJwtService jwtService,
            IClaimService claimService)
            => (_unitOfWork, _mapper, _jwtService, _claimService)
            = (unitOfWork, mapper, jwtService, claimService);

        public async Task<Response<Pagination<UserModel>>> GetUsersAsync(int pageIndex = 1, int pageSize = 10)
        {
            var data = await _unitOfWork.UserRepository.GetAllAsync(pageIndex, pageSize, x => x.Include(x => x.Orders)!);

            if (data is null || data.Items.Count is 0)
                return new Response<Pagination<UserModel>>("List is empty!", 404);

            var result = _mapper.Map<Pagination<UserModel>>(data);

            return new Response<Pagination<UserModel>>(result);
        }

        public async Task<Response<UserModel>> GetUserByIdAsync(Guid id)
        {
            var data = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (data is null)
                return new Response<UserModel>("Not found user!", 404);

            var result = _mapper.Map<UserModel>(data);
            return new Response<UserModel>(result);
        }

        public async Task<Response<UserModel>> GetUserProfile()
        {
            var currentUserId = _claimService.GetCurrentUserId;
            if (currentUserId is null || currentUserId == Guid.Empty)
                return new Response<UserModel>("Not login yet!", 404);

            var data = await _unitOfWork.UserRepository.GetEntityByCondition(x => x.Id == currentUserId, x => x.Include(x => x.Orders)!);

            if (data is null)
                return new Response<UserModel>("Not found user!", 404);

            var result = _mapper.Map<UserModel>(data);
            return new Response<UserModel>(result);
        }

        public async Task<Response<string>> LoginAsync(LoginModel model)
        {
            var userLogin = await _unitOfWork.UserRepository.GetEntityByCondition(x => x.Email == model.Email);

            if (userLogin is null || !_jwtService.Verify(model.Password, userLogin.Password))
            {
                return new Response<string>("Email or password is incorrect!", 404);
            }

            var result = _jwtService.GenerateToken(userLogin);
            return new Response<string>(result);
        }

        public async Task<Response<UserModel>> RegisterUserAsync(RegisterModel model)
        {
            var userExist = await _unitOfWork.UserRepository.GetEntityByCondition(x => x.Email == model.Email || x.Phone == model.Phone || x.Username == model.Username);
            if (userExist is not null)
            {
                return new Response<UserModel>("User is existed!", 500);
            }

            var user = _mapper.Map<User>(model);
            var salt = _jwtService.Salt();
            user.Password = _jwtService.Hash(user.Password, salt);
            user.Role = RoleEnum.Customer;
            await _unitOfWork.UserRepository.AddEntityAsync(user);

            var isSuccess = await _unitOfWork.SaveChangesAsync();

            return isSuccess > 0
                ? new Response<UserModel>(_mapper.Map<UserModel>(user))
                : new Response<UserModel>("Register user fail!", 400);
        }

        public async Task<Response<UserModel>> DeleteUser(Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (user is null)
                return new Response<UserModel>("Not found user!", 404);

            _unitOfWork.UserRepository.SoftRemove(user);

            var isSuccess = await _unitOfWork.SaveChangesAsync();

            return isSuccess > 0
                ? new Response<UserModel>(_mapper.Map<UserModel>(user))
                : new Response<UserModel>("Delete user fail!", 400);
        }


    }
}

