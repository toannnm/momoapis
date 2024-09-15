using Application.Extensions;

namespace Application.Models.MomoModels
{
    public class QueryTransactionModel
    {
        public string partnerCode { get; set; }
        public string requestId { get; set; }
        public string orderId { get; set; }
        public string signature { get; set; }
        public string lang { get; set; }
        public string MakeSignature(string accessKey, string secretKey)
        {
            string rawHash = "accessKey=" + accessKey +
                "&orderId=" + orderId +
                "&partnerCode=" + partnerCode +
                "&requestId=" + requestId;
            return signature = HashingMomo.SignSHA256(rawHash, secretKey);
        }

    }
    public class QueryTransactionResponse
    {
        public string partnerCode { get; set; }
        public string orderId { get; set; }
        public string requestId { get; set; }
        public string extraData { get; set; }
        public int amount { get; set; }
        public long transId { get; set; }
        public string payType { get; set; }
        public int resultCode { get; set; }
        public List<Refundtran> refundTrans { get; set; }
        public string message { get; set; }
        public long responseTime { get; set; }
        public long lastUpdated { get; set; }
        public string signature { get; set; }
    }
    public class Refundtran
    {
        public string orderId { get; set; }
        public int amount { get; set; }
        public int resultCode { get; set; }
        public long transId { get; set; }
        public string createdTime { get; set; }
    }
}



