using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Types
{
    public class WideStringArray
    {
        public List<string> Elements { get; set; }

        public WideStringArray()
        {
            Elements = new List<string>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                Elements = new List<string>(count);

                for (int i = 0; i < count; i++)
                {
                    Elements.Add(reader.ReadWideString());
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(Elements.Count);

            for (var i = 0; i < Elements.Count; i++)
            {
                writer.WriteWideString(Elements[i]);
            }
        }
    }
}
