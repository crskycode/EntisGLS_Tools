using CSXToolPlus.Objects;
using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class FieldInfoEntry
    {
        public string Name { get; set; }
        public uint Flags { get; set; }
        public TypedObject TypeObject { get; set; }

        public FieldInfoEntry()
        {
            Name = string.Empty;
            TypeObject = new TypedObject();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Name = reader.ReadWideString();
            Flags = reader.ReadUInt32();
            TypeObject.Read(reader);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteWideString(Name);
            writer.WriteUInt32(Flags);
            TypeObject.Write(writer);
        }
    }
}
