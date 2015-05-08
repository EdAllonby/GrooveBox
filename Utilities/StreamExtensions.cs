using System.IO;

namespace Utilities
{
    public static class StreamExtensions
    {
        public static byte[] StreamToBytes(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}