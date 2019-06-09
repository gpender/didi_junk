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
    public class ScopeBufferMessage : MessageBase
    {
        public int ChannelCount
        {
            get { return ((int)header[0] - 4) / 4; }
        }
        public float[] Data { get; set; }
        public ScopeBufferMessage(byte[] frame) : base(frame)
        {
            byte[] data = frame.Skip(frame[0]).ToArray();

            if (header[2] == 0x02)
            {
                Array.Resize(ref data, 1024 * 32);
                ScopeFullBufferDataStructure ds = BytesToStruct<ScopeFullBufferDataStructure>(ref data, data.Length);
                Data = ds.data;
            }
            if (header[2] == 0x03)
            {
                Array.Resize(ref data, 1024 * 16);
                ScopeHalfBufferDataStructure ds = (ScopeHalfBufferDataStructure)RawDataToObject(ref data, typeof(ScopeHalfBufferDataStructure), data.Length);//BytesToStruct<ScopeHalfBufferDataStructure>(data, data.Length);
                Data = ds.data;
            }
            MessageBytes = frame;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        internal class ScopeFullBufferDataStructure
        {
            [Endian(Endianness.BigEndian)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 * 1024)]
            public float[] data;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        internal struct ScopeHalfBufferDataStructure
        {
            [Endian(Endianness.BigEndian)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16 * 1024)]
            public float[] data;
        }
    }
}
