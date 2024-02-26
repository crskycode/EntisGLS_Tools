using CSXToolPlus.Types;
using CSXToolPlus.Utils;

namespace CSXToolPlus.Sections
{
    public class SectionLinkInfoEx
    {
        public uint Flags { get; set; }
        public DwordArray ExtNakedGlobalRef { get; set; }
        public DwordArray ExtNakedConstRef { get; set; }
        public DwordArray ExtNakedSharedRef { get; set; }
        public DwordArray ExtNakedFuncRef { get; set; }
        public TagedDwordArray ImpNakedGlobalRef { get; set; }
        public TagedDwordArray ImpNakedConstRef { get; set; }
        public TagedDwordArray ImpNakedSharedRef { get; set; }
        public TagedDwordArray ImpNakedFuncRef { get; set; }

        public SectionLinkInfoEx()
        {
            ExtNakedGlobalRef = new DwordArray();
            ExtNakedConstRef = new DwordArray();
            ExtNakedSharedRef = new DwordArray();
            ExtNakedFuncRef = new DwordArray();
            ImpNakedGlobalRef = new TagedDwordArray();
            ImpNakedConstRef = new TagedDwordArray();
            ImpNakedSharedRef = new TagedDwordArray();
            ImpNakedFuncRef = new TagedDwordArray();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Flags = reader.ReadUInt32();

            ExtNakedGlobalRef.Read(reader);
            ExtNakedConstRef.Read(reader);
            ExtNakedSharedRef.Read(reader);

            if ((Flags & 8) != 0)
            {
                ExtNakedFuncRef.Read(reader);
            }

            ImpNakedGlobalRef.Read(reader);
            ImpNakedConstRef.Read(reader);
            ImpNakedSharedRef.Read(reader);

            if ((Flags & 0x80000) != 0)
            {
                ImpNakedFuncRef.Read(reader);
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteUInt32(Flags);

            ExtNakedGlobalRef.Write(writer);
            ExtNakedConstRef.Write(writer);
            ExtNakedSharedRef.Write(writer);

            if ((Flags & 8) != 0)
            {
                ExtNakedFuncRef.Write(writer);
            }

            ImpNakedGlobalRef.Write(writer);
            ImpNakedConstRef.Write(writer);
            ImpNakedSharedRef.Write(writer);

            if ((Flags & 0x80000) != 0)
            {
                ImpNakedFuncRef.Write(writer);
            }
        }
    }
}
