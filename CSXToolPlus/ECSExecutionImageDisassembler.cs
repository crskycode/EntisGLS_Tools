using CSXToolPlus.Extensions;
using CSXToolPlus.Sections;
using CSXToolPlus.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSXToolPlus
{
    public class ECSExecutionImageDisassembler
    {
        private readonly MemoryStream _stream;
        private readonly BinaryReader _reader;
        private readonly ECSExecutionImageAssembly _assembly;
        private readonly StreamWriter? _writer;

        private readonly SectionFunction _sectionFunction;
        private readonly SectionFuncInfo _sectionFuncInfo;
        private readonly SectionImportNativeFunc _sectionImportNativeFunc;
        private readonly SectionClassInfo _sectionClassInfo;
        private readonly SectionConstString _sectionConstString;

        private readonly Dictionary<uint, FuncInfoEntry> _funcMap;

        private int _addr;
        private CSInstructionCode _code;

        public ECSExecutionImageDisassembler(
            byte[] data,
            SectionFunction sectionFunction,
            SectionFuncInfo sectionFuncInfo,
            SectionImportNativeFunc sectionImportNativeFunc,
            SectionClassInfo sectionClassInfo,
            SectionConstString sectionConstString,
            StreamWriter? writer)
        {
            // input
            _stream = new MemoryStream(data);
            _reader = new BinaryReader(_stream);
            // sections
            _sectionFunction = sectionFunction;
            _sectionFuncInfo = sectionFuncInfo;
            _sectionImportNativeFunc = sectionImportNativeFunc;
            _sectionClassInfo = sectionClassInfo;
            _sectionConstString = sectionConstString;
            // For fast lookup
            _funcMap = _sectionFuncInfo.Functions.ToDictionary(x => x.Header.Address);
            // output
            _assembly = new ECSExecutionImageAssembly();
            _writer = writer;
        }

        public ECSExecutionImageAssembly Assembly
        {
            get => _assembly;
        }

        private void Line(string line)
        {
            // Debug.WriteLine("{0:X8} | {1}", _addr, line);
            _writer?.WriteLine("{0:X8} | {1}", _addr, line);
        }

        public void Execute()
        {
            var ordered = _sectionFuncInfo.Functions.OrderBy(e => e.Header.Address);

            foreach (var e in ordered)
            {
                var startAddr = e.Header.Address;
                var endAddr = startAddr + e.Header.Bytes;

                ExecuteRange(startAddr, endAddr);
            }

            // Maybe it's an older format, try disassemble whole image.
            // Assuming there are no padding bytes.
            if (_sectionFuncInfo.Functions.Count == 0)
            {
                ExecuteRange(0, _stream.Length);
            }
        }

        public void ExecuteRange(long startAddr, long endAddr)
        {
            _stream.Position = startAddr;

            while (_stream.Position < endAddr)
            {
                _addr = Convert.ToInt32(_stream.Position);
                _code = (CSInstructionCode)_reader.ReadByte();

                switch (_code)
                {
                    case CSInstructionCode.csicNew:
                        CommandNew();
                        break;
                    case CSInstructionCode.csicFree:
                        CommandFree();
                        break;
                    case CSInstructionCode.csicLoad:
                        CommandLoad();
                        break;
                    case CSInstructionCode.csicStore:
                        CommandStore();
                        break;
                    case CSInstructionCode.csicEnter:
                        CommandEnter();
                        break;
                    case CSInstructionCode.csicLeave:
                        CommandLeave();
                        break;
                    case CSInstructionCode.csicJump:
                        CommandJump();
                        break;
                    case CSInstructionCode.csicCJump:
                        CommandCJump();
                        break;
                    case CSInstructionCode.csicCall:
                        CommandCall();
                        break;
                    case CSInstructionCode.csicReturn:
                        CommandReturn();
                        break;
                    case CSInstructionCode.csicElement:
                        CommandElement();
                        break;
                    case CSInstructionCode.csicElementIndirect:
                        CommandElementIndirect();
                        break;
                    case CSInstructionCode.csicOperate:
                        CommandOperate();
                        break;
                    case CSInstructionCode.csicUniOperate:
                        CommandUniOperate();
                        break;
                    case CSInstructionCode.csicCompare:
                        CommandCompare();
                        break;
                    case CSInstructionCode.csicExOperate:
                        CommandExOperate();
                        break;
                    case CSInstructionCode.csicExUniOperate:
                        CommandExUniOperate();
                        break;
                    case CSInstructionCode.csicExCall:
                        CommandExCall();
                        break;
                    case CSInstructionCode.csicExReturn:
                        CommandExReturn();
                        break;
                    case CSInstructionCode.csicCallMember:
                        CommandCallMember();
                        break;
                    case CSInstructionCode.csicCallNativeMember:
                        CommandCallNativeMember();
                        break;
                    case CSInstructionCode.csicSwap:
                        CommandSwap();
                        break;
                    case CSInstructionCode.csicReferenceForPointer:
                        CommandReferenceForPointer();
                        break;
                    case CSInstructionCode.csicCallNativeFunction:
                        CommandCallNativeFunction();
                        break;
                    case CSInstructionCode.codeLoadMemBaseIndex:
                        ShellCommandLoadMemBaseIndex();
                        break;
                    case CSInstructionCode.codeStoreMem:
                        ShellCommandStoreMem();
                        break;
                    case CSInstructionCode.codeStoreMemBaseIndex:
                        ShellCommandStoreMemBaseIndex();
                        break;
                    case CSInstructionCode.codeLoadLocal:
                        ShellCommandLoadLocal();
                        break;
                    case CSInstructionCode.codeStoreLocal:
                        ShellCommandStoreLocal();
                        break;
                    case CSInstructionCode.codeMoveReg:
                        ShellCommandMoveReg();
                        break;
                    case CSInstructionCode.codeSllImm8:
                        ShellCommandSllImm8();
                        break;
                    case CSInstructionCode.codeAddImm32:
                        ShellCommandAddImm32();
                        break;
                    case CSInstructionCode.codeMulImm32:
                        ShellCommandMulImm32();
                        break;
                    case CSInstructionCode.codeAddSPImm32:
                        ShellCommandAddSPImm32();
                        break;
                    case CSInstructionCode.codeLoadImm64:
                        ShellCommandLoadImm64();
                        break;
                    case CSInstructionCode.codeAddReg:
                        ShellCommandAddReg();
                        break;
                    case CSInstructionCode.codeSubReg:
                        ShellCommandSubReg();
                        break;
                    case CSInstructionCode.codeAndReg:
                        ShellCommandAndReg();
                        break;
                    case CSInstructionCode.codeOrReg:
                        ShellCommandOrReg();
                        break;
                    case CSInstructionCode.codeCmpNeReg:
                        ShellCommandCmpNeReg();
                        break;
                    case CSInstructionCode.codeCmpEqReg:
                        ShellCommandCmpEqReg();
                        break;
                    case CSInstructionCode.codeCmpLtReg:
                        ShellCommandCmpLtReg();
                        break;
                    case CSInstructionCode.codeCmpLeReg:
                        ShellCommandCmpLeReg();
                        break;
                    case CSInstructionCode.codeCmpGtReg:
                        ShellCommandCmpGtReg();
                        break;
                    case CSInstructionCode.codeCmpGeReg:
                        ShellCommandCmpGeReg();
                        break;
                    case CSInstructionCode.codeJumpOffset32:
                        ShellCommandJumpOffset32();
                        break;
                    case CSInstructionCode.codeCNJumpOffset32:
                        ShellCommandCNJumpOffset32();
                        break;
                    case CSInstructionCode.codeCJumpOffset32:
                        ShellCommandCJumpOffset32();
                        break;
                    case CSInstructionCode.codeCallImm32:
                        ShellCommandCallImm32();
                        break;
                    case CSInstructionCode.codeSysCallImm32:
                        ShellCommandSysCallImm32();
                        break;
                    case CSInstructionCode.codeReturn:
                        ShellCommandReturn();
                        break;
                    case CSInstructionCode.codePushReg:
                        ShellCommandPushReg();
                        break;
                    case CSInstructionCode.codePopReg:
                        ShellCommandPopReg();
                        break;
                    case CSInstructionCode.codePushRegs:
                        ShellCommandPushRegs();
                        break;
                    case CSInstructionCode.codePopRegs:
                        ShellCommandPopRegs();
                        break;
                    default:
                        throw new Exception("Unknow command.");
                }

                // _writer?.Flush();

                var size = Convert.ToInt32(_stream.Position - _addr);
                _assembly.Add(_code, _addr, size);
            }

            _writer?.Flush();
        }

        private string GetStringLiteral(out int index)
        {
            var length = _reader.ReadUInt32();

            if (length != 0x80000000)
            {
                _reader.BaseStream.Position -= 4;
                Console.WriteLine("WARNING: {0:X8} String literal found in code section!", _reader.BaseStream.Position);
                index = -1;
                return _reader.ReadWideString();
            }
            else
            {
                index = _reader.ReadInt32();
                return _sectionConstString.Strings[index].String;
            }
        }

        private string GetStringLiteral()
        {
            return GetStringLiteral(out _);
        }

        // 0x00 : New
        // Desc : Create a new variable.
        private void CommandNew()
        {
            var csomType = (CSObjectMode)_reader.ReadByte();
            var csvtType = (CSVariableType)_reader.ReadByte();

            var className = string.Empty;

            if (csvtType == CSVariableType.csvtClassObject)
            {
                var classIndex = _reader.ReadInt32();
                className = _sectionClassInfo.Names[classIndex];
            }
            else
            {
                if (csvtType == CSVariableType.csvtObject)
                    className = GetStringLiteral();
            }

            var varName = GetStringLiteral();

            string mode;

            switch (csomType)
            {
                case CSObjectMode.csomStack:
                    mode = "Stack";
                    break;
                case CSObjectMode.csomThis:
                    mode = "This";
                    break;
                default:
                    throw new Exception("Unexpected object mode.");
            }

            if (string.IsNullOrEmpty(className))
                Line($"New {mode} \"{varName}\"");
            else
                Line($"New {mode} \"{className}\" \"{varName}\"");
        }

        // 0x01 : Free
        // Desc : Pop an object from stack and destroy it.
        private void CommandFree()
        {
            Line($"Free");
        }

        // 0x02 : Load
        // Desc : Load an object then push to stack.
        private void CommandLoad()
        {
            var csomType = (CSObjectMode)_reader.ReadByte();
            var csvtType = (CSVariableType)_reader.ReadByte();

            if (csomType == CSObjectMode.csomImmediate)
            {
                // Load object from code section.

                switch (csvtType)
                {
                    case CSVariableType.csvtObject:
                    {
                        // Create a new object.
                        var className = GetStringLiteral();
                        Line($"Load New \"{className}\"");
                        break;
                    }
                    case CSVariableType.csvtReference:
                    {
                        // Create a new reference object.
                        Line($"Load New \"ECSReference\"");
                        break;
                    }
                    case CSVariableType.csvtArray:
                    {
                        // Create a new array object.
                        Line($"Load New \"ECSArray\"");
                        break;
                    }
                    case CSVariableType.csvtHash:
                    {
                        // Create a new hash object.
                        Line($"Load New \"ECSHash\"");
                        break;
                    }
                    case CSVariableType.csvtInteger:
                    {
                        // Create new integer object.
                        var value = _reader.ReadUInt32();
                        Line($"Load Integer {value}");
                        break;
                    }
                    case CSVariableType.csvtReal:
                    {
                        // Create new real object.
                        var value = _reader.ReadDouble();
                        Line($"Load Real {value}");
                        break;
                    }
                    case CSVariableType.csvtString:
                    {
                        // Create new string object.
                        var value = GetStringLiteral(out int index);

                        if (index != -1)
                            Line($"Load String \"{value.Escape()}\" ({index})");
                        else
                            Line($"Load String \"{value.Escape()}\"");

                        break;
                    }
                    case CSVariableType.csvtInteger64:
                    {
                        // Create new integer64 object.
                        var value = _reader.ReadUInt64();
                        Line($"Load Integer64 {value}");
                        break;
                    }
                    case CSVariableType.csvtPointer:
                    {
                        // Create new integer64 object.
                        var value = _reader.ReadUInt32();
                        Line($"Load Pointer {value}");
                        break;
                    }
                    case CSVariableType.csvtClassObject:
                    {
                        var classIndex = _reader.ReadInt32();
                        Line($"Load New \"{_sectionClassInfo.Names[classIndex]}\"");
                        break;
                    }
                    case CSVariableType.csvtBoolean:
                    {
                        var value = _reader.ReadByte();
                        Line($"Load Boolean {value}");
                        break;
                    }
                    default:
                    {
                        throw new InvalidDataException("Unexpected variable type.");
                    }
                }
            }
            else
            {
                string mode;

                switch (csomType)
                {
                    case CSObjectMode.csomStack:
                        mode = "Stack";
                        break;
                    case CSObjectMode.csomThis:
                        mode = "This";
                        break;
                    case CSObjectMode.csomGlobal:
                        mode = "Global";
                        break;
                    case CSObjectMode.csomData:
                        mode = "Data";
                        break;
                    case CSObjectMode.csomAuto:
                        mode = "Auto";
                        break;
                    default:
                        throw new InvalidDataException("Unexpected object mode.");
                }

                switch (csvtType)
                {
                    case CSVariableType.csvtReference:
                    {
                        Line($"Load {mode}");
                        break;
                    }
                    case CSVariableType.csvtInteger:
                    {
                        // Find property or element by index.
                        var index = _reader.ReadInt32();
                        Line($"Load {mode} [{index}]");
                        break;
                    }
                    case CSVariableType.csvtString:
                    {
                        // Find property or element by name.
                        var name = GetStringLiteral();
                        Line($"Load {mode} [\"{name}\"]");
                        break;
                    }
                    default:
                    {
                        throw new InvalidDataException("Unexpected variable type.");
                    }
                }
            }
        }

        // 0x03 : Store
        // Desc : Pop an object from stack, execute operate, store to stack.
        private void CommandStore()
        {
            var csotType = (CSOperatorType)_reader.ReadByte();

            switch (csotType)
            {
                case CSOperatorType.csotNop:
                    Line($"Store");
                    break;
                case CSOperatorType.csotAdd:
                    Line($"Store.Add");
                    break;
                case CSOperatorType.csotSub:
                    Line($"Store.Sub");
                    break;
                case CSOperatorType.csotMul:
                    Line($"Store.Mul");
                    break;
                case CSOperatorType.csotDiv:
                    Line($"Store.Div");
                    break;
                case CSOperatorType.csotMod:
                    Line($"Store.Mod");
                    break;
                case CSOperatorType.csotAnd:
                    Line($"Store.And");
                    break;
                case CSOperatorType.csotOr:
                    Line($"Store.Or");
                    break;
                case CSOperatorType.csotXor:
                    Line($"Store.Xor");
                    break;
                case CSOperatorType.csotLogicalAnd:
                    Line($"Store.LAnd");
                    break;
                case CSOperatorType.csoutLogicalOr:
                    Line($"Store.LOr");
                    break;
                default:
                    throw new InvalidDataException("Unknow store operator.");
            }
        }

        // 0x04 : Enter
        // Desc : Enter into a namespace
        private void CommandEnter()
        {
            var name = GetStringLiteral();
            var numArgs = _reader.ReadInt32();

            if (numArgs != -1)
            {
                var sb = new StringBuilder();

                sb.Append('(');

                for (var i = 0; i < numArgs; i++)
                {
                    var csvtType = (CSVariableType)_reader.ReadByte();

                    var className = string.Empty;
                    string varName;

                    if (csvtType == CSVariableType.csvtClassObject)
                    {
                        var classIndex = _reader.ReadInt32();
                        className = _sectionClassInfo.Names[classIndex];
                    }
                    else
                    {
                        if (csvtType == CSVariableType.csvtObject)
                            className = GetStringLiteral();
                    }

                    varName = GetStringLiteral();

                    if (string.IsNullOrEmpty(className))
                        sb.Append(varName);
                    else
                        sb.AppendFormat("{{{0}:{1}}}", className, varName);

                    if (i < numArgs - 1)
                        sb.Append(", ");
                }

                sb.Append(')');

                Line($"Enter \"{name}\" {sb}");
            }
            else
            {
                var flag = _reader.ReadByte();

                if (flag != 0)
                {
                    throw new InvalidDataException("定義されていない拡張名前空間命令を実行しようとしました。");
                }

                var catchAddr = (long)_reader.ReadInt32();
                catchAddr += _stream.Position;

                Line($"Enter \"{name}\" Try-Catch {catchAddr:X8}");
            }
        }

        // 0x05 : Leave
        // Desc : Leave from a namespace
        private void CommandLeave()
        {
            Line($"Leave");
        }

        // 0x06 : Jump
        // Desc : Unconditional Jump
        private void CommandJump()
        {
            var addr = (long)_reader.ReadInt32();
            addr += _stream.Position;

            Line($"Jump {addr:X8}");
        }

        // 0x07 : CJump
        // Desc : Conditional jump.
        private void CommandCJump()
        {
            var cond = _reader.ReadByte();

            var addr = (long)_reader.ReadInt32();
            addr += _stream.Position;

            Line($"CJump {cond} {addr:X8}");
        }

        // 0x08 : Call
        // Desc : Push return address and arguments to stack, then jump to function.
        private void CommandCall()
        {
            var csomType = (CSObjectMode)_reader.ReadByte();
            var numArgs = _reader.ReadInt32();
            var funcName = GetStringLiteral();

            var mode = string.Empty;

            switch (csomType)
            {
                case CSObjectMode.csomImmediate:
                    if (funcName != "@CATCH")
                        throw new InvalidDataException("Unexpected object mode.");
                    break;
                case CSObjectMode.csomThis:
                    mode = "This";
                    break;
                case CSObjectMode.csomGlobal:
                    mode = "Global";
                    break;
                case CSObjectMode.csomAuto:
                    mode = "Auto";
                    break;
                default:
                    throw new InvalidDataException("Unexpected object mode.");
            }

            Line($"Call {mode} \"{funcName}\" <{numArgs}>");
        }

        // 0x09 : Return
        // Desc : Return
        private void CommandReturn()
        {
            var freeStack = _reader.ReadByte();

            if (freeStack == 1)
                Line($"Return Void");
            else
                Line($"Return");
        }

        // 0x0A : Element
        // Desc : References an array element or object member.
        private void CommandElement()
        {
            var csvtType = (CSVariableType)_reader.ReadByte();

            switch (csvtType)
            {
                case CSVariableType.csvtInteger:
                {
                    var index = _reader.ReadInt32();
                    Line($"Element [{index}]");
                    break;
                }
                case CSVariableType.csvtString:
                {
                    var name = GetStringLiteral();
                    Line($"Element [\"{name}\"]");
                    break;
                }
                default:
                {
                    throw new InvalidDataException("Unexpected variable type.");
                }
            }
        }

        // 0x0B : ElementIndirect
        // Desc : References an array element or object member.
        private void CommandElementIndirect()
        {
            Line($"ElementIndirect");
        }

        // 0x0C : Operate
        // Desc : 
        private void CommandOperate()
        {
            var csotType = (CSOperatorType)_reader.ReadByte();

            switch (csotType)
            {
                case CSOperatorType.csotNop:
                    Line($"Operate.Nop");
                    break;
                case CSOperatorType.csotAdd:
                    Line($"Operate.Add");
                    break;
                case CSOperatorType.csotSub:
                    Line($"Operate.Sub");
                    break;
                case CSOperatorType.csotMul:
                    Line($"Operate.Mul");
                    break;
                case CSOperatorType.csotDiv:
                    Line($"Operate.Div");
                    break;
                case CSOperatorType.csotMod:
                    Line($"Operate.Mod");
                    break;
                case CSOperatorType.csotAnd:
                    Line($"Operate.And");
                    break;
                case CSOperatorType.csotOr:
                    Line($"Operate.Or");
                    break;
                case CSOperatorType.csotXor:
                    Line($"Operate.Xor");
                    break;
                case CSOperatorType.csotLogicalAnd:
                    Line($"Operate.LAnd");
                    break;
                case CSOperatorType.csoutLogicalOr:
                    Line($"Operate.LOr");
                    break;
                case CSOperatorType.csotShiftRight:
                    Line($"Operate.ShiftRight");
                    break;
                case CSOperatorType.csotShiftLeft:
                    Line($"Operate.ShiftLeft");
                    break;
                default:
                    throw new InvalidDataException("Unknow operator.");
            }
        }

        // 0x0D : UniOperate
        // Desc :
        private void CommandUniOperate()
        {
            var csuotType = (CSUnaryOperatorType)_reader.ReadByte();

            switch (csuotType)
            {
                case CSUnaryOperatorType.csuotPlus:
                    Line($"UnaryOperate.Plus");
                    break;
                case CSUnaryOperatorType.csuotNegate:
                    Line($"UnaryOperate.Negate");
                    break;
                case CSUnaryOperatorType.csuotBitNot:
                    Line($"UnaryOperate.BitNot");
                    break;
                case CSUnaryOperatorType.csuotLogicalNot:
                    Line($"UnaryOperate.LogicalNot");
                    break;
                default:
                    throw new InvalidDataException("Unknow unary operator.");
            }
        }

        // 0x0E : Compare
        // Desc : 
        private void CommandCompare()
        {
            var csctType = (CSCompareType)_reader.ReadByte();

            switch (csctType)
            {
                case CSCompareType.csctNotEqual:
                    Line($"Compare.NotEqual");
                    break;
                case CSCompareType.csctEqual:
                    Line($"Compare.Equal");
                    break;
                case CSCompareType.csctLessThan:
                    Line($"Compare.LessThan");
                    break;
                case CSCompareType.csctLessEqual:
                    Line($"Compare.LessEqual");
                    break;
                case CSCompareType.csctGreaterThan:
                    Line($"Compare.GreaterThan");
                    break;
                case CSCompareType.csctGreaterEqual:
                    Line($"Compare.GreaterEqual");
                    break;
                case CSCompareType.csctNotEqualPointer:
                    Line($"Compare.NotEqualPointer");
                    break;
                case CSCompareType.csctEqualPointer:
                    Line($"Compare.EqualPointer");
                    break;
                default:
                    throw new InvalidDataException("Unknown compare operator.");
            }
        }

        // 0x0F : ExOperate
        // Desc : 
        private void CommandExOperate()
        {
            var csxotType = (CSExtraOperatorType)_reader.ReadByte();

            switch (csxotType)
            {
                case CSExtraOperatorType.csxotArrayDim:
                {
                    var dim = _reader.ReadInt32();

                    var sb = new StringBuilder();

                    for (var i = 0; i < dim; i++)
                    {
                        var v1 = _reader.ReadInt32();
                        sb.Append(v1);
                        if (i < dim - 1)
                            sb.Append(", ");
                    }

                    Line($"ExOperate.ArrayDim {{ {sb} }}");

                    break;
                }
                case CSExtraOperatorType.csxotHashContainer:
                {
                    Line($"ExOperate.HashContainer");
                    break;
                }
                case CSExtraOperatorType.csxotMoveReference:
                {
                    Line($"ExOperate.MoveReference");
                    break;
                }
                default:
                {
                    throw new InvalidDataException("Unknow extra operator.");
                }
            }
        }

        // 0x10 : ExUniOperate
        // Desc : 
        private void CommandExUniOperate()
        {
            var csxuotType = (CSExtraUniOperatorType)_reader.ReadByte();

            switch (csxuotType)
            {
                case CSExtraUniOperatorType.csxuotDeselect:
                {
                    Line("ExUniOperate.Deselect");
                    break;
                }
                case CSExtraUniOperatorType.csxuotBoolean:
                {
                    Line("ExUniOperate.Boolean");
                    break;
                }
                case CSExtraUniOperatorType.csxuotSizeOf:
                {
                    Line("ExUniOperate.SizeOf");
                    break;
                }
                case CSExtraUniOperatorType.csxuotTypeOf:
                {
                    Line("ExUniOperate.TypeOf");
                    break;
                }
                case CSExtraUniOperatorType.csxuotStaticCast:
                {
                    var varOffset = _reader.ReadInt32();
                    var varBounds = _reader.ReadInt32();
                    var funcOffset = _reader.ReadInt32();
                    Line($"ExUniOperate.StaticCast {varOffset}, {varBounds}, {funcOffset}");
                    break;
                }
                case CSExtraUniOperatorType.csxuotDynamicCast:
                {
                    var castType = GetStringLiteral();
                    Line($"ExUniOperate.DynamicCast \"{castType}\"");
                    break;
                }
                case CSExtraUniOperatorType.csxuotDuplicate:
                {
                    Line("ExUniOperate.Duplicate");
                    break;
                }
                case CSExtraUniOperatorType.csxuotDelete:
                {
                    Line("ExUniOperate.Delete");
                    break;
                }
                case CSExtraUniOperatorType.csxuotDeleteArray:
                {
                    Line("ExUniOperate.DeleteArray");
                    break;
                }
                case CSExtraUniOperatorType.csxuotLoadAddress:
                {
                    Line("ExUniOperate.LoadAddress");
                    break;
                }
                case CSExtraUniOperatorType.csxuotRefAddress:
                {
                    Line("ExUniOperate.RefAddress");
                    break;
                }
                default:
                {
                    throw new InvalidDataException("Unknow extra operator.");
                }
            }
        }

        // 0x11 : ExCall
        // Desc : 
        private void CommandExCall()
        {
            var argCount = _reader.ReadInt32();
            var csomType = (CSObjectMode)_reader.ReadByte();
            var csvtType = (CSVariableType)_reader.ReadByte();

            if (csomType == CSObjectMode.csomImmediate)
            {
                if (csvtType == CSVariableType.csvtString)
                {
                    var funcName = GetStringLiteral();
                    Line($"ExCall \"{funcName}\" <{argCount}>");
                }
                else if (csvtType == CSVariableType.csvtInteger)
                {
                    var funcAddr = _reader.ReadUInt32();

                    if (_funcMap.TryGetValue(funcAddr, out FuncInfoEntry? func))
                        Line($"ExCall \"{func.Name}\" <{argCount}>");
                    else
                        Line($"ExCall {funcAddr:X8} <{argCount}>");
                }
            }
            else
            {
                throw new InvalidDataException("Unexpected object mode.");
            }
        }

        // 0x12 : ExReturn
        // Desc : 
        private void CommandExReturn()
        {
            byte bytFreeStack = _reader.ReadByte();

            if (bytFreeStack == 1)
                Line($"ExReturn Void");
            else
                Line($"ExReturn");
        }

        // 0x13 : CallMember
        // Desc : 
        private void CommandCallMember()
        {
            var argCount = _reader.ReadInt32();
            var classIndex = _reader.ReadInt32();
            var funcIndex = _reader.ReadInt32();
            Line($"CallMember \"{_sectionClassInfo.Infos[classIndex].MethodInfo[funcIndex].PrototypeInfo.GlobalName}\" <{argCount}>");
        }

        // 0x14 : CallNativeMember
        // Desc : 
        private void CommandCallNativeMember()
        {
            var argCount = _reader.ReadInt32();
            var classIndex = _reader.ReadInt32();
            var funcIndex = _reader.ReadInt32();
            Line($"CallNativeMember \"{_sectionClassInfo.Infos[classIndex].MethodInfo[funcIndex].PrototypeInfo.GlobalName}\" <{argCount}>");
        }

        // 0x15 : Swap
        // Desc : 
        private void CommandSwap()
        {
            var bytSubCode = _reader.ReadByte();
            var index1 = _reader.ReadInt32();
            var index2 = _reader.ReadInt32();
            Line($"Swap #{index1}, #{index2}");
        }

        // 0x16 : ReferenceForPointer
        // Desc : 
        private void CommandReferenceForPointer()
        {
            var csvtRefType = (CSVariableType)_reader.ReadByte();

            if (Enum.IsDefined(csvtRefType))
                Line($"ReferenceForPointer {csvtRefType}");
            else
                throw new InvalidDataException("Unexpected variable type.");
        }

        // 0x1D : CallNativeFunction
        // Desc : 
        private void CommandCallNativeFunction()
        {
            var argCount = _reader.ReadInt32();
            var funcIndex = _reader.ReadInt32();
            Line($"CallNativeFunction \"{_sectionImportNativeFunc.NativeFunc.Names.Elements[funcIndex]}\" <{argCount}>");
        }

        // 0x82 : LoadMemBaseIndex
        // Desc : 
        private void ShellCommandLoadMemBaseIndex()
        {
            var data_type = _reader.ReadByte();
            var index = _reader.ReadByte();
            var reg = _reader.ReadByte();
            Line($"LoadMemBaseIndex {data_type}, %{reg}, {index}");
        }

        // 0x84 : StoreMem
        // Desc : 
        private void ShellCommandStoreMem()
        {
            var data_type = _reader.ReadByte();
            var reg = _reader.ReadByte();
            Line($"StoreMem {data_type}, %{reg}");
        }

        // 0x86 : StoreMemBaseIndex
        // Desc : 
        private void ShellCommandStoreMemBaseIndex()
        {
            var data_type = _reader.ReadByte();
            var index = _reader.ReadByte();
            var reg = _reader.ReadByte();
            Line($"StoreMemBaseIndex {data_type}, %{reg}, {index}");
        }

        // 0x88 : LoadLocal
        // Desc : 
        private void ShellCommandLoadLocal()
        {
            var data_type = _reader.ReadByte();
            var reg = _reader.ReadByte();
            var mem = _reader.ReadInt32();
            Line($"LoadLocal {data_type}, %{reg}, {mem}");
        }

        // 0x8A : StoreLocal
        // Desc : 
        private void ShellCommandStoreLocal()
        {
            var data_type = _reader.ReadByte();
            var reg = _reader.ReadByte();
            var mem = _reader.ReadInt32();
            Line($"StoreLocal {data_type}, %{reg}, {mem}");
        }

        // 0x90 : MoveReg
        // Desc : 
        private void ShellCommandMoveReg()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            Line($"MoveReg %{dst}, %{src}");
        }

        // 0x96 : SllImm8
        // Desc : 
        private void ShellCommandSllImm8()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            var num = _reader.ReadByte();
            Line($"SllImm8 %{dst}, %{src}, {num}");
        }

        // 0x98 : AddImm32
        // Desc : 
        private void ShellCommandAddImm32()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            var val = _reader.ReadInt32();
            Line($"AddImm32 %{dst}, %{src}, {val}");
        }

        // 0x99 : MulImm32
        // Desc : 
        private void ShellCommandMulImm32()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            var val = _reader.ReadInt32();
            Line($"MulImm32 %{dst}, %{src}, {val}");
        }

        // 0x9A : AddSPImm32
        // Desc : 
        private void ShellCommandAddSPImm32()
        {
            var val = _reader.ReadInt32();
            Line($"AddSPImm32 {val}");
        }

        // 0x9B : LoadImm64
        // Desc : 
        private void ShellCommandLoadImm64()
        {
            var reg = _reader.ReadByte();
            var val = _reader.ReadInt64();
            Line($"LoadImm64 %{reg}, {val}");
        }

        // 0xA0 : AddReg
        // Desc : 
        private void ShellCommandAddReg()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            Line($"AddReg %{dst}, %{src}");
        }

        // 0xA1 : SubReg
        // Desc : 
        private void ShellCommandSubReg()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            Line($"SubReg %{dst}, %{src}");
        }

        // 0xA5 : AndReg
        // Desc : 
        private void ShellCommandAndReg()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            Line($"AndReg %{dst}, %{src}");
        }

        // 0xA6 : OrReg
        // Desc : 
        private void ShellCommandOrReg()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            Line($"OrReg %{dst}, %{src}");
        }

        // 0xC0 : CmpNeReg
        // Desc : 
        private void ShellCommandCmpNeReg()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            Line($"CmpNeReg %{dst}, %{src}");
        }

        // 0xC1 : CmpEqReg
        // Desc : 
        private void ShellCommandCmpEqReg()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            Line($"CmpLtReg %{dst}, %{src}");
        }

        // 0xC2 : CmpLtReg
        // Desc : 
        private void ShellCommandCmpLtReg()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            Line($"CmpLtReg %{dst}, %{src}");
        }

        // 0xC3 : CmpLeReg
        // Desc : 
        private void ShellCommandCmpLeReg()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            Line($"CmpLeReg %{dst}, %{src}");
        }

        // 0xC4 : CmpGtReg
        // Desc : 
        private void ShellCommandCmpGtReg()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            Line($"CmpGtReg %{dst}, %{src}");
        }

        // 0xC5 : CmpGeReg
        // Desc : 
        private void ShellCommandCmpGeReg()
        {
            var dst = _reader.ReadByte();
            var src = _reader.ReadByte();
            Line($"CmpGeReg %{dst}, %{src}");
        }

        // 0xD0 : JumpOffset32
        // Desc : 
        private void ShellCommandJumpOffset32()
        {
            var offset = _reader.ReadInt32();
            var dest = _addr + offset + 5;
            Line($"JumpOffset32 {dest:X8}");
        }

        // 0xD3 : CNJumpOffset32
        // Desc : 
        private void ShellCommandCNJumpOffset32()
        {
            var reg = _reader.ReadByte();
            var offset = _reader.ReadInt32();
            var dest = _addr + offset + 6;
            Line($"CNJumpOffset32 %{reg}, {dest:X8}");
        }

        // 0xD3 : CJumpOffset32
        // Desc : 
        private void ShellCommandCJumpOffset32()
        {
            var reg = _reader.ReadByte();
            var offset = _reader.ReadInt32();
            var dest = _addr + offset + 6;
            Line($"CJumpOffset32 %{reg}, {dest:X8}");
        }

        // 0xD4 : CallImm32
        // Desc : 
        private void ShellCommandCallImm32()
        {
            var dst = _reader.ReadUInt32();

            if (_funcMap.TryGetValue(dst, out FuncInfoEntry? func))
                Line($"CallImm32 \"{func.Name}\"");
            else
                Line($"CallImm32 {dst:X8}");
        }

        // 0xD6 : SysCallImm32
        // Desc : 
        private void ShellCommandSysCallImm32()
        {
            var num = _reader.ReadInt32();
            Line($"SysCallImm32 0x{num:X2}");
        }

        // 0xD8 : Return
        // Desc : 
        private void ShellCommandReturn()
        {
            Line($"Return");
        }

        // 0xDC : PushReg
        // Desc : 
        private void ShellCommandPushReg()
        {
            var reg = _reader.ReadByte();
            Line($"PushReg %{reg}");
        }

        // 0xDD : PopReg
        // Desc : 
        private void ShellCommandPopReg()
        {
            var reg = _reader.ReadByte();
            Line($"PopReg %{reg}");
        }

        // 0xDE : PushRegs
        // Desc : 
        private void ShellCommandPushRegs()
        {
            var reg_first = _reader.ReadByte();
            var count = _reader.ReadByte();
            Line($"PushRegs %{reg_first}, {count}");
        }

        // 0xDF : PopRegs
        // Desc : 
        private void ShellCommandPopRegs()
        {
            var reg_first = _reader.ReadByte();
            var count = _reader.ReadByte();
            Line($"PopRegs %{reg_first}, {count}");
        }
    }
}
