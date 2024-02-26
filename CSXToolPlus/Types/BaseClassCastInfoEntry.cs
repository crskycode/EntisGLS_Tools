using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class BaseClassCastInfoEntry
    {
        public string Name { get; set; }
        public ECSCastInterface Pci { get; set; }
        public uint Flags { get; set; }

        public BaseClassCastInfoEntry()
        {
            Name = string.Empty;
            Pci = new ECSCastInterface();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Name = reader.ReadWideString();
            Pci.Read(reader);
            Flags = reader.ReadUInt32();
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteWideString(Name);
            Pci.Write(writer);
            writer.WriteUInt32(Flags);
        }
    }
}
