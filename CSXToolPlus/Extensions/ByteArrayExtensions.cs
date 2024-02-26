using System;
using System.Buffers.Binary;

namespace CSXToolPlus.Extensions
{
    public static class ByteArrayExtensions
    {
        public static int ReadInt16(this byte[] array)
        {
            return BinaryPrimitives.ReadInt16LittleEndian(array);
        }

        public static uint ReadUInt16(this byte[] array)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(array);
        }

        public static int ReadInt32(this byte[] array)
        {
            return BinaryPrimitives.ReadInt32LittleEndian(array);
        }

        public static uint ReadUInt32(this byte[] array)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(array);
        }

        public static long ReadInt64(this byte[] array)
        {
            return BinaryPrimitives.ReadInt64LittleEndian(array);
        }

        public static ulong ReadUInt64(this byte[] array)
        {
            return BinaryPrimitives.ReadUInt64LittleEndian(array);
        }

        public static int ReadInt16(this byte[] array, int offset)
        {
            return BinaryPrimitives.ReadInt16LittleEndian(array.AsSpan(offset));
        }

        public static uint ReadUInt16(this byte[] array, int offset)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(array.AsSpan(offset));
        }

        public static int ReadInt32(this byte[] array, int offset)
        {
            return BinaryPrimitives.ReadInt32LittleEndian(array.AsSpan(offset));
        }

        public static uint ReadUInt32(this byte[] array, int offset)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(array.AsSpan(offset));
        }

        public static long ReadInt64(this byte[] array, int offset)
        {
            return BinaryPrimitives.ReadInt64LittleEndian(array.AsSpan(offset));
        }

        public static ulong ReadUInt64(this byte[] array, int offset)
        {
            return BinaryPrimitives.ReadUInt64LittleEndian(array.AsSpan(offset));
        }
    }
}
