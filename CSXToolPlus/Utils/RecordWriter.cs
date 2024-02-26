using System;

namespace CSXToolPlus.Utils
{
    public class RecordWriter
    {
        public static void Write(SimpleBinaryWriter writer, long id, Action<SimpleBinaryWriter> action)
        {
            writer.WriteInt64(id);
            var v1 = writer.Writer.BaseStream.Position;
            writer.WriteInt64(0L);
            var v2 = writer.Writer.BaseStream.Position;
            action(writer);
            var v3 = writer.Writer.BaseStream.Position;
            var length = v3 - v2;
            writer.Writer.BaseStream.Position = v1;
            writer.WriteInt64(length);
            writer.Writer.BaseStream.Position = v3;
        }
    }
}
