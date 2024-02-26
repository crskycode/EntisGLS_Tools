using CSXToolPlus.Utils;
using System;

namespace CSXToolPlus.Types
{
    public class MethodInfoEntry
    {
        public PrototypeInfoEntry PrototypeInfo { get; set; }
        public string FuncClass { get; set; }
        public byte[] PointerData { get; set; }

        public MethodInfoEntry()
        {
            PrototypeInfo = new PrototypeInfoEntry();
            FuncClass = string.Empty;
            PointerData = Array.Empty<byte>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            PrototypeInfo.Read(reader);
            FuncClass = reader.ReadWideString();
            PointerData = reader.ReadBytes(40);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            PrototypeInfo.Write(writer);
            writer.WriteWideString(FuncClass);
            writer.WriteBytes(PointerData);
        }
    }
}
