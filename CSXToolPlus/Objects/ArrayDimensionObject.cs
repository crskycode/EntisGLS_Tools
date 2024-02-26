using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Objects
{
    public class ArrayDimensionObject
    {
        public TypedObject ElementTypeObject { get; set; }
        public int Dimension { get; set; }
        public List<int> Bounds { get; set; }
        public List<TypedObject> Elements { get; set; }

        public ArrayDimensionObject()
        {
            ElementTypeObject = new TypedObject();
            Bounds = new List<int>();
            Elements = new List<TypedObject>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            ElementTypeObject.Read(reader);

            Dimension = reader.ReadInt32();

            if (Dimension > 0)
            {
                Bounds = new List<int>(Dimension);

                for (var i = 0; i < Dimension; i++)
                {
                    Bounds.Add(reader.ReadInt32());
                }
            }

            var count = reader.ReadInt32();

            if (count > 0)
            {
                Elements = new List<TypedObject>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new TypedObject();
                    entry.Read(reader);
                    Elements.Add(entry);
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            ElementTypeObject.Write(writer);

            writer.WriteInt32(Dimension);

            for (var i = 0; i < Bounds.Count; i++)
            {
                writer.WriteInt32(Bounds[i]);
            }

            writer.WriteInt32(Elements.Count);

            for (var i = 0; i < Elements.Count; i++)
            {
                Elements[i].Write(writer);
            }
        }
    }
}
