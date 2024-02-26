using CSXToolPlus.Types;
using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Sections
{
    public class SectionFunction
    {
        public DwordArray Prologue { get; set; }
        public DwordArray Epilogue { get; set; }
        public List<FuncNameEntry> FuncNames { get; set; }

        public SectionFunction()
        {
            Prologue = new DwordArray();
            Epilogue = new DwordArray();
            FuncNames = new List<FuncNameEntry>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Prologue.Read(reader);
            Epilogue.Read(reader);

            var count = reader.ReadInt32();

            if (count > 0)
            {
                FuncNames = new List<FuncNameEntry>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new FuncNameEntry();
                    entry.Read(reader);
                    FuncNames.Add(entry);
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            Prologue.Write(writer);
            Epilogue.Write(writer);

            writer.WriteInt32(FuncNames.Count);

            for (var i = 0; i < FuncNames.Count; i++)
            {
                FuncNames[i].Write(writer);
            }
        }
    }
}
