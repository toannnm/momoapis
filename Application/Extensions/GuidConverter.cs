using System.Numerics;

namespace Application.Extensions
{
    public class GuidConverter
    {
        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
        public static Guid ConvertIntToGuid(BigInteger integerValue)
        {
            // Convert the integer to a 16-byte array
            byte[] byteArray = integerValue.ToByteArray();

            // Ensure the byte array is 16 bytes long (GUID is 128 bits)
            if (byteArray.Length < 16)
            {
                Array.Resize(ref byteArray, 16);
            }
            else if (byteArray.Length > 16)
            {
                byte[] truncatedByteArray = new byte[16];
                Array.Copy(byteArray, truncatedByteArray, 16);
                byteArray = truncatedByteArray;
            }

            // Convert the byte array to a GUID
            return new Guid(byteArray);
        }
    }
}
