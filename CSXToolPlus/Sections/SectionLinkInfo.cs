using CSXToolPlus.Types;
using CSXToolPlus.Utils;

namespace CSXToolPlus.Sections
{
    public class SectionLinkInfo
    {
        public DwordArray ExtGlobalRef { get; set; }
        public DwordArray ExtDataRef { get; set; }
        public TagedDwordArray ImpGlobalRef { get; set; }
        public TagedDwordArray ImpDataRef { get; set; }

        public SectionLinkInfo()
        {
            ExtGlobalRef = new DwordArray();
            ExtDataRef = new DwordArray();
            ImpGlobalRef = new TagedDwordArray();
            ImpDataRef = new TagedDwordArray();
        }

        public void Read(SimpleBinaryReader reader)
        {
            ExtGlobalRef.Read(reader);
            ExtDataRef.Read(reader);
            ImpGlobalRef.Read(reader);
            ImpDataRef.Read(reader);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            ExtGlobalRef.Write(writer);
            ExtDataRef.Write(writer);
            ImpGlobalRef.Write(writer);
            ImpDataRef.Write(writer);
        }
    }
}
