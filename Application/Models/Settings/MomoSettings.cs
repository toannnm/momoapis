namespace Application.Models.Settings
{
    public class MomoSection
    {
        public string PartnerCode { get; set; }
        public string ReturnUrl { get; set; }
        public string PaymentUrl { get; set; }
        public string IpnUrl { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string ServiceName { get; set; }
        public string MediaType { get; set; }
        public string OrderInfo { get; set; }
        public string QueryTransactionEndpoint { get; set; }
        public string RefundEndpoint { get; set; }
        public string Lang { get; set; }
    }

}
