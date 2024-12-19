using System.Runtime.InteropServices;
using System.Security;

namespace BDAS_2_dog_shelter
{
    internal class Helpers
    {
        //https://stackoverflow.com/questions/14293344/hashing-a-securestring-in-net
        public static byte[] HashSecureString(SecureString input, Func<byte[], byte[]> hash)
        {
            var bstr = Marshal.SecureStringToBSTR(input);
            var length = Marshal.ReadInt32(bstr, -4);
            var bytes = new byte[length];

            var bytesPin = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                Marshal.Copy(bstr, bytes, 0, length);
                Marshal.ZeroFreeBSTR(bstr);

                return hash(bytes);
            }
            finally
            {
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = 0;
                }

                bytesPin.Free();
            }
        }
    }
}
