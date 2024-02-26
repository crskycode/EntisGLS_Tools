using System.IO;
using System.Text;

namespace CSXToolPlus.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static string ReadWideString(this BinaryReader reader)
        {
            var length = reader.ReadInt32();

            if (length > 0)
            {
                var numBytesToRead = 2 * length;

                var bytes = reader.ReadBytes(numBytesToRead);

                if (bytes.Length < numBytesToRead)
                {
                    throw new EndOfStreamException();
                }

                return Encoding.Unicode.GetString(bytes);
            }

            return string.Empty;
        }
    }
}
