using CSXToolPlus.Utils;

namespace CSXToolPlus.Objects
{
    public class ClassInfoObject
    {
        public string ClassName { get; set; }

        public ClassInfoObject()
        {
            ClassName = string.Empty;
        }

        public void Read(SimpleBinaryReader reader)
        {
            ClassName = reader.ReadWideString();
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteWideString(ClassName);
        }
    }
}
