using System.IO;

namespace CSXToolPlus.Types
{
    public class FileHeader
    {
        public byte[] Header { get; set; }
        public uint FileID { get; set; }
        public uint Reserved { get; set; }
        public byte[] FormatDescription { get; set; }

        public FileHeader()
        {
            Header = [];
            FormatDescription = [];
        }

        public void Read(BinaryReader reader)
        {
            Header = reader.ReadBytes(8);
            FileID = reader.ReadUInt32();
            Reserved = reader.ReadUInt32();
            FormatDescription = reader.ReadBytes(48);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write(FileID);
            writer.Write(Reserved);
            writer.Write(FormatDescription);
        }
    }
}
