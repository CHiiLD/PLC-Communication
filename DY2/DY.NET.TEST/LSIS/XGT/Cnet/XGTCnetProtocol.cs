﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DY.NET.LSIS.XGT
{
    public class XGTCnetProtocol : IProtocol
    {
        public XGTCnetHeader Header { set; get; }
        public ushort LocalPort { set; get; }
        public XGTCnetCommand Command { set; get; }
        public XGTCnetCommandType CommandType { set; get; }
        public XGTCnetHeader Tail { set; get; }
        public XGTCnetProtocolError Error { get; set; }

        public IList<IProtocolItem> Items { get; set; }

        public XGTCnetProtocol()
        {
            Initialize();
        }

        public XGTCnetProtocol(ushort localPort, XGTCnetCommand cmd, XGTCnetCommandType type)
            : this()
        {
            LocalPort = localPort;
            Command = cmd;
            CommandType = type;
        }

        public virtual int GetErrorCode()
        {
            //Error 코드를 int로 변환해서 반환
            return (int)Error;
        }

        public virtual void Initialize()
        {
            Header = XGTCnetHeader.NONE;
            LocalPort = ushort.MaxValue;
            Command = XGTCnetCommand.NONE;
            CommandType = XGTCnetCommandType.NONE;
            Tail = XGTCnetHeader.NONE;
            Error = XGTCnetProtocolError.OK;
            Items = null;
        }
    }
}