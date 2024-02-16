using CSXTool.ECS.Enums;
using CSXTool.ECS.Stuff;
using CSXTool.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSXTool.ECS
{
    public class ECSExecutionImageDisassembler
    {
        private readonly MemoryStream _stream;
        private readonly BinaryReader _reader;
        private readonly TaggedRefAddressList? _constStr;
        private readonly ECSExecutionImageAssembly _assembly;
        private readonly StreamWriter? _writer;

        private int _addr;
        private CSInstructionCode _code;

        public ECSExecutionImageDisassembler(byte[] data, List<FunctionNameItem> functionList, TaggedRefAddressList? constStr, StreamWriter? writer)
        {
            // input
            _stream = new MemoryStream(data);
            _reader = new BinaryReader(_stream);
            _constStr = constStr;
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
            _writer?.WriteLine("{0:X8} | {1}", _addr, line);
        }

        public void Execute()
        {
            while (_stream.Position < _stream.Length)
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
                    default:
                        throw new Exception("Unknow command.");
                }

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
                index = -1;
                return _reader.ReadWideString();
            }
            else
            {
                index = _reader.ReadInt32();
                return _constStr[index].Tag;
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
            if (csvtType == CSVariableType.csvtObject)
                className = GetStringLiteral();

            var name = GetStringLiteral();

            var pObj = string.Empty;

            switch (csomType)
            {
                case CSObjectMode.csomStack:
                    pObj = "stack";
                    break;
                case CSObjectMode.csomThis:
                    pObj = "this";
                    break;
                default:
                    throw new Exception("Unknow object mode.");
            }

            if (string.IsNullOrEmpty(className))
                Line($"New {pObj} \"{name}\"");
            else
                Line($"New {pObj} \"{className}\" \"{name}\"");
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
                        Line($"Load * {className}");
                        break;
                    }
                    case CSVariableType.csvtReference:
                    {
                        // Create a new reference object.
                        Line($"Load * ECSReference");
                        break;
                    }
                    case CSVariableType.csvtArray:
                    {
                        // Create a new array object.
                        Line($"Load * ECSArray");
                        break;
                    }
                    case CSVariableType.csvtHash:
                    {
                        // Create a new hash object.
                        Line($"Load * ECSHash");
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
                            Line($"Load Const String {index} \"{value.Escape()}\"");
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
                    default:
                    {
                        throw new Exception("Unknow object type.");
                    }
                }
            }
            else
            {
                var pObj = string.Empty;

                switch (csomType)
                {
                    case CSObjectMode.csomStack:
                        pObj = "stack";
                        break;
                    case CSObjectMode.csomThis:
                        pObj = "this";
                        break;
                    case CSObjectMode.csomGlobal:
                        pObj = "global";
                        break;
                    case CSObjectMode.csomData:
                        pObj = "data";
                        break;
                    case CSObjectMode.csomAuto:
                        pObj = "auto";
                        break;
                    default:
                        throw new Exception("Unknow object mode.");
                }

                switch (csvtType)
                {
                    case CSVariableType.csvtReference:
                    {
                        Line($"Load {pObj}");
                        break;
                    }
                    case CSVariableType.csvtInteger:
                    {
                        // Find property or element by index.
                        var index = _reader.ReadInt32();
                        Line($"Load {pObj} [{index}]");
                        break;
                    }
                    case CSVariableType.csvtString:
                    {
                        // Find property or element by name.
                        var name = GetStringLiteral();
                        Line($"Load {pObj} [\"{name}\"]");
                        break;
                    }
                    default:
                    {
                        throw new Exception("Unknow object type.");
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
                    Line($"Store Add");
                    break;
                case CSOperatorType.csotSub:
                    Line($"Store Sub");
                    break;
                case CSOperatorType.csotMul:
                    Line($"Store Mul");
                    break;
                case CSOperatorType.csotDiv:
                    Line($"Store Div");
                    break;
                case CSOperatorType.csotMod:
                    Line($"Store Mod");
                    break;
                case CSOperatorType.csotAnd:
                    Line($"Store And");
                    break;
                case CSOperatorType.csotOr:
                    Line($"Store Or");
                    break;
                case CSOperatorType.csotXor:
                    Line($"Store Xor");
                    break;
                case CSOperatorType.csotLogicalAnd:
                    Line($"Store LAnd");
                    break;
                case CSOperatorType.csoutLogicalOr:
                    Line($"Store LOr");
                    break;
                default:
                    throw new Exception("Unknow operator.");
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
                    if (csvtType == CSVariableType.csvtObject)
                        className = GetStringLiteral();

                    var varName = GetStringLiteral();

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
                    throw new Exception("定義されていない拡張名前空間命令を実行しようとしました。");
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

            var pObj = string.Empty;

            switch (csomType)
            {
                case CSObjectMode.csomImmediate:
                    if (funcName != "@CATCH")
                        throw new Exception("Unknow object mode.");
                    break;
                case CSObjectMode.csomThis:
                    pObj = "this";
                    break;
                case CSObjectMode.csomAuto:
                    pObj = "auto";
                    break;
                default:
                    throw new Exception("Unknow object mode.");
            }

            Line($"Call {pObj} \"{funcName}\" <{numArgs}>");
        }

        // 0x09 : Return
        // Desc : Return
        private void CommandReturn()
        {
            var freeStack = _reader.ReadByte();
            Line($"Return {freeStack}");
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
                    Line($"Element {index}");
                    break;
                }
                case CSVariableType.csvtString:
                {
                    var name = GetStringLiteral();
                    Line($"Element \"{name}\"");
                    break;
                }
                default:
                {
                    throw new Exception("Unknow object type.");
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
                    Line($"Operate Nop");
                    break;
                case CSOperatorType.csotAdd:
                    Line($"Operate Add");
                    break;
                case CSOperatorType.csotSub:
                    Line($"Operate Sub");
                    break;
                case CSOperatorType.csotMul:
                    Line($"Operate Mul");
                    break;
                case CSOperatorType.csotDiv:
                    Line($"Operate Div");
                    break;
                case CSOperatorType.csotMod:
                    Line($"Operate Mod");
                    break;
                case CSOperatorType.csotAnd:
                    Line($"Operate And");
                    break;
                case CSOperatorType.csotOr:
                    Line($"Operate Or");
                    break;
                case CSOperatorType.csotXor:
                    Line($"Operate Xor");
                    break;
                case CSOperatorType.csotLogicalAnd:
                    Line($"Operate LAnd");
                    break;
                case CSOperatorType.csoutLogicalOr:
                    Line($"Operate LOr");
                    break;
                default:
                    throw new Exception("Unknow operator.");
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
                    Line($"UnaryOperate Plus");
                    break;
                case CSUnaryOperatorType.csuotNegate:
                    Line($"UnaryOperate Negate");
                    break;
                case CSUnaryOperatorType.csuotBitNot:
                    Line($"UnaryOperate BitNot");
                    break;
                case CSUnaryOperatorType.csuotLogicalNot:
                    Line($"UnaryOperate LogicalNot");
                    break;
                default:
                    throw new Exception("Unknow unary operator.");
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
                    Line($"Compare NotEqual");
                    break;
                case CSCompareType.csctEqual:
                    Line($"Compare Equal");
                    break;
                case CSCompareType.csctLessThan:
                    Line($"Compare LessThan");
                    break;
                case CSCompareType.csctLessEqual:
                    Line($"Compare LessEqual");
                    break;
                case CSCompareType.csctGreaterThan:
                    Line($"Compare GreaterThan");
                    break;
                case CSCompareType.csctGreaterEqual:
                    Line($"Compare GreaterEqual");
                    break;
                default:
                    throw new Exception("Unknown compare operator.");
            }
        }
    }
}
