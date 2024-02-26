using CSXToolPlus.Utils;
using System;
using System.IO;

namespace CSXToolPlus.Sections
{
    public class SectionImportNativeFunc
    {
        public SectionNativeFunc NativeFunc { get; set; }
        public SectionNakedFunc NakedFunc { get; set; }

        private const long ID_NativeFunc = 0x636E66766974616E;
        private const long ID_NakedFunc = 0x636E6664656B616E;

        private byte[] _sectionNativeFuncBuffer;
        private byte[] _sectionNakedFuncBuffer;

        public SectionImportNativeFunc()
        {
            _sectionNativeFuncBuffer = Array.Empty<byte>();
            _sectionNakedFuncBuffer = Array.Empty<byte>();

            NativeFunc = new SectionNativeFunc();
            NakedFunc = new SectionNakedFunc();
        }

        public void Read(SimpleBinaryReader reader)
        {
            while (reader.Reader.BaseStream.Position < reader.Reader.BaseStream.Length)
            {
                var id = reader.ReadInt64();
                var length = reader.ReadInt64();

                switch (id)
                {
                    case ID_NativeFunc:
                        ReadNativeFuncSection(reader, length);
                        break;
                    case ID_NakedFunc:
                        ReadNakedFuncSection(reader, length);
                        break;
                    default:
                        throw new InvalidDataException();
                }
            }
        }

        private void ReadNativeFuncSection(SimpleBinaryReader reader, long length)
        {
            _sectionNativeFuncBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionNativeFuncBuffer);
            var _reader = new SimpleBinaryReader(_stream, reader.Info);
            NativeFunc.Read(_reader);
        }

        private void ReadNakedFuncSection(SimpleBinaryReader reader, long length)
        {
            _sectionNakedFuncBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionNakedFuncBuffer);
            var _reader = new SimpleBinaryReader(_stream, reader.Info);
            NakedFunc.Read(_reader);
        }

        public void Write(SimpleBinaryWriter writer)
        {
            if (_sectionNativeFuncBuffer.Length > 0)
            {
                RecordWriter.Write(writer, ID_NativeFunc, WriteNativeFuncSection);
            }

            if (_sectionNakedFuncBuffer.Length > 0)
            {
                RecordWriter.Write(writer, ID_NakedFunc, WriteNakedFuncSection);
            }
        }

        private void WriteNativeFuncSection(SimpleBinaryWriter writer)
        {
            NativeFunc.Write(writer);
        }

        private void WriteNakedFuncSection(SimpleBinaryWriter writer)
        {
            NakedFunc.Write(writer);
        }
    }
}
