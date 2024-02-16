using System.IO;
using System.Text;

namespace CSXTool.Extensions
{
    public static class BinaryWriterExtensions
    {
        public static void WriteWideString(this BinaryWriter writer, string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s);
            var length = bytes.Length / 2;
            writer.Write(length);
            writer.Write(bytes);
        }
    }
}
