using CSXToolPlus.Types;
using CSXToolPlus.Utils;
using System;
using System.Collections.Generic;

namespace CSXToolPlus.Sections
{
    public class SectionClassInfo
    {
        public List<string> Names { get; set; }
        public List<ClassInfoEntry> Infos { get; set; }

        public SectionClassInfo()
        {
            Names = new List<string>();
            Infos = new List<ClassInfoEntry>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                Names = new List<string>(count);

                for (var i = 0; i < count; i++)
                {
                    Names.Add(reader.ReadWideString());
                }

                Infos = new List<ClassInfoEntry>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new ClassInfoEntry();
                    entry.Read(reader);
                    Infos.Add(entry);
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            if (Names.Count != Infos.Count)
            {
                throw new InvalidOperationException();
            }

            writer.WriteInt32(Names.Count);

            for (var i = 0; i < Names.Count; i++)
            {
                writer.WriteWideString(Names[i]);
            }

            for (var i = 0; i < Infos.Count; i++)
            {
                Infos[i].Write(writer);
            }
        }
    }
}
