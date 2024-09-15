using System.Security.Cryptography;
using System.Text;

namespace Application.Extensions
{
    public static class HashingMomo
    {
        public static string SignSHA256(string message, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string hex = BitConverter.ToString(hashmessage);
                hex = hex.Replace("-", "").ToLower();
                return hex;

            }
        }
        public static string SignHMACSHA256(string key, string data)
        {
            var encoding = new UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(key);
            byte[] dataBytes = encoding.GetBytes(data);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmacsha256.ComputeHash(dataBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
