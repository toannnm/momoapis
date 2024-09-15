using Application.Extensions;
using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IServices;
using Application.Interfaces.IUnitOfWork;
using Application.Models.HelperModels;
using Application.Models.OrderModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimService _claimService;
        private readonly IEmailService _emailService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IClaimService claimService, IEmailService emailService)
        => (_unitOfWork, _mapper, _claimService, _emailService) = (unitOfWork, mapper, claimService, emailService);

        public async Task<Response<Pagination<OrderModel>>> GetOrdersAsync(int pageIndex, int pageSize = 10)
        {
            var data = await _unitOfWork.OrderRepository.GetAllAsync(pageIndex, pageSize, x => x.Include(x => x.OrderDetails!));

            if (data is null || data.Items.Count is 0 || !data.Items.Any())
                return new Response<Pagination<OrderModel>>("List is empty!", 404);

            var result = _mapper.Map<Pagination<OrderModel>>(data);

            return new Response<Pagination<OrderModel>>(result);
        }

        public async Task<Response<OrderModel>> GetOrderByIdAsync(Guid id)
        {
            var data = await _unitOfWork.OrderRepository.GetByIdAsync(id);

            if (data is null)
                return new Response<OrderModel>("Not found order!", 404);

            var result = _mapper.Map<OrderModel>(data);
            return new Response<OrderModel>(result);
        }

        public async Task<Response<OrderModel>> AddOrderAsync(CreateOrderModel model)
        {
            var currentUser = _claimService.GetCurrentUserId;
            if (currentUser is null || currentUser == Guid.Empty)
                return new Response<OrderModel>("Not login yet!", 404);

            if (model.OrderDetails?.Count is 0 || model.OrderDetails is null || !model.OrderDetails.Any())
            {
                return new Response<OrderModel>("Order details is empty!", 404);
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(model.UserId);
            if (user is not null && user.Role is Domain.Enums.RoleEnum.Admin)
            {
                return new Response<OrderModel>("User can not be admin!", 500);
            }

            var order = _mapper.Map<Order>(model);

            var orderDetails = _mapper.Map<List<OrderDetail>>(model.OrderDetails);
            order.OrderDetails = orderDetails;

            orderDetails.ForEach(x =>
            {
                x.CreatedBy = _claimService.GetCurrentUserName;
                x.CreationDate = _claimService.GetCurrentDate;
            });
            var itemPrice = await CalculateItemPrice(orderDetails);

            order.TotalPrice = orderDetails.Sum(x => x.ItemPrice);

            await _unitOfWork.OrderRepository.AddEntityAsync(order);

            var isSuccess = await _unitOfWork.SaveChangesAsync();

            if (isSuccess > 0)
            {
                var result = _mapper.Map<OrderModel>(order);
                await _emailService.SendEmail(user!, order);
                return new Response<OrderModel>(result);

            }

            return new Response<OrderModel>("Failed to add order!", 500);
        }

        private async Task<decimal> CalculateItemPrice(List<OrderDetail> orderDetails)
        {
            decimal itemPrice = 0;
            foreach (var obj in orderDetails)
            {
                var note = await _unitOfWork.DocumentRepository.GetEntityByCondition(x => x.Id == obj.DocumentId);

                if (note is null)
                {
                    throw new Exception("Note is not found!");
                }

                itemPrice = note.Price * obj.Quantity;
                obj.ItemPrice = itemPrice;
            }
            return itemPrice;
        }

        private async Task<int> DecreaseQuantity(List<OrderDetail> orderDetails)
        {
            int quantity = 0;
            foreach (var x in orderDetails)
            {
                var document = await _unitOfWork.DocumentRepository.GetByIdAsync(x.DocumentId);

                if (document is null)
                {
                    throw new Exception("Note is not found!");
                }

                if (document.Quantity < x.Quantity)
                {
                    throw new Exception($"Not enough quantity for document with ID {x.DocumentId}!");
                }
                else if (document.Quantity == x.Quantity)
                {
                    document.Quantity -= x.Quantity;
                }

                quantity += x.Quantity;
            }
            await _unitOfWork.SaveChangesAsync();
            return quantity;
        }
    }

}

