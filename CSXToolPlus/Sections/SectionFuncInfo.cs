using CSXToolPlus.Types;
using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Sections
{
    public class SectionFuncInfo
    {
        public List<FuncInfoEntry> Functions { get; set; }

        public SectionFuncInfo()
        {
            Functions = new List<FuncInfoEntry>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                Functions = new List<FuncInfoEntry>(count);

                for (int i = 0; i < count; i++)
                {
                    var entry = new FuncInfoEntry();
                    entry.Read(reader);
                    Functions.Add(entry);
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(Functions.Count);

            for (var i = 0; i < Functions.Count; i++)
            {
                Functions[i].Write(writer);
            }
        }
    }
}
