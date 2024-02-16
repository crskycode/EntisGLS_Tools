using System;
using System.IO;
using System.Text;

namespace CSXTool.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static int CheckReadInt32(this BinaryReader reader)
        {
            checked
            {
                return (int)reader.ReadUInt32();
            }
        }

        public static string ReadWideString(this BinaryReader reader)
        {
            var length = reader.CheckReadInt32();

            if (length > 0)
            {
                var numBytesToRead = Convert.ToInt32(2 * length);

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
