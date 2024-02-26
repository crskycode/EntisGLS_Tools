using CSXToolPlus.Utils;
using System;
using System.Collections.Generic;

namespace CSXToolPlus.Types
{
    public class ClassInfoEntry
    {
        public uint Flags { get; set; }
        public string Name { get; set; }
        public string GlobalName { get; set; }
        public List<BaseClassInfoEntry> BaseClassInfo { get; set; }
        public List<BaseClassCastInfoEntry> BaseClassCastInfo { get; set; }
        public List<FieldInfoEntry> FieldInfo { get; set; }
        public List<MethodInfoEntry> MethodInfo { get; set; }
        public byte[] ExtraData { get; set; }

        public ClassInfoEntry()
        {
            Name = string.Empty;
            GlobalName = string.Empty;
            BaseClassInfo = new List<BaseClassInfoEntry>();
            BaseClassCastInfo = new List<BaseClassCastInfoEntry>();
            FieldInfo = new List<FieldInfoEntry>();
            MethodInfo = new List<MethodInfoEntry>();
            ExtraData = Array.Empty<byte>();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Flags = reader.ReadUInt32();
            Name = reader.ReadWideString();
            GlobalName = reader.ReadWideString();
            ReadBaseClassInfo(reader);
            ReadBaseClassCastInfo(reader);
            ReadFieldInfo(reader);
            ReadMethodInfo(reader);
            ReadExtraData(reader);
        }

        private void ReadBaseClassInfo(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                BaseClassInfo = new List<BaseClassInfoEntry>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new BaseClassInfoEntry();
                    entry.Read(reader);
                    BaseClassInfo.Add(entry);
                }
            }
        }

        private void ReadBaseClassCastInfo(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                BaseClassCastInfo = new List<BaseClassCastInfoEntry>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new BaseClassCastInfoEntry();
                    entry.Read(reader);
                    BaseClassCastInfo.Add(entry);
                }
            }
        }

        private void ReadFieldInfo(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                FieldInfo = new List<FieldInfoEntry>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new FieldInfoEntry();
                    entry.Read(reader);
                    FieldInfo.Add(entry);
                }
            }
        }

        private void ReadMethodInfo(SimpleBinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                MethodInfo = new List<MethodInfoEntry>(count);

                for (var i = 0; i < count; i++)
                {
                    var entry = new MethodInfoEntry();
                    entry.Read(reader);
                    MethodInfo.Add(entry);
                }
            }
        }

        private void ReadExtraData(SimpleBinaryReader reader)
        {
            var length = reader.ReadInt32();
            ExtraData = reader.ReadBytes(length);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteUInt32(Flags);
            writer.WriteWideString(Name);
            writer.WriteWideString(GlobalName);
            WriteBaseClassInfo(writer);
            WriteBaseClassCastInfo(writer);
            WriteFieldInfo(writer);
            WriteMethodInfo(writer);
            WriteExtraData(writer);
        }

        private void WriteBaseClassInfo(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(BaseClassInfo.Count);

            for (var i = 0; i < BaseClassInfo.Count; i++)
            {
                BaseClassInfo[i].Write(writer);
            }
        }

        private void WriteBaseClassCastInfo(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(BaseClassCastInfo.Count);

            for (var i = 0; i < BaseClassCastInfo.Count; i++)
            {
                BaseClassCastInfo[i].Write(writer);
            }
        }

        private void WriteFieldInfo(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(FieldInfo.Count);

            for (var i = 0; i < FieldInfo.Count; i++)
            {
                FieldInfo[i].Write(writer);
            }
        }

        private void WriteMethodInfo(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(MethodInfo.Count);

            for (var i = 0; i < MethodInfo.Count; i++)
            {
                MethodInfo[i].Write(writer);
            }
        }

        private void WriteExtraData(SimpleBinaryWriter writer)
        {
            writer.WriteInt32(ExtraData.Length);
            writer.WriteBytes(ExtraData);
        }
    }
}
