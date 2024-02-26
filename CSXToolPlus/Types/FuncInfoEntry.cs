using CSXToolPlus.Utils;
using System;

namespace CSXToolPlus.Types
{
    public class FuncInfoEntry
    {
        public FuncEntryHeader Header { get; set; }
        public string Name { get; set; }
        public byte[] Reserved { get; set; }

        public FuncInfoEntry()
        {
            Header = new FuncEntryHeader();
            Name = string.Empty;
            Reserved = Array.Empty<byte>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Header.Read(reader);
            Name = reader.ReadWideString();

            var length = Convert.ToInt32(Header.Reserved);

            if (length > 0)
            {
                Reserved = reader.ReadBytes(length);
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            Header.Write(writer);
            writer.WriteWideString(Name);

            if (Reserved.Length > 0)
            {
                writer.WriteBytes(Reserved);
            }
        }
    }
}
