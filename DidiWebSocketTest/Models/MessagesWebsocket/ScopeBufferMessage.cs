using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DidiWebSocketTest.Models.Messages
{
    public class ScopeBufferMessage : WebSocketMessageBase
    {
        public int ChannelCount
        {
            get { return 2; }// ((int)header[0] - 4) / 4; }
        }
        public int SampleCount { get; set; }
        public float[] Data { get; set; }
        public ScopeBufferMessage(byte[] frame) : base(frame)
        {
            byte[] frameData = frame.Skip(frame[0]).ToArray();
            SampleCount = frameData.Length / (4* (ChannelCount + 1));
            if (header[2] == 0x02)
            {
                ScopeFullBufferDataStructure ds = BytesToStruct<ScopeFullBufferDataStructure>(ref frameData, frameData.Length);
                ResizeSortByTimeStampAndSetData(ds);
            }
            if (header[2] == 0x03)
            {
                ScopeHalfBufferDataStructure ds = BytesToStruct<ScopeHalfBufferDataStructure>(ref frameData, frameData.Length);
                ResizeSortByTimeStampAndSetData(ds);
            }
            MessageBytes = frame;
        }

        private void ResizeSortByTimeStampAndSetData(IBufferDataStructure ds)
        {
            int validDataArraySize = (ChannelCount + 1) * (ds.CircularBuffer.Length / (ChannelCount + 1));
            var validDataArray = ds.CircularBuffer.Take(validDataArraySize).ToArray();
            var tmp = validDataArray.Where((x, i) => i % (ChannelCount + 1) == 0).ToArray();
            var minTimeStamp = tmp.Min();
            var index = Array.IndexOf(tmp, minTimeStamp);
            Data = validDataArray.Skip(index * (ChannelCount + 1)).ToArray();
            Data = Data.Concat(validDataArray.Take(index * (ChannelCount + 1))).ToArray();
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        internal class ScopeFullBufferDataStructure : IBufferDataStructure
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8 * 1024)]
            public float[] circularBuffer;
            public float[] CircularBuffer { get { return circularBuffer; } }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        internal class ScopeHalfBufferDataStructure : IBufferDataStructure
        {
            [Endian(Endianness.BigEndian)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4 * 1024)]
            public float[] circularBuffer;
            public float[] CircularBuffer { get { return circularBuffer; } }
        }
        internal interface IBufferDataStructure
        {
            float[] CircularBuffer { get; }
        }
    }
}
