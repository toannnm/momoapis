namespace Application.Interfaces.IExtensionServices
{
    public interface IClaimService
    {
        Guid? GetCurrentUserId { get; }
        string? GetCurrentUserName { get; }
        public string? GetCurrentEmail { get; }
        DateTime? GetCurrentDate { get; }
    }
}
