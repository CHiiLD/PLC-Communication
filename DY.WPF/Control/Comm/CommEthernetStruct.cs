﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace DY.WPF
{
    public struct EthernetCommStruct
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public ProtocolType Type { get; set; }
    }
}