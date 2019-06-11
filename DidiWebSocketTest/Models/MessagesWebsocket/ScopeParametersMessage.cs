using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace DidiWebSocketTest.Models.Messages
{
    public class ScopeParametersMessage : WebSocketMessageBase
    {
        public ScopeParametersMessage()
        {
            header[2] = 0x00; // get_scope_params message
            MessageBytes = header;
        }
        public ScopeParametersMessage(byte[] frame)
        {
            ScopeParameterDataStructure ds = (ScopeParameterDataStructure)RawDataToObject(ref frame, typeof(ScopeParameterDataStructure), frame.Length);
            //MessageBytes = frame;
            header = ds.header;
            MessageBytes = ds.parameter;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        internal class ScopeParameterDataStructure
        {
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            //public String message;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = identifierSize)]
            //public char[] identifier = new char[identifierSize];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] // Header
            public byte[] header = new byte[4];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] // First Channel
            public byte[] parameter = new byte[8];
            ////[MarshalAs(UnmanagedType.ByValArray, SizeConst = tcp_portSize)]
            ////public byte[] tcp_port = new byte[tcp_portSize];
            //[MarshalAs(UnmanagedType.U1)]
            //public char driveType;
            //[MarshalAs(UnmanagedType.U1)]
            //public byte unused_1;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = msgTypeSize)]
            //public char[] msgType = new char[msgTypeSize];
            //[MarshalAs(UnmanagedType.U1)]
            //public char msgCode;
            //[MarshalAs(UnmanagedType.U1)]
            //public byte flags;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = unused_2Size)]
            //public byte[] unused_2 = new byte[unused_2Size];        }
        }
    }
}
