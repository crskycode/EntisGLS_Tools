using CSXToolPlus.Utils;

namespace CSXToolPlus.Types
{
    public class ECSCastInterface
    {
        public int CastObject { get; set; } // union { CastObject, NativeParent }
        public int VarOffset { get; set; }
        public int VarBounds { get; set; }
        public int FuncOffset { get; set; }

        public void Read(SimpleBinaryReader reader)
        {
            CastObject = reader.ReadInt32();
            VarOffset = reader.ReadInt32();
            VarBounds = reader.ReadInt32();
            FuncOffset = reader.ReadInt32();
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(CastObject);
            writer.WriteInt32(VarOffset);
            writer.WriteInt32(VarBounds);
            writer.WriteInt32(FuncOffset);
        }
    }
}
