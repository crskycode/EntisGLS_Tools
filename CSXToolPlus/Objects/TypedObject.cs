using CSXToolPlus.Types;
using CSXToolPlus.Utils;
using System;

namespace CSXToolPlus.Objects
{
    public class TypedObject
    {
        public CSVariableType Type { get; set; }
        public object Data { get; set; }

        public TypedObject()
        {
            Data = new object();
        }

        public void Read(SimpleBinaryReader reader)
        {
            Type = (CSVariableType)reader.ReadInt32();

            switch (Type)
            {
                case CSVariableType.Invalid:
                {
                    // No data
                    break;
                }
                case CSVariableType.csvtObject:
                {
                    var obj = new ClassInfoObject();
                    obj.Read(reader);
                    Data = obj;
                    break;
                }
                case CSVariableType.csvtReference:
                {
                    if (reader.Info.Version == 1)
                    {
                        var obj = new TypedObject();
                        obj.Read(reader);
                        Data = obj;
                    }
                    break;
                }
                case CSVariableType.csvtArray:
                {
                    var obj = new ArrayObject();
                    obj.Read(reader);
                    Data = obj;
                    break;
                }
                case CSVariableType.csvtHash:
                {
                    // No data
                    break;
                }
                case CSVariableType.csvtInteger:
                {
                    if (reader.Info.IntBase == 64)
                        Data = reader.ReadInt64();
                    else
                        Data = reader.ReadInt32();
                    break;
                }
                case CSVariableType.csvtReal:
                {
                    Data = reader.ReadDouble();
                    break;
                }
                case CSVariableType.csvtString:
                {
                    Data = reader.ReadWideString();
                    break;
                }
                case CSVariableType.csvtInteger64:
                {
                    // NOTE: Read 1 Int64 in 《恋色マリアージュ》
                    // NOTE: Read 2 Int64 in 《お兄ちゃん右手の使用を禁止します》
                    if (reader.Info.FullVer == 3)
                    {
                        var obj = new Integer64Object();
                        obj.Read(reader);
                        Data = obj;
                    }
                    else
                    {
                        Data = reader.ReadInt64();
                    }
                    break;
                }
                case CSVariableType.csvtPointer:
                {
                    // NOTE: No data in 《恋色マリアージュ》
                    if (reader.Info.FullVer == 3)
                    {
                        var obj = new PointerObject();
                        obj.Read(reader);
                        Data = obj;
                    }
                    break;
                }
                case CSVariableType.csvtClassObject:
                {
                    throw new NotImplementedException();
                }
                case CSVariableType.csvtBoolean:
                {
                    Data = reader.ReadInt64();
                    break;
                }
                case CSVariableType.csvtInt8:
                {
                    Data = reader.ReadInt64();
                    break;
                }
                case CSVariableType.csvtUint8:
                {
                    Data = reader.ReadInt64();
                    break;
                }
                case CSVariableType.csvtInt16:
                {
                    Data = reader.ReadInt64();
                    break;
                }
                case CSVariableType.csvtUint16:
                {
                    Data = reader.ReadInt64();
                    break;
                }
                case CSVariableType.csvtInt32:
                {
                    Data = reader.ReadInt64();
                    break;
                }
                case CSVariableType.csvtUint32:
                {
                    Data = reader.ReadInt64();
                    break;
                }
                case CSVariableType.csvtArrayDimension:
                {
                    var obj = new ArrayDimensionObject();
                    obj.Read(reader);
                    Data = obj;
                    break;
                }
                case CSVariableType.csvtHashContainer:
                {
                    var obj = new HashContainerObject();
                    obj.Read(reader);
                    Data = obj;
                    break;
                }
                case CSVariableType.csvtReal32:
                {
                    Data = reader.ReadDouble();
                    break;
                }
                case CSVariableType.csvtReal64:
                {
                    Data = reader.ReadDouble();
                    break;
                }
                case CSVariableType.csvtPointerReference:
                {
                    throw new NotImplementedException();
                }
                case CSVariableType.csvtBuffer:
                {
                    throw new NotImplementedException();
                }
                case CSVariableType.csvtFunction:
                {
                    throw new NotImplementedException();
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void Write(SimpleBinaryWriter writer)
        {
            writer.WriteInt32((int)Type);

            switch (Type)
            {
                case CSVariableType.Invalid:
                {
                    // No data
                    break;
                }
                case CSVariableType.csvtObject:
                {
                    var obj = (ClassInfoObject)Data;
                    obj.Write(writer);
                    break;
                }
                case CSVariableType.csvtReference:
                {
                    if (writer.Info.Version == 1)
                    {
                        var obj = (TypedObject)Data;
                        obj.Write(writer);
                    }
                    break;
                }
                case CSVariableType.csvtArray:
                {
                    var obj = (ArrayObject)Data;
                    obj.Write(writer);
                    break;
                }
                case CSVariableType.csvtHash:
                {
                    // No data
                    break;
                }
                case CSVariableType.csvtInteger:
                {
                    if (writer.Info.IntBase == 64)
                        writer.WriteInt64((long)Data);
                    else
                        writer.WriteInt32((int)(long)Data);
                    break;
                }
                case CSVariableType.csvtReal:
                {
                    writer.WriteDouble((double)Data);
                    break;
                }
                case CSVariableType.csvtString:
                {
                    writer.WriteWideString((string)Data);
                    break;
                }
                case CSVariableType.csvtInteger64:
                {
                    if (writer.Info.FullVer == 3)
                    {
                        var obj = (Integer64Object)Data;
                        obj.Write(writer);
                    }
                    else
                    {
                        writer.WriteInt64((long)Data);
                    }
                    break;
                }
                case CSVariableType.csvtPointer:
                {
                    if (writer.Info.FullVer == 3)
                    {
                        var obj = (PointerObject)Data;
                        obj.Write(writer);
                    }
                    break;
                }
                case CSVariableType.csvtClassObject:
                {
                    throw new NotImplementedException();
                }
                case CSVariableType.csvtBoolean:
                {
                    writer.WriteInt64((long)Data);
                    break;
                }
                case CSVariableType.csvtInt8:
                {
                    writer.WriteInt64((long)Data);
                    break;
                }
                case CSVariableType.csvtUint8:
                {
                    writer.WriteInt64((long)Data);
                    break;
                }
                case CSVariableType.csvtInt16:
                {
                    writer.WriteInt64((long)Data);
                    break;
                }
                case CSVariableType.csvtUint16:
                {
                    writer.WriteInt64((long)Data);
                    break;
                }
                case CSVariableType.csvtInt32:
                {
                    writer.WriteInt64((long)Data);
                    break;
                }
                case CSVariableType.csvtUint32:
                {
                    writer.WriteInt64((long)Data);
                    break;
                }
                case CSVariableType.csvtArrayDimension:
                {
                    var obj = (ArrayDimensionObject)Data;
                    obj.Write(writer);
                    break;
                }
                case CSVariableType.csvtHashContainer:
                {
                    var obj = (HashContainerObject)Data;
                    obj.Write(writer);
                    break;
                }
                case CSVariableType.csvtReal32:
                {
                    writer.WriteDouble((double)Data);
                    break;
                }
                case CSVariableType.csvtReal64:
                {
                    writer.WriteDouble((double)Data);
                    break;
                }
                case CSVariableType.csvtPointerReference:
                {
                    throw new NotImplementedException();
                }
                case CSVariableType.csvtBuffer:
                {
                    throw new NotImplementedException();
                }
                case CSVariableType.csvtFunction:
                {
                    throw new NotImplementedException();
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
