using CSXTool.ECS.Enums;
using CSXTool.ECS.Stuff;
using CSXTool.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

#pragma warning disable CS8602
#pragma warning disable CS8604

namespace CSXTool.ECS
{
    public partial class ECSExecutionImage
    {
        public void Save(string filePath)
        {
            using var stream = File.Create(filePath);
            using var writer = new BinaryWriter(stream);

            writer.Write(m_FileHeader.Signature);
            writer.Write(m_FileHeader.FileId);
            writer.Write(m_FileHeader.Reserved);
            writer.Write(m_FileHeader.FormatDescription);

            // "header"
            if (m_exiHeader != null)
                WriteSection(writer, 0x2020726564616568u, WriteHeaderSection);
            // "image"
            WriteSection(writer, 0x2020206567616D69u, WriteImageSection);
            // "function"
            WriteSection(writer, 0x6E6F6974636E7566u, WriteFunctionSection);
            // "global"
            WriteSection(writer, 0x20206C61626F6C67u, WriteGlobalSection);
            // "data"
            WriteSection(writer, 0x2020202061746164u, WriteDataSection);
            // "conststr"
            if (m_extConstStr != null)
                WriteSection(writer, 0x72747374736E6F63u, WriteConstantStringSection);
            // "linkinf"
            WriteSection(writer, 0x20666E696B6E696Cu, WriteLinkInformationSection);

            writer.Flush();
            writer.Close();
        }

        private static void WriteSection(BinaryWriter writer, ulong id, Action<BinaryWriter> action)
        {
            writer.Write(id);
            var v1 = writer.BaseStream.Position;
            writer.Write(0L);
            var v2 = writer.BaseStream.Position;
            action(writer);
            var v3 = writer.BaseStream.Position;
            var size = v3 - v2;
            writer.BaseStream.Position = v1;
            writer.Write(size);
            writer.BaseStream.Position = v3;
        }

        private void WriteHeaderSection(BinaryWriter writer)
        {
            writer.Write(m_exiHeader);
        }

        private void WriteImageSection(BinaryWriter writer)
        {
            writer.Write(m_Image);
        }

        private void WriteFunctionSection(BinaryWriter writer)
        {
            WriteDWordArray(writer, m_pifPrologue);
            WriteDWordArray(writer, m_pifEpilogue);

            writer.Write(m_FunctionList.Count);

            for (var i = 0; i < m_FunctionList.Count; i++)
            {
                writer.Write(m_FunctionList[i].Addr);
                writer.WriteWideString(m_FunctionList[i].Name);
            }
        }

        private void WriteGlobalSection(BinaryWriter writer)
        {
            writer.Write(m_csgGlobal.Count);

            for (var i = 0; i < m_csgGlobal.Count; i++)
            {
                var e = m_csgGlobal[i];
                writer.WriteWideString(e.Name);
                WriteObject(writer, e.Obj);
            }
        }

        private void WriteDataSection(BinaryWriter writer)
        {
            writer.Write(m_csgData.Count);

            for (var i = 0; i < m_csgData.Count; i++)
            {
                var e = m_csgData[i];

                writer.WriteWideString(e.Name);

                if (e.Obj is ECSGlobal ns)
                {
                    writer.Write(ns.Count);

                    for (var j = 0; j < ns.Count; j++)
                    {
                        var v1 = ns[j];

                        writer.WriteWideString(v1.Name);
                        WriteObject(writer, v1.Obj);
                    }
                }
                else
                {
                    writer.Write(0x80000000);
                    WriteObject(writer, e.Obj);
                }
            }
        }

        private void WriteConstantStringSection(BinaryWriter writer)
        {
            writer.Write(m_extConstStr.Count);

            for (var i = 0; i < m_extConstStr.Count; i++)
            {
                var e = m_extConstStr[i];
                writer.WriteWideString(e.Tag);
                WriteDWordArray(writer, e.Refs);
            }
        }

        private void WriteLinkInformationSection(BinaryWriter writer)
        {
            WriteDWordArray(writer, m_extGlobalRef);
            WriteDWordArray(writer, m_extDataRef);
            WriteTagedDWordArray(writer, m_impGlobalRef);
            WriteTagedDWordArray(writer, m_impDataRef);
        }

        private static void WriteDWordArray(BinaryWriter writer, List<int> array)
        {
            writer.Write(array.Count);

            for (var i = 0; i < array.Count; i++)
            {
                writer.Write(array[i]);
            }
        }

        private static void WriteTagedDWordArray(BinaryWriter writer, TaggedRefAddressList list)
        {
            writer.Write(list.Count);

            for (var i = 0; i < list.Count; i++)
            {
                var e = list[i];

                writer.WriteWideString(e.Tag);

                WriteDWordArray(writer, e.Refs);
            }
        }

        private void WriteObject(BinaryWriter writer, ECSObject obj)
        {
            if (obj is ECSClassInfoObject v1)
            {
                writer.Write((int)CSVariableType.csvtObject);
                writer.WriteWideString(v1.ClassName);
            }
            else if (obj is ECSReference)
            {
                writer.Write((int)CSVariableType.csvtReference);
            }
            else if (obj is ECSArray v3)
            {
                writer.Write((int)CSVariableType.csvtArray);

                writer.Write(v3.Count);

                for (var i = 0; i < v3.Count; i++)
                {
                    WriteObject(writer, v3[i]);
                }
            }
            else if (obj is ECSHash)
            {
                writer.Write((int)CSVariableType.csvtHash);
            }
            else if (obj is ECSInteger v5)
            {
                writer.Write((int)CSVariableType.csvtInteger);

                if (m_Header.IntBase == 64)
                    writer.Write(v5.Value);
                else
                    writer.Write((int)v5.Value);
            }
            else if (obj is ECSReal v6)
            {
                writer.Write((int)CSVariableType.csvtReal);
                writer.Write(v6.Value);
            }
            else if (obj is ECSString v7)
            {
                writer.Write((int)CSVariableType.csvtString);
                writer.WriteWideString(v7.Value);
            }
            else
            {
                throw new Exception("Unknown object type.");
            }
        }
    }
}
