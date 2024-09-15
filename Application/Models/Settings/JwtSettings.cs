namespace Application.Models.Settings
{
    public class JwtSection
    {
        public string Key { get; set; } = null!;
        public int ExpiresInDays { get; set; }
    }
}
