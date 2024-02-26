using CSXToolPlus.Types;
using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Sections
{
    public class SectionConstString
    {
        public List<ConstStringEntry> Strings { get; set; }

        public SectionConstString()
        {
            Strings = new List<ConstStringEntry>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                Strings = new List<ConstStringEntry>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new ConstStringEntry();
                    entry.Read(reader);
                    Strings.Add(entry);
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(Strings.Count);

            for (var i = 0; i < Strings.Count; i++)
            {
                Strings[i].Write(writer);
            }
        }
    }
}
