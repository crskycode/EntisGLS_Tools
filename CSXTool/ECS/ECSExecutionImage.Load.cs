using CSXTool.ECS.Enums;
using CSXTool.ECS.Stuff;
using CSXTool.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CSXTool.ECS
{
    public partial class ECSExecutionImage
    {
        public void Load(string filePath)
        {
            using var reader = new BinaryReader(File.OpenRead(filePath));

            var signature = reader.ReadBytes(8);
            var fileId = reader.ReadUInt32();
            var reserved = reader.ReadUInt32();
            var formatDesc = reader.ReadBytes(48);

            m_FileHeader = new EMCFileHeader(signature, fileId, reserved, formatDesc);

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var id = reader.ReadUInt64();

                if (id == 0)
                {
                    // Assuming that the junk data at the end of the file, we ignore it.
                    break;
                }

                var size = reader.ReadInt64();

                switch (id)
                {
                    case 0x2020726564616568u: // "header"
                        ReadHeaderSection(reader, size);
                        break;
                    case 0x2020206567616D69u: // "image"
                        ReadImageSection(reader, size);
                        break;
                    case 0x6E6F6974636E7566u: // "function"
                        ReadFunctionSection(reader, size);
                        break;
                    case 0x20206C61626F6C67u: // "global"
                        ReadGlobalSection(reader, size);
                        break;
                    case 0x2020202061746164u: // "data"
                        ReadDataSection(reader, size);
                        break;
                    case 0x72747374736E6F63u: // "conststr"
                        ReadConstantStringSection(reader, size);
                        break;
                    case 0x20666E696B6E696Cu: // "linkinf"
                        ReadLinkInformationSection(reader, size);
                        break;
                    case 0x0000000000000000u: // Padding or junk.
                        reader.BaseStream.Position = reader.BaseStream.Length;
                        break;
                    default:
                        throw new Exception("Unknow Record ID");
                }
            }

            Debug.Assert(reader.BaseStream.Position == reader.BaseStream.Length);

            reader.Dispose();
        }

        private void ReadHeaderSection(BinaryReader reader, long size)
        {
            m_exiHeader = reader.ReadBytes(Convert.ToInt32(size));

            // Compatibility

            if (m_exiHeader.Length >= 4)
                m_Header.Version = BitConverter.ToUInt32(m_exiHeader, 0);

            if (m_exiHeader.Length >= 8)
                m_Header.IntBase = BitConverter.ToUInt32(m_exiHeader, 4);
        }

        private void ReadImageSection(BinaryReader reader, long size)
        {
            m_Image = reader.ReadBytes(Convert.ToInt32(size));
        }

        private void ReadFunctionSection(BinaryReader reader, long size)
        {
            m_pifPrologue = ReadDWordArray(reader);
            m_pifEpilogue = ReadDWordArray(reader);

            var count = reader.ReadInt32();

            if (count > 0)
            {
                m_FunctionList = new List<FunctionNameItem>(count);

                for (var i = 0; i < count; i++)
                {
                    var item = new FunctionNameItem();

                    item.Addr = reader.ReadInt32();
                    item.Name = reader.ReadWideString();

                    m_FunctionList.Add(item);
                }
            }
        }

        private void ReadGlobalSection(BinaryReader reader, long size)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                m_csgGlobal = new ECSGlobal(count);

                for (var i = 0; i < count; i++)
                {
                    var name = reader.ReadWideString();
                    var obj = ReadObject(reader);

                    m_csgGlobal.Add(name, obj);
                }
            }
        }

        private void ReadDataSection(BinaryReader reader, long size)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                m_csgData = new ECSGlobal(count);

                for (var i = 0; i < count; i++)
                {
                    var name = reader.ReadWideString();

                    var length = reader.ReadInt32();

                    if (length >= 0)
                    {
                        var ns = new ECSGlobal(length);

                        for (var j = 0; j < length; j++)
                        {
                            var v1 = reader.ReadWideString();
                            var v2 = ReadObject(reader);
                            ns.Add(v1, v2);
                        }

                        m_csgData.Add(name, ns);
                    }
                    else
                    {
                        var obj = ReadObject(reader);
                        m_csgData.Add(name, obj);
                    }
                }
            }
        }

        private void ReadConstantStringSection(BinaryReader reader, long size)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                m_extConstStr = new TaggedRefAddressList(count);

                for (var i = 0; i < count; i++)
                {
                    var str = reader.ReadWideString();
                    var refs = ReadDWordArray(reader);

                    m_extConstStr.Add(str, refs);
                }
            }
            else
            {
                m_extConstStr = new();
            }
        }

        private void ReadLinkInformationSection(BinaryReader reader, long size)
        {
            m_extGlobalRef = ReadDWordArray(reader);
            m_extDataRef = ReadDWordArray(reader);
            m_impGlobalRef = ReadTagedDWordArray(reader);
            m_impDataRef = ReadTagedDWordArray(reader);

            if (m_extGlobalRef.Count != 0 || m_extDataRef.Count != 0 || m_impGlobalRef.Count != 0 || m_impDataRef.Count != 0)
            {
                Console.WriteLine("WARNING: The script file contains some data ( linker data ) that cannot be handle by this tool, which may cause script rebuild errors.");
            }
        }

        private static List<int> ReadDWordArray(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                var array = new List<int>(count);

                for (var i = 0; i < count; i++)
                {
                    array.Add(reader.ReadInt32());
                }

                return array;
            }

            return [];
        }

        private static TaggedRefAddressList ReadTagedDWordArray(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            if (count > 0)
            {
                var list = new TaggedRefAddressList(count);

                for (var i = 0; i < count; i++)
                {
                    var name = reader.ReadWideString();
                    var data = ReadDWordArray(reader);
                    list.Add(name, data);
                }

                return list;
            }
            else
            {
                var list = new TaggedRefAddressList();
                return list;
            }
        }

        private ECSObject ReadObject(BinaryReader reader)
        {
            var type = (CSVariableType)reader.ReadInt32();

            switch (type)
            {
                case CSVariableType.csvtObject:
                {
                    var className = reader.ReadWideString();
                    var obj = new ECSClassInfoObject(className);
                    return obj;
                }
                case CSVariableType.csvtReference:
                {
                    var obj = new ECSReference();
                    return obj;
                }
                case CSVariableType.csvtArray:
                {
                    var count = reader.ReadInt32();

                    if (count > 0)
                    {
                        var obj = new ECSArray(count);

                        for (var i = 0; i < count; i++)
                        {
                            var v1 = ReadObject(reader);
                            obj.Add(v1);
                        }

                        return obj;
                    }
                    else
                    {
                        var obj = new ECSArray();
                        return obj;
                    }
                }
                case CSVariableType.csvtHash:
                {
                    var obj = new ECSHash();
                    return obj;
                }
                case CSVariableType.csvtInteger:
                {
                    var val = (m_Header.IntBase == 64) ? reader.ReadInt64() : reader.ReadUInt32();
                    var obj = new ECSInteger(val);
                    return obj;
                }
                case CSVariableType.csvtReal:
                {
                    var val = reader.ReadDouble();
                    var obj = new ECSReal(val);
                    return obj;
                }
                case CSVariableType.csvtString:
                {
                    var str = reader.ReadWideString();
                    var obj = new ECSString(str);
                    return obj;
                }
                default:
                {
                    throw new Exception("Unknow variable type.");
                }
            }
        }
    }
}
