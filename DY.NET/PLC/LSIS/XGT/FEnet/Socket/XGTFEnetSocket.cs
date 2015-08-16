﻿using System;
using System.Net.Sockets;
using NLog;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DY.NET.LSIS.XGT
{
    /// <summary>
    /// XGT FEnet 이더넷 클라이언트 소켓 클래스
    /// </summary>
    public sealed partial class XGTFEnetSocket : ASocketCover
    {
        private static Logger LOG = LogManager.GetCurrentClassLogger();

        private const string ERROR_TCPCLIENT_IS_NULL = "TcpClient instance is null.";

        private string m_Host; //IP
        private int m_Port;    //포트
        private TcpClient m_TcpClient;

        /// <summary>
        /// 생성자
        /// </summary>
        public XGTFEnetSocket(string host, XGTFEnetPort port)
        {
            m_Host = host;
            m_Port = (int)port;
            Description = "LSIS XGT FEnet(" + m_Host + ":" + m_Port + ")";
        }

        /// <summary>
        /// 소멸자
        /// </summary>
        ~XGTFEnetSocket()
        {
            Dispose();
        }

        /// <summary>
        /// 통신 가능 여부를 파악한다.
        /// </summary>
        /// <returns>통신 가능 여부</returns>
        public override bool IsConnected()
        {
            if (m_TcpClient == null)
                return false;
            if (m_TcpClient.Client == null)
                return false;
            try
            {
                m_TcpClient.Client.Send(EMPTY_BYTE, 0, 0);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// TcpClient을 통해 서버와 접속 시도
        /// </summary>
        /// <returns>접속 시도 결과</returns>
        public override bool Connect()
        {
            m_TcpClient = new TcpClient(m_Host, m_Port);
            BaseStream = m_TcpClient.GetStream();
            ConnectionStatusChangedEvent(true);
            LOG.Debug(Description + " 이더넷 통신 동기 접속");
            return IsConnected();
        }

        /// <summary>
        /// TcpClient 연결 해제
        /// </summary>
        public override void Close()
        {
            if (m_TcpClient != null)
                m_TcpClient.Close();
            ConnectionStatusChangedEvent(false);
            LOG.Debug(Description + " 이더넷 통신 동기 접속 해제");
        }

        /// <summary>
        /// 메모리 해제 및 반환
        /// </summary>
        public override void Dispose()
        {
            if (m_TcpClient != null)
            {
                if (m_TcpClient != null)
                    m_TcpClient.Close();
                m_TcpClient = null;
                m_Host = null;
                m_Port = -1;
            }
            base.Dispose();
            GC.SuppressFinalize(this);
            LOG.Debug(Description + " 이더넷 통신 해제 및 메모리 해제");
        }

        /// <summary>
        /// 서버로부터 수신할 데이터가 더 있는지 검사한다.
        /// </summary>
        /// <param name="request">요청 프로토콜</param>
        /// <returns>수신할 데이터가 더 있다면 true, 아니면 false</returns>
        protected override bool DoReadAgain(AProtocol request)
        {
            ushort target = 0, sum = 0;
            if (StreamBufferIndex < XGTFEnetHeader.APPLICATION_HEARDER_FORMAT_SIZE)
                return false;
            target = CV2BR.ToValue(new byte[] { StreamBuffer[16], StreamBuffer[17] });
            for (int i = XGTFEnetHeader.APPLICATION_HEARDER_FORMAT_SIZE; i < StreamBufferIndex; i++) //바이트의 개수
                sum++;
            return target != sum;
        }

        /// <summary>
        /// XGTFEnetProtocol 응답 프로토콜 생성
        /// </summary>
        /// <param name="request">요청 프로토콜</param>
        /// <param name="recv_data">ASCII 데이터</param>
        /// <returns></returns>
        protected override AProtocol CreateResponseProtocol(AProtocol request, byte[] recv_data)
        {
            XGTFEnetProtocol resp = request.MirrorProtocol as XGTFEnetProtocol; //응답 객체 생성 전에 재활용이 가능한지 검토
            if (request.MirrorProtocol == null)
                request.MirrorProtocol = resp = XGTFEnetProtocol.CreateResponseProtocol(recv_data, request as XGTFEnetProtocol);
            else
                resp.ASCIIProtocol = recv_data;
            resp.AnalysisProtocol();
            return resp;
        }

        /// <summary>
        /// 서버와 통신하여 통신 속도를 측정
        /// </summary>
        /// <returns> 
        /// 0 >=: Milliseconds
        /// 0 <: DeliveryError
        /// </returns>
        public override async Task<long> PingAsync()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("%DW0", null);
            XGTFEnetProtocol cnet = XGTFEnetProtocol.NewRSSProtocol(typeof(ushort), 00, dictionary);
            Delivery delivery = await PostAsync(cnet);
            return delivery.Error == DeliveryError.SUCCESS ? delivery.DelivaryTime.ElapsedMilliseconds : -1;
        }
    }
}