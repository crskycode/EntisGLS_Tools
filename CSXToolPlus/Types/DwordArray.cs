using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Types
{
    public class DwordArray
    {
        public List<uint> Elements { get; set; }

        public DwordArray()
        {
            Elements = new List<uint>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                Elements = new List<uint>(count);

                for (int i = 0; i < count; i++)
                {
                    Elements.Add(reader.ReadUInt32());
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(Elements.Count);

            for (var i = 0; i < Elements.Count; i++)
            {
                writer.WriteUInt32(Elements[i]);
            }
        }
    }
}
