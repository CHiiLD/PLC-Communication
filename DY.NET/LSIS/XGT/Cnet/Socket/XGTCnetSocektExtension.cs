﻿using System;
using System.IO.Ports;


namespace DY.NET.LSIS.XGT
{
    /// <summary>
    /// partial 기능을 사용하여 프로퍼티 리턴 함수 선언
    /// </summary>
    public sealed partial class XGTCnetSocket
    {
        public string GetPortName()
        {
            if (_SerialPort == null)
                throw new NullReferenceException(ERROR_SERIAL_IS_NULL);
            return _SerialPort.PortName;
        }

        public int GetBaudRate()
        {
            if (_SerialPort == null)
                throw new NullReferenceException(ERROR_SERIAL_IS_NULL);
            return _SerialPort.BaudRate;
        }

        public Parity GetParity()
        {
            if (_SerialPort == null)
                throw new NullReferenceException(ERROR_SERIAL_IS_NULL);
            return _SerialPort.Parity;
        }

        public int GetDataBits()
        {
            if (_SerialPort == null)
                throw new NullReferenceException(ERROR_SERIAL_IS_NULL);
            return _SerialPort.DataBits;
        }

        public StopBits GetStopBits()
        {
            if (_SerialPort == null)
                throw new NullReferenceException(ERROR_SERIAL_IS_NULL);
            return _SerialPort.StopBits;
        }
    }
}
