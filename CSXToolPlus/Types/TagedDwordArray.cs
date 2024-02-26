using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Types
{
    public class TagedDwordArray
    {
        public List<TagedDwordArrayEntry> Elements { get; set; }

        public TagedDwordArray()
        {
            Elements = new List<TagedDwordArrayEntry>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                Elements = new List<TagedDwordArrayEntry>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new TagedDwordArrayEntry();
                    entry.Read(reader);
                    Elements.Add(entry);
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(Elements.Count);

            for (var i = 0; i < Elements.Count; i++)
            {
                Elements[i].Write(writer);
            }
        }
    }
}
