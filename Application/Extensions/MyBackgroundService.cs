using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IUnitOfWork;
using Domain.Enums;

namespace Application.Extensions
{
    public class MyBackgroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public MyBackgroundService(IUnitOfWork unitOfWork, IEmailService emailService)
        => (_unitOfWork, _emailService) = (unitOfWork, emailService);

        public async Task SendEmailToCustomer()
        {
            var user = await _unitOfWork.UserRepository.GetEntityByCondition(x => x.Role == RoleEnum.Customer && x.Email == "Toanmnh2002@gmail.com")!;
            var order = await _unitOfWork.OrderRepository.GetEntityByCondition(x => x.UserId == user!.Id && x.PaymentStatus == PaymentStatusEnum.Inprocessing)!;
            await _emailService.SendEmail(user!, order!);
        }
    }
}
