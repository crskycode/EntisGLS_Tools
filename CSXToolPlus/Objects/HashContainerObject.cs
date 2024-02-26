using CSXToolPlus.Utils;

namespace CSXToolPlus.Objects
{
    public class HashContainerObject
    {
        public TypedObject ElementTypeObject { get; set; }

        public HashContainerObject()
        {
            ElementTypeObject = new TypedObject();
        }

        public void Read(SimpleBinaryReader reader)
        {
            ElementTypeObject.Read(reader);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            ElementTypeObject.Write(writer);
        }
    }
}
