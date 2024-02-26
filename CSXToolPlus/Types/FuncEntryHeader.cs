using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class FuncEntryHeader
    {
        public uint Flags { get; set; }
        public uint Address { get; set; }
        public uint Bytes { get; set; }
        public uint Reserved { get; set; }

        public void Read(SimpleBinaryReader reader)
        {
            Flags = reader.ReadUInt32();
            Address = reader.ReadUInt32();
            Bytes = reader.ReadUInt32();
            Reserved = reader.ReadUInt32();
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteUInt32(Flags);
            writer.WriteUInt32(Address);
            writer.WriteUInt32(Bytes);
            writer.WriteUInt32(Reserved);
        }
    }
}
