using Application.Extensions;

namespace Application.Models.MomoModels
{
    public class RefundModel
    {
        public string partnerCode { get; set; }
        public string orderId { get; set; }
        public string requestId { get; set; }
        public string amount { get; set; }
        public long transId { get; set; }
        public string lang { get; set; }
        public string description { get; set; }
        public string signature { get; set; }
        public string message { get; set; }
        public string MakeSignature(string accessKey, string secretKey)
        {
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&description=" + description +
                "&orderId=" + orderId +
                "&partnerCode=" + partnerCode +
                "&requestId=" + requestId +
                "&transId=" + transId;
            return signature = HashingMomo.SignSHA256(rawHash, secretKey);
        }
    }
}
