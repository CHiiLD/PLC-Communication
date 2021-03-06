﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DY.NET.LSIS.XGT
{
    /// <summary>
    /// XGTFEnetProtocol를 LSIS-XGT PLC에 보낼 FEnet Protocol-ASCII구조로 압축하고
    /// LSIS-XGT PLC에서 받은 FEnet Protocol-ASCII 데이터를 XGTFEnetProtocol로 해석한다.
    /// </summary>
    public class XGTFEnetCompressor : IProtocolCompressor
    {
        public const int HEADER_SIZE = 20;
        public const int READ_RESPONSE_DATA_IDX = 30;

        public const int HEADER_RESERVE_IDX1 = 8;
        public const int HEADER_RESERVE_IDX2 = 9;
        public const int HEADER_PLC_INFO_IDX = 10;
        public const int HEADER_SYS_STATE_IDX = 11;
        public const int HEADER_CPU_INFO_IDX = 12;
        public const int HEADER_DIRECTION_IDX = 13;
        public const int HEADER_INVOKE_IDX1 = 14;
        public const int HEADER_INVOKE_IDX2 = 15;
        public const int HEADER_LENGTH_IDX1 = 16;
        public const int HEADER_LENGTH_IDX2 = 17;
        public const int HEADER_POSITION_IDX = 18;
        public const int HEADER_BCC_IDX = 19;

        public const int BODY_COMMAND_IDX1 = 20;
        public const int BODY_COMMAND_IDX2 = 21;
        public const int BODY_DATATYPE_IDX1 = 22;
        public const int BODY_DATATYPE_IDX2 = 23;
        public const int BODY_RESERVE_IDX1 = 24;
        public const int BODY_RESERVE_IDX2 = 25;
        public const int BODY_ERROR_IDX1 = 26;
        public const int BODY_ERROR_IDX2 = 27;
        public const int BODY_BLOCK_IDX1 = 28;
        public const int BODY_BLOCK_IDX2 = 29;

        private const byte ERROR_VALUE = 0xFF;

        /// <summary>
        /// XGTFEnetProtocol를 LSIS-XGT PLC에 보낼 FEnet Protocol-ASCII구조로 압축하여 반환한다.
        /// </summary>
        /// <param name="protocol">XGTCnetProtocol</param>
        /// <returns>FEnet Protocol-ASCII</returns>
        public virtual byte[] Encode(IProtocol protocol)
        {
            const int ITEM_MAX_COUNT = 16;
            const int ADDRESS_STRING_MAX_LENGTH = 16;

            XGTFEnetProtocol fenet = protocol as XGTFEnetProtocol;
            if (!(fenet.Command == XGTFEnetCommand.READ_REQT || fenet.Command == XGTFEnetCommand.WRITE_REQT))
                throw new ArgumentException("Response command not supported.");
            List<byte> buf = new List<byte>();
            fenet.CompanyID = XGTFEnetCompanyID.LSIS_XGT;
            fenet.StreamDirection = XGTFEnetStreamDirection.PC2PLC;

            buf.AddRange(fenet.Command.ToBytes().Reverse());
            buf.AddRange(fenet.DataType.ToBytes().Reverse());
            buf.AddRange(new byte[] { 0x00, 0x00 }); //reserved
            buf.AddRange(XGTFEnetTranslator.ToASCII<ushort>(fenet.Data.Count)); //블록 수

            if (fenet.Data.Count > ITEM_MAX_COUNT)
                throw new Exception("블록 수 초과 에러");

            if (fenet.DataType == XGTFEnetDataType.BIT)
                ConvertByteMemAddress(fenet.Data);

            foreach (var item in fenet.Data)
            {
                if (item.Address.Length > ADDRESS_STRING_MAX_LENGTH)
                    throw new Exception("주소 문자열 길이 초과 에러");

                buf.AddRange(XGTFEnetTranslator.ToASCII<ushort>(item.Address.Length)); //addr str length
                buf.AddRange(XGTFEnetTranslator.ToASCII<string>(item.Address)); //addr str
                if (fenet.Command == XGTFEnetCommand.WRITE_REQT)
                {
                    buf.AddRange(fenet.DataType.ToBytes().Reverse()); //data type
                    byte[] value = XGTFEnetTranslator.ToCode(protocol.Type, item.Value);
                    buf.AddRange(value); //value
                }
            }

            byte[] header_buf = new byte[HEADER_SIZE];
            byte[] company_name = fenet.CompanyID.ToBytes();
            byte direction = fenet.StreamDirection.ToByte();
            byte[] invoke = XGTFEnetTranslator.ToASCII<ushort>(fenet.InvokeID);
            byte[] body_len = XGTFEnetTranslator.ToASCII<ushort>(buf.Count);
            Buffer.BlockCopy(company_name, 0, header_buf, 0, company_name.Length);
            Buffer.SetByte(header_buf, HEADER_DIRECTION_IDX, direction);
            Buffer.BlockCopy(invoke, 0, header_buf, HEADER_INVOKE_IDX1, invoke.Length);
            Buffer.BlockCopy(body_len, 0, header_buf, HEADER_LENGTH_IDX1, body_len.Length);
            Buffer.SetByte(header_buf, HEADER_BCC_IDX, (byte)header_buf.Sum(x => x));

            buf.InsertRange(0, header_buf);
            return buf.ToArray();
        }

        /// <summary>
        /// LSIS-XGT PLC에서 받은 FEnet Protocol-ASCII 데이터를 XGTCnetProtocol로 해석하여 반환한다.
        /// </summary>
        /// <param name="ascii">FEnet Protocol-ASCII</param>
        /// <returns>XGTCnetProtocol</returns>
        public virtual IProtocol Decode(byte[] ascii, IProtocol request)
        {
            Type type = request.Type;
            XGTFEnetCommand command = (XGTFEnetCommand)XGTFEnetTranslator.ToValue<ushort>(ascii[BODY_COMMAND_IDX1], ascii[BODY_COMMAND_IDX2]);
            if (!(command == XGTFEnetCommand.READ_RESP || command == XGTFEnetCommand.WRITE_RESP))
                throw new ArgumentException("Request command not supported.");
            XGTFEnetDataType datatype = (XGTFEnetDataType)XGTFEnetTranslator.ToValue<ushort>(ascii[BODY_DATATYPE_IDX1], ascii[BODY_DATATYPE_IDX2]);
            XGTFEnetProtocol fenet = new XGTFEnetProtocol();
            fenet.Command = command;
            fenet.DataType = datatype;
            fenet.Type = type;
            fenet.CompanyID = XGTFEnetCompanyID.LSIS_XGT.ToBytes().SequenceEqual(
                new byte[] { ascii[0], ascii[1], ascii[2], ascii[3], ascii[4], ascii[5], ascii[6], ascii[7] }) ?
                XGTFEnetCompanyID.LSIS_XGT : XGTFEnetCompanyID.NONE;
            fenet.CpuType = (XGTFEnetCpuType)(0xFC & ascii[HEADER_PLC_INFO_IDX]);
            fenet.Class = (XGTFEnetClass)(0x02 & ascii[HEADER_PLC_INFO_IDX]);
            fenet.CpuState = (XGTFEnetCpuState)(0x01 & ascii[HEADER_PLC_INFO_IDX]);
            fenet.PLCState = (XGTFEnetPLCSystemState)(0x1F & ascii[HEADER_SYS_STATE_IDX]);
            fenet.CpuInfo = (XGTFEnetCpuInfo)ascii[HEADER_CPU_INFO_IDX];
            fenet.StreamDirection = (XGTFEnetStreamDirection)ascii[HEADER_DIRECTION_IDX];
            fenet.InvokeID = XGTFEnetTranslator.ToValue<ushort>(ascii[HEADER_INVOKE_IDX1], ascii[HEADER_INVOKE_IDX2]);
            fenet.BodyLength = GetApplicationDataLength(ascii);
            fenet.BasePosition = (byte)(ascii[HEADER_POSITION_IDX] >> 4);
            fenet.SlotPosition = (byte)((byte)0x0F & ascii[HEADER_POSITION_IDX]);

            int temp_bcc = 0;
            for (int i = 0; i < HEADER_SIZE; i++)
                temp_bcc += ascii[i];
            fenet.BCC = (byte)temp_bcc;
            ushort er_bl = XGTFEnetTranslator.ToValue<ushort>(ascii[BODY_BLOCK_IDX1], ascii[BODY_BLOCK_IDX2]);
            if (ascii[BODY_ERROR_IDX1] == ERROR_VALUE || ascii[BODY_ERROR_IDX2] == ERROR_VALUE)
            {
                fenet.Error = (XGTFEnetError)er_bl;
                return fenet;
            }

            if (fenet.Command == XGTFEnetCommand.READ_RESP)
            {
                IList<IProtocolData> reqtData = request.Data;
                ushort count = er_bl;
                if (fenet.Data == null) 
                    fenet.Data = new List<IProtocolData>(); 
                else 
                    fenet.Data.Clear();
                if (reqtData.Count != count)
                    throw new Exception("읽기 프로토콜의 요청 데이터 개수와 응답 데이터의 개수가 불일치");
                int idx = READ_RESPONSE_DATA_IDX;
                for (int i = 0; i < count; i++)
                {
                    ushort size = XGTFEnetTranslator.ToValue<ushort>(ascii[idx], ascii[idx + 1]);
                    idx += 2;
                    byte[] code = new byte[size];
                    Buffer.BlockCopy(ascii, idx, code, 0, size);
                    fenet.Data.Add(new ProtocolData(reqtData[i].Address, XGTFEnetTranslator.ToValue(type, code)));
                    idx += size;
                }
            }
            return fenet;
        }

        public ushort GetApplicationDataLength(byte[] ascii)
        {
            return XGTFEnetTranslator.ToValue<ushort>(ascii[HEADER_LENGTH_IDX1], ascii[HEADER_LENGTH_IDX2]);
        }

        private void ConvertByteMemAddress(IList<IProtocolData> items)
        {
            foreach (var item in items)
            {
                string dec_str = item.Address.Substring(3, item.Address.Length - 4);
                string hex_str = item.Address.Last().ToString();
                int dec_int = Convert.ToInt32(dec_str);
                int hex_int = Convert.ToInt32(hex_str, 16);
                int mem_int = (dec_int * 16) + hex_int;
                string addr = item.Address.Substring(0, 3) + mem_int.ToString();
                item.Address = addr;
            }
        }
    }
}