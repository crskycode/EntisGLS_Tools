using CSXToolPlus.Extensions;
using CSXToolPlus.Types;
using System.IO;
using System.Text;

namespace CSXToolPlus.Utils
{
    public class SimpleBinaryWriter
    {
        public BinaryWriter Writer { get; }
        public VersionInfo Info { get; set; }

        public SimpleBinaryWriter(Stream stream, VersionInfo info)
        {
            Writer = new BinaryWriter(stream, Encoding.UTF8, true);
            Info = info;
        }

        public void WriteBytes(byte[] bytes)
        {
            Writer.Write(bytes);
        }

        public void WriteByte(byte value)
        {
            Writer.Write(value);
        }

        public void WriteInt32(int value)
        {
            Writer.Write(value);
        }

        public void WriteUInt32(uint value)
        {
            Writer.Write(value);
        }

        public void WriteInt64(long value)
        {
            Writer.Write(value);
        }

        public void WriteUInt64(ulong value)
        {
            Writer.Write(value);
        }

        public void WriteSingle(float value)
        {
            Writer.Write(value);
        }

        public void WriteDouble(double value)
        {
            Writer.Write(value);
        }

        public void WriteWideString(string value)
        {
            Writer.WriteWideString(value);
        }
    }
}
