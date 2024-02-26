using CSXToolPlus.Objects;
using CSXToolPlus.Utils;
using System;
using System.Collections.Generic;

namespace CSXToolPlus.Types
{
    public class GlobalObjectEntry
    {
        public string Name { get; set; }
        public object Object { get; set; }

        public GlobalObjectEntry()
        {
            Name = string.Empty;
            Object = new object();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Name = reader.ReadWideString();

            var length = reader.ReadInt32();

            if (length >= 0)
            {
                var obj = new List<TagedObjectEntry>(length);

                for (var i = 0; i < length; i++)
                {
                    var entry = new TagedObjectEntry();
                    entry.Read(reader);
                    obj.Add(entry);
                }

                Object = obj;
            }
            else
            {
                var obj = new TypedObject();
                obj.Read(reader);
                Object = obj;
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteWideString(Name);

            if (Object is List<TagedObjectEntry> list)
            {
                writer.WriteInt32(list.Count);

                for (var i = 0; i < list.Count; i++)
                {
                    list[i].Write(writer);
                }
            }
            else if (Object is TypedObject obj)
            {
                writer.WriteUInt32(0x80000000);
                obj.Write(writer);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
