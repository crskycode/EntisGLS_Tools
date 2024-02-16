using CSXTool.ECS.Enums;
using CSXTool.ECS.Stuff;
using CSXTool.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

#pragma warning disable CS8602
#pragma warning disable CS8604

namespace CSXTool.ECS
{
    public partial class ECSExecutionImage
    {
        private EMCFileHeader? m_FileHeader;
        private byte[]? m_exiHeader;                    // ヘッダー
        private byte[]? m_Image;                        // 実行イメージバッファ
        private List<int>? m_pifPrologue;               // 初期化関数アドレス
        private List<int>? m_pifEpilogue;               // 終了関数アドレス
        private List<FunctionNameItem>? m_FunctionList; // 関数名連想アドレス配列
        private ECSGlobal? m_csgGlobal;                 // 大域変数、名前・型情報
        private ECSGlobal? m_csgData;                   // 大域定数、名前・型情報
        private TaggedRefAddressList? m_extConstStr;    // 固定文字列参照
        // リンク用データ
        // ※コードイメージ上から各記憶クラスに対して参照している
        // 　コードイメージ上の位置を記録した配列
        // 　コードイメージ上には参照するアドレスが格納されており、
        // 　リンク時にはコードイメージ上の値が修正される
        private List<int>? m_extGlobalRef;              // 大域変数参照リスト
        private List<int>? m_extDataRef;                // 大域定数参照リスト
        private TaggedRefAddressList? m_impGlobalRef;   // 外部参照（未解決変数）
        private TaggedRefAddressList? m_impDataRef;     // 外部参照（未解決定数）

        public ECSExecutionImage()
        {
            m_FunctionList = [];
            m_csgGlobal = new();
            m_csgData = new();
        }

        public void ExportText(string filePath)
        {
            Console.WriteLine("Parsing code...");

            var disasm = new ECSExecutionImageDisassembler(m_Image, m_FunctionList, null);
            disasm.Execute();

            var stream = new MemoryStream(m_Image);
            var reader = new BinaryReader(stream);

            var stack = new LinkedList<Tuple<int, string>>();

            Console.WriteLine("Generating text...");

            var writer = File.CreateText(filePath);

            foreach (var cmd in disasm.Assembly)
            {
                if (cmd.Code == CSInstructionCode.csicLoad)
                {
                    stream.Position = cmd.Addr + 1;

                    var csomType = (CSObjectMode)reader.ReadByte();
                    var csvtType = (CSVariableType)reader.ReadByte();

                    if (csomType == CSObjectMode.csomImmediate && csvtType == CSVariableType.csvtString)
                    {
                        var value = reader.ReadWideString();

                        stack.AddFirst(Tuple.Create(cmd.Addr, value));

                        if (stack.Count > 8)
                            stack.RemoveLast();
                    }
                }
                else if (cmd.Code == CSInstructionCode.csicCall)
                {
                    stream.Position = cmd.Addr + 1;

                    var csomType = (CSObjectMode)reader.ReadByte();
                    var numArgs = reader.ReadInt32();
                    var funcName = reader.ReadWideString();

                    if (numArgs == 1)
                    {
                        if (funcName == "Mess" || funcName == "SceneTitle")
                        {
                            var addr = stack.First().Item1;
                            var text = stack.First().Item2.Escape();

                            if (text.Length > 0 && text != "「" && text != "」" && !text.StartsWith('@'))
                            {
                                writer.WriteLine("◇{0:X8}◇{1}", addr, text);
                                writer.WriteLine("◆{0:X8}◆{1}", addr, text);
                                writer.WriteLine();
                            }
                        }
                    }
                    else if (numArgs == 2)
                    {
                        if (funcName == "Talk" || funcName == "AddSelect")
                        {
                            var addr = stack.First().Item1;
                            var text = stack.First().Item2.Escape();

                            if (text.Length > 0)
                            {
                                writer.WriteLine("◇{0:X8}◇{1}", addr, text);
                                writer.WriteLine("◆{0:X8}◆{1}", addr, text);
                                writer.WriteLine();
                            }
                        }
                    }
                }
            }

            writer.Flush();
            writer.Dispose();

            Console.WriteLine("Done.");
        }

        public void ExportAllText(string filePath)
        {
            Console.WriteLine("Parsing code...");

            var disasm = new ECSExecutionImageDisassembler(m_Image, m_FunctionList, null);
            disasm.Execute();

            var stream = new MemoryStream(m_Image);
            var reader = new BinaryReader(stream);

            Console.WriteLine("Generating text...");

            var writer = File.CreateText(filePath);

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
                        var text = reader.ReadWideString().Escape();

                        // writer.WriteLine("◇{0:X8}◇{1}", cmd.Addr, text);
                        // writer.WriteLine("◆{0:X8}◆{1}", cmd.Addr, text);
                        // writer.WriteLine();

                        writer.WriteLine("{0:X8} | \"{1}\"", cmd.Addr, text);
                    }
                }
            }

            writer.Flush();
            writer.Dispose();

            Console.WriteLine("Done.");
        }

        public void Disasm(string filePath)
        {
            var writer = File.CreateText(filePath);

            Console.WriteLine("Generating disassembly...");

            var disasm = new ECSExecutionImageDisassembler(m_Image, m_FunctionList, writer);
            disasm.Execute();

            writer.Flush();
            writer.Close();

            Console.WriteLine("Done.");
        }

        [GeneratedRegex(@"◆(\w+)◆(.+$)")]
        private static partial Regex TextLineRegex();

        private static Dictionary<long, string> LoadTranslation(string filePath)
        {
            using var reader = File.OpenText(filePath);

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

                dict.Add(addr, text);
            }

            reader.Close();

            return dict;
        }

        private void BuildImage(ECSExecutionImageAssembly assembly, BinaryReader reader, BinaryWriter writer, Dictionary<long, string> translation)
        {
            foreach (var cmd in assembly)
            {
                cmd.NewAddr = Convert.ToInt32(writer.BaseStream.Position);

                if (cmd.Code == CSInstructionCode.csicLoad)
                {
                    if (translation.TryGetValue(cmd.Addr, out string? text))
                    {
                        // Found the translation, now write new command.

                        reader.BaseStream.Position = cmd.Addr;

                        var code = (CSInstructionCode)reader.ReadByte();
                        var csomType = (CSObjectMode)reader.ReadByte();
                        var csvtType = (CSVariableType)reader.ReadByte();

                        if (code != CSInstructionCode.csicLoad || csomType != CSObjectMode.csomImmediate || csvtType != CSVariableType.csvtString)
                        {
                            throw new Exception("Wrong instruction code.");
                        }

                        writer.Write((byte)CSInstructionCode.csicLoad);
                        writer.Write((byte)CSObjectMode.csomImmediate);
                        writer.Write((byte)CSVariableType.csvtString);
                        writer.WriteWideString(text);
                    }
                    else
                    {
                        // No translation found. Write the original command.

                        var addr = Convert.ToInt32(cmd.Addr);
                        var size = Convert.ToInt32(cmd.Size);

                        writer.Write(m_Image, addr, size);
                    }
                }
                else
                {
                    // Write the original command.

                    var addr = Convert.ToInt32(cmd.Addr);
                    var size = Convert.ToInt32(cmd.Size);

                    writer.Write(m_Image, addr, size);
                }
            }
        }

        private void FixImage(ECSExecutionImageAssembly assembly, BinaryReader reader, BinaryWriter writer, Dictionary<int, ECSExecutionImageCommandRecord> commandMap)
        {
            foreach (var cmd in assembly)
            {
                if (cmd.Code == CSInstructionCode.csicEnter)
                {
                    reader.BaseStream.Position = cmd.Addr;

                    reader.ReadByte();  // code
                    reader.ReadWideString();  // name
                    var numArgs = reader.ReadInt32();

                    if (numArgs == -1)
                    {
                        reader.ReadByte(); // flag

                        // Since we haven't modified the Enter command, it has the same bytes as the original.
                        // So we can save this offset.
                        // Fix at this location.
                        var loc = reader.BaseStream.Position - cmd.Addr;

                        var catchAddr = reader.ReadInt32();
                        catchAddr += Convert.ToInt32(reader.BaseStream.Position);

                        // Calculate new destination address.
                        var targetCmd = commandMap[catchAddr];
                        var newAddr = Convert.ToInt32(targetCmd.NewAddr - (cmd.NewAddr + loc) - 4);

                        // Fix the catch address.
                        writer.BaseStream.Position = cmd.NewAddr + loc;
                        writer.Write(newAddr);
                    }
                }
                else if (cmd.Code == CSInstructionCode.csicJump)
                {
                    reader.BaseStream.Position = cmd.Addr;

                    reader.ReadByte(); // code

                    // Fix at this location.
                    var loc = reader.BaseStream.Position - cmd.Addr;

                    var addr = reader.ReadInt32();
                    addr += Convert.ToInt32(reader.BaseStream.Position);

                    // Calculate new destination address.
                    var targetCmd = commandMap[addr];
                    var newAddr = Convert.ToInt32(targetCmd.NewAddr - (cmd.NewAddr + loc) - 4);

                    // Fix jump destination.
                    writer.BaseStream.Position = cmd.NewAddr + loc;
                    writer.Write(newAddr);
                }
                else if (cmd.Code == CSInstructionCode.csicCJump)
                {
                    reader.BaseStream.Position = cmd.Addr;

                    reader.ReadByte(); // code
                    reader.ReadByte(); // cond

                    // Fix at this location.
                    var loc = reader.BaseStream.Position - cmd.Addr;

                    var addr = reader.ReadInt32();
                    addr += Convert.ToInt32(reader.BaseStream.Position);

                    // Calculate new destination address.
                    var targetCmd = commandMap[addr];
                    var newAddr = Convert.ToInt32(targetCmd.NewAddr - (cmd.NewAddr + loc) - 4);

                    // Fix jump destination.
                    writer.BaseStream.Position = cmd.NewAddr + loc;
                    writer.Write(newAddr);
                }
            }
        }

        private void FixReferences(Dictionary<int, ECSExecutionImageCommandRecord> commandMap)
        {
            for (var i = 0; i < m_pifPrologue.Count; i++)
            {
                m_pifPrologue[i] = commandMap[m_pifPrologue[i]].NewAddr;
            }

            for (var i = 0; i < m_pifEpilogue.Count; i++)
            {
                m_pifEpilogue[i] = commandMap[m_pifEpilogue[i]].NewAddr;
            }

            foreach (var e in m_FunctionList)
            {
                e.Addr = commandMap[e.Addr].NewAddr;
            }
        }

        public void ImportText(string filePath)
        {
            Console.WriteLine("Loading translation...");

            var translation = LoadTranslation(filePath);

            Console.WriteLine("Parsing code...");

            var disasm = new ECSExecutionImageDisassembler(m_Image, m_FunctionList, null);
            disasm.Execute();

            Console.WriteLine("Preparing to rebuild...");

            var readStream = new MemoryStream(m_Image);
            var codeReader = new BinaryReader(readStream);

            var estimatedSize = m_Image.Length + (m_Image.Length / 2);

            var writeStream = new MemoryStream(estimatedSize);
            var codeWriter = new BinaryWriter(writeStream);

            var commandMap = disasm.Assembly.ToDictionary(a => a.Addr);

            Console.WriteLine("Building image...");

            BuildImage(disasm.Assembly, codeReader, codeWriter, translation);

            Console.WriteLine("Fixing image...");

            FixImage(disasm.Assembly, codeReader, codeWriter, commandMap);

            m_Image = writeStream.ToArray();

            Console.WriteLine("Fixing references...");

            FixReferences(commandMap);

            Console.WriteLine("Rebuild finished.");
        }
    }
}
