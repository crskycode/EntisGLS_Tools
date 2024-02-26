using CSXToolPlus.Objects;
using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class TypeInfoEntry
    {
        public uint Flags { get; set; }
        public TypedObject TypeObject { get; set; }

        public TypeInfoEntry()
        {
            TypeObject = new TypedObject();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Flags = reader.ReadUInt32();
            TypeObject.Read(reader);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteUInt32(Flags);
            TypeObject.Write(writer);
        }
    }
}
