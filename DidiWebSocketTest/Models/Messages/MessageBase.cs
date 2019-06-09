using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace DidiWebSocketTest.Models.Messages
{
    // Header bytes
    // 0 - Data Offset (header size: normally = 4)
    // 1 - Version (=1)
    // 2 - Message Type
    // 3 - Reserved

    // Response Message Types
    // 0 - GET SCOPE PARAMS
    // 1 - GET CONFIG PARAMS
    // 2 - SEND FULL BUFFER
    // 3 - SEND FULL BUFFER
    public abstract class MessageBase
    {
        // Header bytes
        // 0 - Data Offset (header size: normally = 4)
        // 1 - Version (=1)
        // 2 - Message Type
        // 3 - Reserved
        const int headerLength = 0x04;
        const int version = 0x01;
        protected byte[] header = new byte[4] { headerLength, version, 0x00, 0x00 };
        public string Message { get; protected set; }
        public byte[] MessageBytes { get; protected set; }
        public MessageType MessageType
        {
            get { return (MessageType)header[2]; }
        }
        public MessageBase()
        {
        }
        public MessageBase(byte[] frame)
        {
            header = frame.Take(headerLength).ToArray();
        }
        protected IEnumerable<byte> GetBytesFromString(string stringProperty, int stringPropertyRequestedSize)
        {
            List<byte> bytes = Encoding.ASCII.GetBytes(stringProperty).ToList();
            int actualSize = bytes.Count;
            for (int i = actualSize; i < stringPropertyRequestedSize; i++)
            {
                bytes.Add(0x00);
            }
            return bytes;
        }
        protected byte[] GetBigEndianBytes(ushort val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return bytes;
        }
        protected byte BitArrayToByte(BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }

        protected BitArray ByteToBitArray(byte b)
        {
            BitArray ba = new BitArray(new byte[] { b });
            return ba;
        }

        protected string ByteArrayToIpAddress(byte[] bytes)
        {
            IPAddress ip = new IPAddress(bytes);
            return ip.ToString();
        }
        protected T BytesToStruct<T>(ref byte[] rawData, int dataTotalSize) where T : class
        {
            if (rawData.Length != dataTotalSize)
            {
                throw new Exception("Incorrect Data length in Message");
            }
            T result = default(T);
            RespectEndianness(typeof(T), rawData);
            GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            IntPtr rawDataPtr = IntPtr.Zero;
            try
            {
                // Get the address of the data array
                rawDataPtr = handle.AddrOfPinnedObject();
                // overlay the data type on top of the raw data
                result = (T)Marshal.PtrToStructure(rawDataPtr, typeof(T));
            }
            finally
            {
                // must explicitly release
                handle.Free();
            }
            return result;
        }
        private static void RespectEndianness(Type type, byte[] data)
        {
            var fields = type.GetFields().Where(f => f.IsDefined(typeof(EndianAttribute), false))
                .Select(f => new
                {
                    Field = f,
                    Attribute = (EndianAttribute)f.GetCustomAttributes(typeof(EndianAttribute), false)[0],
                    Offset = Marshal.OffsetOf(type, f.Name).ToInt32()
                }).ToList();

            foreach (var field in fields)
            {
                if ((field.Attribute.Endianness == Endianness.BigEndian && BitConverter.IsLittleEndian) ||
                    (field.Attribute.Endianness == Endianness.LittleEndian && !BitConverter.IsLittleEndian))
                {
                    if (field.Field.FieldType.IsArray)
                    {
                        int size = Marshal.SizeOf(field.Field.FieldType.GetElementType());
                        Array.Reverse(data, field.Offset, size);
                    }
                    else
                    {
                        Array.Reverse(data, field.Offset, Marshal.SizeOf(field.Field.FieldType));
                    }
                }
            }
        }

        //No longer used
        protected object RawDataToObject(ref byte[] rawData, Type overlayType, int dataTotalSize)
        {
            if (rawData.Length != dataTotalSize)
            {
                throw new Exception("Incorrect Data length in Message");
            }
            object result = null;

            GCHandle pinnedRawData = GCHandle.Alloc(rawData, GCHandleType.Pinned);

            try
            {
                // Get the address of the data array
                IntPtr pinnedRawDataPtr = pinnedRawData.AddrOfPinnedObject();
                Debug.WriteLine($"rawdata address {pinnedRawDataPtr.ToString("x")}");

                // overlay the data type on top of the raw data
                result = Marshal.PtrToStructure(pinnedRawDataPtr, overlayType); //0x0317cecc
            }
            finally
            {
                // must explicitly release
                pinnedRawData.Free();
            }
            return result;
        }
    }
    public enum Endianness
    {
        BigEndian,
        LittleEndian
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class EndianAttribute : Attribute
    {
        public Endianness Endianness { get; private set; }

        public EndianAttribute(Endianness endianness)
        {
            this.Endianness = endianness;
        }
    }
}
