using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class TagedDwordArrayEntry
    {
        public string Tag { get; set; }
        public DwordArray Array { get; set; }

        public TagedDwordArrayEntry()
        {
            Tag = string.Empty;
            Array = new DwordArray();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Tag = reader.ReadWideString();
            Array.Read(reader);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteWideString(Tag);
            Array.Write(writer);
        }
    }
}
