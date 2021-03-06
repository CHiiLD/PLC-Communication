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

        public int Port { get { return m_Port; } }
        public string Host { get { return m_Host; } }

        /// <summary>
        /// 생성자
        /// </summary>
        public XGTFEnetSocket(string host, XGTFEnetPort port)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException("Host arguement is null or empty.");
            if (port != XGTFEnetPort.TCP && port != XGTFEnetPort.UDP)
                throw new ArgumentException("Port support 2004 or 2005 only.");

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
            bool isConnected = true;
            using (Locker.Lock())
            {
                try
                {
                    m_TcpClient.Client.Send(EMPTY_BYTE, 0, 0);
                    isConnected = true;
                }
                catch (Exception e)
                {
                    isConnected = false;
                }
            }
            return isConnected;
        }

        /// <summary>
        /// TcpClient을 통해 서버와 접속 시도
        /// </summary>
        /// <returns>접속 시도 결과</returns>
        public override bool Connect()
        {
            using (Locker.Lock())
            {
                m_TcpClient = new TcpClient(m_Host, m_Port);
                BaseStream = m_TcpClient.GetStream();
            }
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
            {
                using (Locker.Lock())
                {
                    m_TcpClient.Close(); //Close 후 다시 Connect시 에러발생(이미 TcpClient에서는 Dispose를 호출하였다)
                }
            }
            ConnectionStatusChangedEvent(false);
            LOG.Debug(Description + " 이더넷 통신 동기 접속 해제");
        }

        /// <summary>
        /// 메모리 해제 및 반환
        /// </summary>
        public override void Dispose()
        {
            Close();
            m_TcpClient = null;
            m_Host = null;
            m_Port = 0;
            base.Dispose();
            GC.SuppressFinalize(this);
            LOG.Debug(Description + " 이더넷 통신 해제 및 메모리 해제");
        }

        /// <summary>
        /// 서버로부터 수신할 데이터가 더 있는지 검사한다.
        /// </summary>
        /// <param name="request">요청 프로토콜</param>
        /// <returns>수신할 데이터가 더 있다면 true, 아니면 false</returns>
        protected override bool IsArrived(AProtocol request, int idx)
        {
            ushort target = 0, sum = 0;
            if (idx < XGTFEnetHeader.APPLICATION_HEARDER_FORMAT_SIZE)
                return false;
            target = CV2BR.ToValue(new byte[] { BaseBuffer[16], BaseBuffer[17] });
            for (int i = XGTFEnetHeader.APPLICATION_HEARDER_FORMAT_SIZE; i < idx; i++) //바이트의 개수
                sum++;
            return target == sum;
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
                resp.ASCII = recv_data;
            resp.AnalysisProtocol();
            return resp;
        }

        protected override void DiscardInBuffer()
        {
        }
    }
}