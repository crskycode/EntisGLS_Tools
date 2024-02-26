using CSXToolPlus.Extensions;
using CSXToolPlus.Types;
using System;
using System.IO;

namespace CSXToolPlus.Sections
{
    public class SectionHeader
    {
        public uint HeaderSize { get; set; }
        public uint Version { get; set; }
        public uint IntBase { get; set; }
        public uint ContainerFlags { get; set; }
        public uint Reserved { get; set; }
        public uint StackSize { get; set; }
        public uint HeapSize { get; set; }
        public uint EntryPoint { get; set; }
        public uint StaticInitialize { get; set; }
        public uint ResumePrepare { get; set; }

        public void Read(byte[] data)
        {
            HeaderSize = Convert.ToUInt32(data.Length);

            if (HeaderSize >= 4)
            {
                Version = data.ReadUInt32(0);
            }

            if (HeaderSize >= 8)
            {
                IntBase = data.ReadUInt32(4);
            }

            if (HeaderSize >= 12)
            {
                ContainerFlags = data.ReadUInt32(8);
            }

            if (HeaderSize >= 16)
            {
                Reserved = data.ReadUInt32(12);
            }

            if (HeaderSize >= 20)
            {
                StackSize = data.ReadUInt32(16);
            }

            if (HeaderSize >= 24)
            {
                HeapSize = data.ReadUInt32(20);
            }

            if (HeaderSize >= 28)
            {
                EntryPoint = data.ReadUInt32(24);
            }

            if (HeaderSize >= 32)
            {
                StaticInitialize = data.ReadUInt32(28);
            }

            if (HeaderSize >= 36)
            {
                ResumePrepare = data.ReadUInt32(32);
            }
        }

        public void Write(BinaryWriter writer)
        {
            if (HeaderSize >= 4)
            {
                writer.Write(Version);
            }

            if (HeaderSize >= 8)
            {
                writer.Write(IntBase);
            }

            if (HeaderSize >= 12)
            {
                writer.Write(ContainerFlags);
            }

            if (HeaderSize >= 16)
            {
                writer.Write(Reserved);
            }

            if (HeaderSize >= 20)
            {
                writer.Write(StackSize);
            }

            if (HeaderSize >= 24)
            {
                writer.Write(HeapSize);
            }

            if (HeaderSize >= 28)
            {
                writer.Write(EntryPoint);
            }

            if (HeaderSize >= 32)
            {
                writer.Write(StaticInitialize);
            }

            if (HeaderSize >= 36)
            {
                writer.Write(ResumePrepare);
            }
        }

        public VersionInfo GetInfo()
        {
            return new VersionInfo
            {
                Version = this.Version,
                IntBase = this.IntBase,
            };
        }
    }
}
