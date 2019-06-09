using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace DidiWebSocketTest.Models.Messages
{
    public class ScopeConfigMessage : MessageBase
    {
        public ScopeConfigMessage()
        {
            header[2] = 0x01; // config_params message
            MessageBytes = header;
        }
        public ScopeConfigMessage(byte[] frame)
        {
            ScopeConfigDataStructure ds = (ScopeConfigDataStructure)RawDataToObject(ref frame, typeof(ScopeConfigDataStructure), frame.Length);
            MessageBytes = ds.version;

        }
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        internal class ScopeConfigDataStructure
        {
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
            //public String message;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = identifierSize)]
            //public char[] identifier = new char[identifierSize];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] // Header
            public byte[] header = new byte[4];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] // First Channel
            public byte[] version = new byte[8];
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
