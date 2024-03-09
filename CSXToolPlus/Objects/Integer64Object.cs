using CSXToolPlus.Utils;

namespace CSXToolPlus.Objects
{
    public class Integer64Object
    {
        public long Mask { get; set; }
        public long Value { get; set; }

        public void Read(SimpleBinaryReader reader)
        {
            Mask = reader.ReadInt64();
            Value = reader.ReadInt64();
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt64(Mask);
            writer.WriteInt64(Value);
        }
    }
}
