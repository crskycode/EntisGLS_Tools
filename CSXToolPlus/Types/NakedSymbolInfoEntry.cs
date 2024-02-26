using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class NakedSymbolInfoEntry
    {
        public uint Flags { get; set; }
        public uint Reserved { get; set; }
        public ulong Address { get; set; }

        public void Read(SimpleBinaryReader reader)
        {
            Flags = reader.ReadUInt32();
            Reserved = reader.ReadUInt32();
            Address = reader.ReadUInt64();
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteUInt32(Flags);
            writer.WriteUInt32(Reserved);
            writer.WriteUInt64(Address);
        }
    }
}
