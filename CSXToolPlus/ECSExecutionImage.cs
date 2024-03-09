using CSXToolPlus.Extensions;
using CSXToolPlus.Sections;
using CSXToolPlus.Types;
using CSXToolPlus.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CSXToolPlus
{
    public partial class ECSExecutionImage
    {
        private const long ID_Header = 0x2020726564616568;
        private const long ID_Image = 0x2020206567616D69;
        private const long ID_ImageGlobal = 0x6C626F6C67676D69;
        private const long ID_ImageConst = 0x74736E6F63676D69;
        private const long ID_ImageShared = 0x6572616873676D69;
        private const long ID_ClassInfo = 0x666E697373616C63;
        private const long ID_Function = 0x6E6F6974636E7566;
        private const long ID_InitNakedFunc = 0x636E666E74696E69;
        private const long ID_FuncInfo = 0x6F666E69636E7566;
        private const long ID_SymbolInfo = 0x666E696C626D7973;
        private const long ID_Global = 0x20206C61626F6C67;
        private const long ID_Data = 0x2020202061746164;
        private const long ID_ConstString = 0x72747374736E6F63;
        private const long ID_LinkInfo = 0x20666E696B6E696C;
        private const long ID_LinkInfoEx = 0x343678656B6E696C;
        private const long ID_RefFunc = 0x20636E7566666572;
        private const long ID_RefCode = 0x2065646F63666572;
        private const long ID_RefClass = 0x7373616C63666572;
        private const long ID_ImportNativeFunc = 0x766974616E706D69;

        private readonly FileHeader _fileHeader;

        private byte[] _sectionHeaderBuffer;
        private byte[] _sectionImageBuffer;
        private byte[] _sectionImageGlobalBuffer;
        private byte[] _sectionImageConstBuffer;
        private byte[] _sectionImageSharedBuffer;
        private byte[] _sectionClassInfoBuffer;
        private byte[] _sectionFunctionBuffer;
        private byte[] _sectionInitNakedFuncBuffer;
        private byte[] _sectionFuncInfoBuffer;
        private byte[] _sectionSymbolInfoBuffer;
        private byte[] _sectionGlobalBuffer;
        private byte[] _sectionDataBuffer;
        private byte[] _sectionConstStringBuffer;
        private byte[] _sectionLinkInfoBuffer;
        private byte[] _sectionLinkInfoExBuffer;
        private byte[] _sectionRefFuncBuffer;
        private byte[] _sectionRefCodeBuffer;
        private byte[] _sectionRefClassBuffer;
        private byte[] _sectionImportNativeFuncBuffer;

        private readonly SectionHeader _sectionHeader;
        private readonly SectionClassInfo _sectionClassInfo;
        private readonly SectionFunction _sectionFunction;
        private readonly SectionInitNakedFunc _sectionInitNakedFunc;
        private readonly SectionFuncInfo _sectionFuncInfo;
        private readonly SectionSymbolInfo _sectionSymbolInfo;
        private readonly SectionGlobal _sectionGlobal;
        private readonly SectionData _sectionData;
        private readonly SectionConstString _sectionConstString;
        private readonly SectionLinkInfo _sectionLinkInfo;
        private readonly SectionLinkInfoEx _sectionLinkInfoEx;
        private readonly SectionRefFunc _sectionRefFunc;
        private readonly SectionRefCode _sectionRefCode;
        private readonly SectionRefClass _sectionRefClass;
        private readonly SectionImportNativeFunc _sectionImportNativeFunc;

        public ECSExecutionImage()
        {
            _fileHeader = new FileHeader();

            _sectionHeaderBuffer = [];
            _sectionImageBuffer = [];
            _sectionImageGlobalBuffer = [];
            _sectionImageConstBuffer = [];
            _sectionImageSharedBuffer = [];
            _sectionClassInfoBuffer = [];
            _sectionFunctionBuffer = [];
            _sectionInitNakedFuncBuffer = [];
            _sectionFuncInfoBuffer = [];
            _sectionSymbolInfoBuffer = [];
            _sectionGlobalBuffer = [];
            _sectionDataBuffer = [];
            _sectionConstStringBuffer = [];
            _sectionLinkInfoBuffer = [];
            _sectionLinkInfoExBuffer = [];
            _sectionRefFuncBuffer = [];
            _sectionRefCodeBuffer = [];
            _sectionRefClassBuffer = [];
            _sectionImportNativeFuncBuffer = [];

            _sectionHeader = new SectionHeader();
            _sectionClassInfo = new SectionClassInfo();
            _sectionFunction = new SectionFunction();
            _sectionInitNakedFunc = new SectionInitNakedFunc();
            _sectionFuncInfo = new SectionFuncInfo();
            _sectionSymbolInfo = new SectionSymbolInfo();
            _sectionGlobal = new SectionGlobal();
            _sectionData = new SectionData();
            _sectionConstString = new SectionConstString();
            _sectionLinkInfo = new SectionLinkInfo();
            _sectionLinkInfoEx = new SectionLinkInfoEx();
            _sectionRefFunc = new SectionRefFunc();
            _sectionRefCode = new SectionRefCode();
            _sectionRefClass = new SectionRefClass();
            _sectionImportNativeFunc = new SectionImportNativeFunc();
        }

        public void Load(string path, uint fullVer)
        {
            var stream = File.OpenRead(path);
            var reader = new BinaryReader(stream);

            _sectionHeader.FullVer = fullVer;

            ReadFileHeader(reader);

            while (stream.Position < stream.Length)
            {
                var id = reader.ReadInt64();
                var length = reader.ReadInt64();

                switch (id)
                {
                    case ID_Header:
                        ReadHeaderSection(reader, length);
                        break;
                    case ID_Image:
                        ReadImageSection(reader, length);
                        break;
                    case ID_ImageGlobal:
                        ReadImageGlobalSection(reader, length);
                        break;
                    case ID_ImageConst:
                        ReadImageConstSection(reader, length);
                        break;
                    case ID_ImageShared:
                        ReadImageSharedSection(reader, length);
                        break;
                    case ID_ClassInfo:
                        ReadClassInfoSection(reader, length);
                        break;
                    case ID_Function:
                        ReadFunctionSection(reader, length);
                        break;
                    case ID_InitNakedFunc:
                        ReadInitNakedFuncSection(reader, length);
                        break;
                    case ID_FuncInfo:
                        ReadFuncInfoSection(reader, length);
                        break;
                    case ID_SymbolInfo:
                        ReadSymbolInfoSection(reader, length);
                        break;
                    case ID_Global:
                        ReadGlobalSection(reader, length);
                        break;
                    case ID_Data:
                        ReadDataSection(reader, length);
                        break;
                    case ID_ConstString:
                        ReadConstStringSection(reader, length);
                        break;
                    case ID_LinkInfo:
                        ReadLinkInfoSection(reader, length);
                        break;
                    case ID_LinkInfoEx:
                        ReadLinkInfoExSection(reader, length);
                        break;
                    case ID_RefFunc:
                        ReadRefFuncSection(reader, length);
                        break;
                    case ID_RefCode:
                        ReadRefCodeSection(reader, length);
                        break;
                    case ID_RefClass:
                        ReadRefClassSection(reader, length);
                        break;
                    case ID_ImportNativeFunc:
                        ReadImportNativeFuncSection(reader, length);
                        break;
                    case 0:
                        break;
                    default:
                        throw new InvalidDataException();
                }
            }

            if (stream.Position != stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }

            stream.Dispose();
        }

        private void ReadFileHeader(BinaryReader reader)
        {
            _fileHeader.Read(reader);

            if (_fileHeader.Header.ReadInt64() != 0x00001A7369746E45)
            {
                throw new InvalidDataException();
            }

            var description = Encoding.UTF8.GetString(_fileHeader.FormatDescription, 0, 18);

            if (description != "Cotopha Image file")
            {
                throw new InvalidDataException();
            }
        }

        private void ReadHeaderSection(BinaryReader reader, long length)
        {
            _sectionHeaderBuffer = reader.ReadBytes(Convert.ToInt32(length));
            _sectionHeader.Read(_sectionHeaderBuffer);
        }

        private void ReadImageSection(BinaryReader reader, long length)
        {
            _sectionImageBuffer = reader.ReadBytes(Convert.ToInt32(length));
        }

        private void ReadImageGlobalSection(BinaryReader reader, long length)
        {
            _sectionImageGlobalBuffer = reader.ReadBytes(Convert.ToInt32(length));
        }

        private void ReadImageConstSection(BinaryReader reader, long length)
        {
            _sectionImageConstBuffer = reader.ReadBytes(Convert.ToInt32(length));
        }

        private void ReadImageSharedSection(BinaryReader reader, long length)
        {
            _sectionImageSharedBuffer = reader.ReadBytes(Convert.ToInt32(length));
        }

        private void ReadClassInfoSection(BinaryReader reader, long length)
        {
            _sectionClassInfoBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionClassInfoBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionClassInfo.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadFunctionSection(BinaryReader reader, long length)
        {
            _sectionFunctionBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionFunctionBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionFunction.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadInitNakedFuncSection(BinaryReader reader, long length)
        {
            _sectionInitNakedFuncBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionInitNakedFuncBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionInitNakedFunc.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadFuncInfoSection(BinaryReader reader, long length)
        {
            _sectionFuncInfoBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionFuncInfoBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionFuncInfo.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadSymbolInfoSection(BinaryReader reader, long length)
        {
            _sectionSymbolInfoBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionSymbolInfoBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionSymbolInfo.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadGlobalSection(BinaryReader reader, long length)
        {
            _sectionGlobalBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionGlobalBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionGlobal.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadDataSection(BinaryReader reader, long length)
        {
            _sectionDataBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionDataBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionData.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadConstStringSection(BinaryReader reader, long length)
        {
            _sectionConstStringBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionConstStringBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionConstString.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadLinkInfoSection(BinaryReader reader, long length)
        {
            _sectionLinkInfoBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionLinkInfoBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionLinkInfo.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadLinkInfoExSection(BinaryReader reader, long length)
        {
            _sectionLinkInfoExBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionLinkInfoExBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionLinkInfoEx.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadRefFuncSection(BinaryReader reader, long length)
        {
            _sectionRefFuncBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionRefFuncBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionRefFunc.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadRefCodeSection(BinaryReader reader, long length)
        {
            _sectionRefCodeBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionRefCodeBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionRefCode.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadRefClassSection(BinaryReader reader, long length)
        {
            _sectionRefClassBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionRefClassBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionRefClass.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        private void ReadImportNativeFuncSection(BinaryReader reader, long length)
        {
            _sectionImportNativeFuncBuffer = reader.ReadBytes(Convert.ToInt32(length));

            var _stream = new MemoryStream(_sectionImportNativeFuncBuffer);
            var _reader = new SimpleBinaryReader(_stream, _sectionHeader.GetInfo());

            _sectionImportNativeFunc.Read(_reader);

            if (_stream.Position != _stream.Length)
            {
                Console.WriteLine("WARNING: Some data is not parsed.");
            }
        }

        public void Save(string path)
        {
            var stream = File.Create(path);
            var writer = new SimpleBinaryWriter(stream, _sectionHeader.GetInfo());

            _fileHeader.Write(writer.Writer);

            if (_sectionHeaderBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_Header, WriteHeaderSection);
            }

            if (_sectionImageBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_Image, WriteImageSection);
            }

            if (_sectionImageGlobalBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_ImageGlobal, WriteImageGlobalSection);
            }

            if (_sectionImageConstBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_ImageConst, WriteImageConstSection);
            }

            if (_sectionImageSharedBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_ImageShared, WriteImageSharedSection);
            }

            if (_sectionClassInfoBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_ClassInfo, WriteClassInfoSection);
            }

            if (_sectionFunctionBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_Function, WriteFunctionSection);
            }

            if (_sectionInitNakedFuncBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_InitNakedFunc, WriteInitNakedFuncSection);
            }

            if (_sectionFuncInfoBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_FuncInfo, WriteFuncInfoSection);
            }

            if (_sectionSymbolInfoBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_SymbolInfo, WriteSymbolInfoSection);
            }

            if (_sectionGlobalBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_Global, WriteGlobalSection);
            }

            if (_sectionDataBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_Data, WriteDataSection);
            }

            if (_sectionConstStringBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_ConstString, WriteConstStringSection);
            }

            if (_sectionLinkInfoBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_LinkInfo, WriteLinkInfoSection);
            }

            if (_sectionLinkInfoExBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_LinkInfoEx, WriteLinkInfoExSection);
            }

            if (_sectionRefFuncBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_RefFunc, WriteRefFuncSection);
            }

            if (_sectionRefCodeBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_RefCode, WriteRefCodeSection);
            }

            if (_sectionRefClassBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_RefClass, WriteRefClassSection);
            }

            if (_sectionImportNativeFuncBuffer.Length != 0)
            {
                RecordWriter.Write(writer, ID_ImportNativeFunc, WriteImportNativeFuncSection);
            }

            stream.Flush();
            stream.Dispose();
        }

        private void WriteHeaderSection(SimpleBinaryWriter writer)
        {
            _sectionHeader.Write(writer.Writer);
        }

        private void WriteImageSection(SimpleBinaryWriter writer)
        {
            writer.WriteBytes(_sectionImageBuffer);
        }

        private void WriteImageGlobalSection(SimpleBinaryWriter writer)
        {
            writer.WriteBytes(_sectionImageGlobalBuffer);
        }

        private void WriteImageConstSection(SimpleBinaryWriter writer)
        {
            writer.WriteBytes(_sectionImageConstBuffer);
        }

        private void WriteImageSharedSection(SimpleBinaryWriter writer)
        {
            writer.WriteBytes(_sectionImageSharedBuffer);
        }

        private void WriteClassInfoSection(SimpleBinaryWriter writer)
        {
            _sectionClassInfo.Write(writer);
        }

        private void WriteFunctionSection(SimpleBinaryWriter writer)
        {
            _sectionFunction.Write(writer);
        }

        private void WriteInitNakedFuncSection(SimpleBinaryWriter writer)
        {
            _sectionInitNakedFunc.Write(writer);
        }

        private void WriteFuncInfoSection(SimpleBinaryWriter writer)
        {
            _sectionFuncInfo.Write(writer);
        }

        private void WriteSymbolInfoSection(SimpleBinaryWriter writer)
        {
            _sectionSymbolInfo.Write(writer);
        }

        private void WriteGlobalSection(SimpleBinaryWriter writer)
        {
            _sectionGlobal.Write(writer);
        }

        private void WriteDataSection(SimpleBinaryWriter writer)
        {
            _sectionData.Write(writer);
        }

        private void WriteConstStringSection(SimpleBinaryWriter writer)
        {
            _sectionConstString.Write(writer);
        }

        private void WriteLinkInfoSection(SimpleBinaryWriter writer)
        {
            _sectionLinkInfo.Write(writer);
        }

        private void WriteLinkInfoExSection(SimpleBinaryWriter writer)
        {
            _sectionLinkInfoEx.Write(writer);
        }

        private void WriteRefFuncSection(SimpleBinaryWriter writer)
        {
            _sectionRefFunc.Write(writer);
        }

        private void WriteRefCodeSection(SimpleBinaryWriter writer)
        {
            _sectionRefCode.Write(writer);
        }

        private void WriteRefClassSection(SimpleBinaryWriter writer)
        {
            _sectionRefClass.Write(writer);
        }

        private void WriteImportNativeFuncSection(SimpleBinaryWriter writer)
        {
            _sectionImportNativeFunc.Write(writer);
        }

        public void Disassemble(string path)
        {
            var output = File.CreateText(path);

            var dis = new ECSExecutionImageDisassembler(
                _sectionImageBuffer,
                _sectionFunction,
                _sectionFuncInfo,
                _sectionImportNativeFunc,
                _sectionClassInfo,
                _sectionConstString,
                output);
            dis.Execute();

            output.Flush();
            output.Dispose();

            Console.WriteLine("Done");
        }

        public void ExportAllText(string path)
        {
            Console.WriteLine("Parsing code...");

            var disasm = new ECSExecutionImageDisassembler(
                _sectionImageBuffer,
                _sectionFunction,
                _sectionFuncInfo,
                _sectionImportNativeFunc,
                _sectionClassInfo,
                _sectionConstString,
                null);
            disasm.Execute();

            var stream = new MemoryStream(_sectionImageBuffer);
            var reader = new BinaryReader(stream);

            Console.WriteLine("Generating text...");

            var writer = File.CreateText(path);

            foreach (var cmd in disasm.Assembly)
            {
                if (cmd.Code == CSInstructionCode.csicLoad)
                {
                    stream.Position = cmd.Addr;

                    reader.ReadByte(); // code
                    var csomType = (CSObjectMode)reader.ReadByte();
                    var csvtType = (CSVariableType)reader.ReadByte();

                    if (csomType == CSObjectMode.csomImmediate && csvtType == CSVariableType.csvtString)
                    {
                        string text;
                        int addr;

                        var length = reader.ReadUInt32();

                        if (length != 0x80000000)
                        {
                            reader.BaseStream.Position -= 4;
                            addr = cmd.Addr;
                            text = reader.ReadWideString();
                        }
                        else
                        {
                            var index = reader.ReadInt32();
                            text = _sectionConstString.Strings[index].String;
                            addr = (int)(index | 0x80000000);
                        }

                        text = text.Escape();

                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            writer.WriteLine("◇{0:X8}◇{1}", addr, text);
                            writer.WriteLine("◆{0:X8}◆{1}", addr, text);
                            writer.WriteLine();
                        }
                    }
                }
            }

            writer.Flush();
            writer.Dispose();

            Console.WriteLine("Done.");
        }

        [GeneratedRegex(@"◆(\w+)◆(.+$)")]
        private static partial Regex TextLineRegex();

        private static Dictionary<long, string> LoadTranslation(string path)
        {
            using var reader = File.OpenText(path);

            var dict = new Dictionary<long, string>();
            var num = 0;

            while (!reader.EndOfStream)
            {
                var n = num;
                var line = reader.ReadLine();
                num++;

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line[0] != '◆')
                {
                    continue;
                }

                var match = TextLineRegex().Match(line);

                if (match.Groups.Count != 3)
                {
                    throw new Exception($"Bad format at line: {n}");
                }

                var addr = long.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
                var text = match.Groups[2].Value.Unescape();

                dict.TryAdd(addr, text);
            }

            reader.Close();

            return dict;
        }

        private void UpdateConstantString(Dictionary<long, string> translation)
        {
            for (var i = 0; i < _sectionConstString.Strings.Count; i++)
            {
                var key = (uint)i | 0x80000000;

                if (translation.TryGetValue(key, out string? text))
                {
                    _sectionConstString.Strings[i].String = text;
                }
            }
        }

        public void ImportText(string path)
        {
            Console.WriteLine("Loading translation...");

            var translation = LoadTranslation(path);

            Console.WriteLine("Updating constant string...");

            UpdateConstantString(translation);

            Console.WriteLine("Rebuild finished.");
        }
    }
}
