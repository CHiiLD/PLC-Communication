﻿namespace DY.NET.LSIS.XGT
{
    /// <summary>
    /// XGT FEnet - 헤더 구조(Application Header Format) - PLC Info - Bit (08-12) 
    /// 시스템 상태
    /// </summary>
    public enum XGTFEnetPLCSystemState
    {
        NONE = 0x00,
        RUN = 0x01,     //작동 
        STOP = 0x02,    //정지 
        PAUSE = 0x04,   //일시정지 
        DEBUG = 0x08    //디버그   
    }

    public static class XGTFEnetPLCSystemStateExtension
    {
        public static byte ToByte(this XGTFEnetPLCSystemState state)
        {
            return (byte)state;
        }
    }
}