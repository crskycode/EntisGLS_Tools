using CSXToolPlus.Objects;
using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class TagedObjectEntry
    {
        public string Tag { get; set; }
        public TypedObject Object { get; set; }

        public TagedObjectEntry()
        {
            Tag = string.Empty;
            Object = new TypedObject();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Tag = reader.ReadWideString();
            Object.Read(reader);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteWideString(Tag);
            Object.Write(writer);
        }
    }
}
