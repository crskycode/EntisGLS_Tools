using CSXToolPlus.Types;
using CSXToolPlus.Utils;

namespace CSXToolPlus.Sections
{
    public class SectionInitNakedFunc
    {
        public DwordArray NakedPrologue { get; set; }
        public DwordArray NakedEpilogue { get; set; }

        public SectionInitNakedFunc()
        {
            NakedPrologue = new DwordArray();
            NakedEpilogue = new DwordArray();
        }

        public void Read(SimpleBinaryReader reader)
        {
            NakedPrologue.Read(reader);
            NakedEpilogue.Read(reader);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            NakedPrologue.Write(writer);
            NakedEpilogue.Write(writer);
        }
    }
}
