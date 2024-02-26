using System.IO;
using System.Text;

namespace CSXToolPlus.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static void WriteWideString(this BinaryWriter writer, string value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            var length = bytes.Length / 2;
            writer.Write(length);
            writer.Write(bytes);
        }
    }
}
