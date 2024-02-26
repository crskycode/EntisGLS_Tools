using CSXToolPlus.Types;
using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Sections
{
    public class SectionSymbolInfo
    {
        public List<SymbolInfoEntry> Symbols { get; set; }

        public SectionSymbolInfo()
        {
            Symbols = new List<SymbolInfoEntry>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                Symbols = new List<SymbolInfoEntry>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new SymbolInfoEntry();
                    entry.Read(reader);
                    Symbols.Add(entry);
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(Symbols.Count);

            for (var i = 0; i < Symbols.Count; i++)
            {
                Symbols[i].Write(writer);
            }
        }
    }
}
