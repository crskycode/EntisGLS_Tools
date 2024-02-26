using CSXToolPlus.Types;
using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Sections
{
    public class SectionData
    {
        public List<GlobalObjectEntry> Objects { get; set; }

        public SectionData()
        {
            Objects = new List<GlobalObjectEntry>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                Objects = new List<GlobalObjectEntry>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new GlobalObjectEntry();
                    entry.Read(reader);
                    Objects.Add(entry);
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(Objects.Count);

            for (var i = 0; i < Objects.Count; i++)
            {
                Objects[i].Write(writer);
            }
        }
    }
}
