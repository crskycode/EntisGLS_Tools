using CSXToolPlus.Types;
using CSXToolPlus.Utils;

namespace CSXToolPlus.Sections
{
    public class SectionRefFunc
    {
        public TagedDwordArray Refs { get; set; }

        public SectionRefFunc()
        {
            Refs = new TagedDwordArray();
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
