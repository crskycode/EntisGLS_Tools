using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class SymbolInfoEntry
    {
        public NakedSymbolInfoEntry Info { get; set; }
        public string Name { get; set; }

        public SymbolInfoEntry()
        {
            Info = new NakedSymbolInfoEntry();
            Name = string.Empty;
        }

        public void Read(SimpleBinaryReader reader)
        {
            Info.Read(reader);
            Name = reader.ReadWideString();
        }

        public void Write(SimpleBinaryWriter writer)
        {
            Info.Write(writer);
            writer.WriteWideString(Name);
        }
    }
}
