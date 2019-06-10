﻿using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace DidiWebSocketTest.Models.Messages
{
    public class ScopeBufferMessage : MessageBase
    {
        public int ChannelCount
        {
            get { return 2; }// ((int)header[0] - 4) / 4; }
        }
        public int SampleCount { get; set; }
        public float[] Data { get; set; }
        public ScopeBufferMessage(byte[] frame) : base(frame)
        {
            byte[] data = frame.Skip(frame[0]).ToArray();
            SampleCount = data.Length / (4* (ChannelCount + 1));
            if (header[2] == 0x02)
            {
                ScopeFullBufferDataStructure ds = BytesToStruct<ScopeFullBufferDataStructure>(ref data, data.Length);
                Data = ds.data;
            }
            if (header[2] == 0x03)
            {
                ScopeHalfBufferDataStructure ds = BytesToStruct<ScopeHalfBufferDataStructure>(ref data, data.Length);
                Data = ds.data;
            }
            MessageBytes = frame;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        internal class ScopeFullBufferDataStructure
        {
            [Endian(Endianness.BigEndian)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8 * 1024)]
            public float[] data;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        internal class ScopeHalfBufferDataStructure
        {
            [Endian(Endianness.BigEndian)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 1024)]
            public float[] data;
        }
    }
}