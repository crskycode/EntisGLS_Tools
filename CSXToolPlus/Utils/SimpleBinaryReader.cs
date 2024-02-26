using CSXToolPlus.Extensions;
using CSXToolPlus.Types;
using System.IO;
using System.Text;

namespace CSXToolPlus.Utils
{
    public class SimpleBinaryReader
    {
        public BinaryReader Reader { get; }
        public VersionInfo Info { get; }

        public SimpleBinaryReader(Stream stream, VersionInfo info)
        {
            Reader = new BinaryReader(stream, Encoding.UTF8, true);
            Info = info;
        }

        public byte[] ReadBytes(int count)
        {
            return Reader.ReadBytes(count);
        }

        public byte ReadByte()
        {
            return Reader.ReadByte();
        }

        public int ReadInt32()
        {
            return Reader.ReadInt32();
        }

        public uint ReadUInt32()
        {
            return Reader.ReadUInt32();
        }

        public long ReadInt64()
        {
            return Reader.ReadInt64();
        }

        public ulong ReadUInt64()
        {
            return Reader.ReadUInt64();
        }

        public float ReadSingle()
        {
            return Reader.ReadSingle();
        }

        public double ReadDouble()
        {
            return Reader.ReadDouble();
        }

        public string ReadWideString()
        {
            return Reader.ReadWideString();
        }
    }
}
