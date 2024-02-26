using CSXToolPlus.Types;
using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Sections
{
    public class SectionGlobal
    {
        public List<TagedObjectEntry> Objects { get; set; }

        public SectionGlobal()
        {
            Objects = new List<TagedObjectEntry>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                Objects = new List<TagedObjectEntry>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new TagedObjectEntry();
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
