using CSXToolPlus.Utils;

namespace CSXToolPlus.Objects
{
    public class PointerObject
    {
        public int RefType { get; set; }
        public byte ReadOnly { get; set; }
        public TypedObject RefTypeObject { get; set; }

        public PointerObject()
        {
            RefTypeObject = new TypedObject();
        }

        public void Read(SimpleBinaryReader reader)
        {
            RefType = reader.ReadInt32();
            ReadOnly = reader.ReadByte();
            RefTypeObject.Read(reader);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(RefType);
            writer.WriteByte(ReadOnly);
            RefTypeObject.Write(writer);
        }
    }
}
