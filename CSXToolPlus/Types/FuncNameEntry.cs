using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class FuncNameEntry
    {
        public uint Address { get; set; }
        public string Name { get; set; }

        public FuncNameEntry()
        {
            Name = string.Empty;
        }

        public void Read(SimpleBinaryReader reader)
        {
            Address = reader.ReadUInt32();
            Name = reader.ReadWideString();
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteUInt32(Address);
            writer.WriteWideString(Name);
        }
    }
}
