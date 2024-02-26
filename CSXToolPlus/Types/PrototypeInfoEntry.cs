using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Types
{
    public class PrototypeInfoEntry
    {
        public uint Flags { get; set; }
        public string Name { get; set; }
        public string GlobalName { get; set; }
        public TypeInfoEntry ReturnType { get; set; }
        public List<TypeInfoEntry> Arguments { get; set; }

        public PrototypeInfoEntry()
        {
            Name = string.Empty;
            GlobalName = string.Empty;
            ReturnType = new TypeInfoEntry();
            Arguments = new List<TypeInfoEntry>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Flags = reader.ReadUInt32();
            Name = reader.ReadWideString();
            GlobalName = reader.ReadWideString();

            ReturnType.Read(reader);

            var argCount = reader.ReadInt32();

            if (argCount > 0)
            {
                Arguments = new List<TypeInfoEntry>(argCount);

                for (var i = 0; i < argCount; i++)
                {
                    var entry = new TypeInfoEntry();
                    entry.Read(reader);
                    Arguments.Add(entry);
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteUInt32(Flags);
            writer.WriteWideString(Name);
            writer.WriteWideString(GlobalName);

            ReturnType.Write(writer);

            writer.WriteInt32(Arguments.Count);

            for (var i = 0; i < Arguments.Count; i++)
            {
                Arguments[i].Write(writer);
            }
        }
    }
}
