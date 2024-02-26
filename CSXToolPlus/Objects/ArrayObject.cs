using CSXToolPlus.Utils;
using System.Collections.Generic;

namespace CSXToolPlus.Objects
{
    public class ArrayObject
    {
        public List<TypedObject> Elements { get; set; }

        public ArrayObject()
        {
            Elements = new List<TypedObject>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                Elements = new List<TypedObject>(count);

                for (var i = 0; i < count; i++)
                {
                    var elem = new TypedObject();
                    elem.Read(reader);
                    Elements.Add(elem);
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(Elements.Count);

            foreach (var elem in Elements)
            {
                elem.Write(writer);
            }
        }
    }
}
