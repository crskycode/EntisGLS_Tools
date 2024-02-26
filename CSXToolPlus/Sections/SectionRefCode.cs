using CSXToolPlus.Types;
using CSXToolPlus.Utils;

namespace CSXToolPlus.Sections
{
    public class SectionRefCode
    {
        public DwordArray Refs { get; set; }

        public SectionRefCode()
        {
            Refs = new DwordArray();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Refs.Read(reader);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            Refs.Write(writer);
        }
    }
}
