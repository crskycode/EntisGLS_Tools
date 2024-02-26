using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class BaseClassInfoEntry
    {
        public uint Flags { get; set; }
        public string Name { get; set; }

        public BaseClassInfoEntry()
        {
            Flags = 0;
            Name = string.Empty;
        }

        public void Read(SimpleBinaryReader reader)
        {
            Flags = reader.ReadUInt32();
            Name = reader.ReadWideString();
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteUInt32(Flags);
            writer.WriteWideString(Name);
        }
    }
}
