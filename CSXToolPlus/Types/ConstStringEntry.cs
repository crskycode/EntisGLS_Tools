using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class ConstStringEntry
    {
        public string String { get; set; }
        public DwordArray Refs { get; set; }

        public ConstStringEntry()
        {
            String = string.Empty;
            Refs = new DwordArray();
        }

        public void Read(SimpleBinaryReader reader)
        {
            String = reader.ReadWideString();
            Refs.Read(reader);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteWideString(String);
            Refs.Write(writer);
        }
    }
}
