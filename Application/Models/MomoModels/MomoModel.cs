using Application.Extensions;

namespace Application.Models.MomoModels
{
    public class MomoModel
    {
        public string partnerCode { get; set; }
        public string partnerName { get; set; } = "test";
        public string storeId { get; set; } = "MomoTestStore";
        public string requestType { get; set; } = "captureWallet";
        public string ipnUrl { get; set; }
        public string redirectUrl { get; set; }
        public string orderId { get; set; }
        public string amount { get; set; }
        public string lang { get; set; } = "vi";
        public string orderInfo { get; set; }
        public string requestId { get; set; }
        public string extraData { get; set; } = "";
        public string signature { get; set; }
        public object userInfo { get; set; }
        public string MakeSignature(string accessKey, string secretKey)
        {
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType;
            return signature = HashingMomo.SignSHA256(rawHash, secretKey);
        }
    }

    public class MomoResponse
    {
        public string partnerCode { get; set; }
        public string orderId { get; set; }
        public string requestId { get; set; }
        public int amount { get; set; }
        public long responseTime { get; set; }
        public string message { get; set; }
        public string resultCode { get; set; }
        public string payUrl { get; set; }
        public string deeplink { get; set; }
        public string deeplinkMiniApp { get; set; }
    }

}

