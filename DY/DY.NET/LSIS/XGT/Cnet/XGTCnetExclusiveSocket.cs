﻿/*
 * 작성자: CHILD	
 * 기능: LS산전의 XGT Cnet 전용 프로토콜을 사용하여 통신하는 소켓 구현 (PLC호스트 PC가 게스트일 때)
 * 주의: Cnet통신은 시리얼통신을 기반으로 합니다. 따라서 시리얼통신을 랩핑합시다.
 *       또한 시리얼통신은 동시에 여러 명령어를 보낼 수 없습니다. (1:1 요청 -> 응답 ->요청 ->응답의 루틴) 
 *       쓰레드에 안전하게 설계되어야 합니다.
 * 날짜: 2015-03-25
 */

using System;
using System.Threading.Tasks;
using System.IO.Ports;

namespace DY.NET.LSIS.XGT
{
    /// <summary>
    /// XGTCnetExclusiveProtocol 과 Serial 통신 사용하기 위해 만들어진 소켓 클래스
    /// </summary>
    public class XGTCnetExclusiveSocket : DYSocket
    {
        #region var_properties_event
        private DYSerialPort _DYSerialPort;
        protected DYSerialPort Serial
        {
            get
            {
                return _DYSerialPort;
            }
            set
            {
                _DYSerialPort = value;
            }
        }
        protected bool IsWaitACKProtocol = false;

        public EventHandler<EventArgs> OnSendedSuccessfully;
        public EventHandler<EventArgs> OnReceivedSuccessfully;

        #endregion

        #region method

        /// <summary>
        /// XGTCnetExclusiveSocket 생성자
        /// </summary>
        /// <param name="serialPort"> DY시리얼포트 클래스 </param>
        /// <param name="serialError"> 시리얼포트의 에러 핸들러 </param>
        public XGTCnetExclusiveSocket(DYSerialPort serialPort)
            : base()
        {
            Serial = serialPort;
            if (Serial == null)
                throw new ArgumentNullException("SerialSocket", "paramiter value is null");

            //이벤트 설정
            Serial.DataReceived += new SerialDataReceivedEventHandler(OnDataRecieve);
        }

        /// <summary>
        /// 접속
        /// </summary>
        /// <returns> 통신 접속 성공 여부 </returns>
        public override bool Connect()
        {
            if (Serial == null)
                throw new NullReferenceException("SerialSocket value is null");
            if (!Serial.IsOpen)
                Serial.Open();
            return Serial.IsOpen;
        }

        /// <summary>
        /// 끊기
        /// </summary>
        /// <returns> 통신 끊기 성공 여부 </returns>
        public override bool Close()
        {
            if (Serial == null)
                return false;
            if (Serial.IsOpen)
                Serial.Close();
            return !Serial.IsOpen;
        }

        /// <summary>
        /// 프로토콜 전송
        /// </summary>
        /// <param name="iProtocol"> XGTCnetExclusiveProtocol 프로토콜 클래스 </param>
        public override void Send(IProtocol iProtocol)
        {
            if (Serial == null)
                return;
            if (!Serial.IsOpen)
                throw new Exception("serial port is not opend");
            if (iProtocol == null)
                throw new ArgumentNullException("protocol argument is null");

            XGTCnetExclusiveProtocol exclusiveProtocol = iProtocol as XGTCnetExclusiveProtocol;
            if (exclusiveProtocol == null)
                throw new ArgumentException("protocol not match XGTCnetExclusiveProtocolFrame type");

            XGTCnetExclusiveProtocol cp_p = new XGTCnetExclusiveProtocol(exclusiveProtocol);
            if (cp_p.ProtocolData == null)
                cp_p.AssembleProtocol();
            if (IsWaitACKProtocol)   //만일 ack응답이 오지 않았다면 큐에 저장하고 대기
            {
                ProtocolStandByQueue.Enqueue(cp_p);
                return;
            }

            IsWaitACKProtocol = true;
            Serial.ReqtProtocol = cp_p;

            //전송
            Serial.Write(cp_p.ProtocolData, 0, cp_p.ProtocolData.Length);

            if (OnSendedSuccessfully != null)
                OnSendedSuccessfully(this, EventArgs.Empty);
            cp_p.OnDataRequestedEvent(this, cp_p);
        }

        /// <summary>
        /// 요청 프로토콜에 의한 응답 프로토콜 이벤트 메서드
        /// </summary>
        /// <param name="sender"> DYSerialPort 객체 </param>
        /// <param name="e"> SerialDataReceivedEventArgs 이벤트 argument </param>
        protected void OnDataRecieve(object sender, SerialDataReceivedEventArgs e)
        {
            if (OnReceivedSuccessfully != null)
                OnReceivedSuccessfully(this, EventArgs.Empty);

            DYSerialPort serialPort = sender as DYSerialPort;
            if (serialPort == null)
                return;
            if (!serialPort.IsOpen)
                return;

            byte[] recievedData = System.Text.Encoding.ASCII.GetBytes(serialPort.ReadExisting());
            XGTCnetExclusiveProtocol recv = serialPort.RecvProtocol as XGTCnetExclusiveProtocol;
            XGTCnetExclusiveProtocol reqt = serialPort.ReqtProtocol as XGTCnetExclusiveProtocol;
            if (recv == null)
            {
                recv = XGTCnetExclusiveProtocol.CreateReceiveProtocol(recievedData, reqt);
                serialPort.RecvProtocol = recv;
            }
            else
            {
                byte[] newBytes = new byte[recv.ProtocolData.Length + recievedData.Length];
                Buffer.BlockCopy(recv.ProtocolData, 0, newBytes, 0, recv.ProtocolData.Length);
                Buffer.BlockCopy(recievedData, 0, newBytes, recv.ProtocolData.Length, recievedData.Length);
                recv.ProtocolData = newBytes;
            }
            if (!recv.IsComeInEXTTail())
                return;
            try
            {
                recv.AnalysisProtocol();  //예외 발생
            }
            catch (Exception exception)
            {
                recv.Error = XGTCnetExclusiveProtocolError.EXCEPTION;
#if DEBUG
                Console.WriteLine(exception.Message);
                System.Diagnostics.Debug.Assert(false);
#endif
            }
            finally
            {
                if (recv.Error == XGTCnetExclusiveProtocolError.OK)
                    reqt.OnDataReceivedEvent(this, recv);
                else
                    reqt.OnErrorEvent(this, recv);

                serialPort.ProtocolClear();
                IsWaitACKProtocol = false;
                if (ProtocolStandByQueue.Count != 0)
                    SendNextProtocol(); //Task.Factory.StartNew(new Action(SendNextProtocol));
            }
        }

        protected virtual void SendNextProtocol()
        {
            IProtocol temp = null;
            if (ProtocolStandByQueue.TryDequeue(out temp))
                Send(temp);
        }

        /// <summary>
        /// 메모리 반환
        /// </summary>
        public override void Dispose()
        {
            if (Serial != null)
            {
                Close();
                Serial.ProtocolClear();
                Serial.Dispose();

                OnSendedSuccessfully = null;
                OnReceivedSuccessfully = null;
            }
            base.Dispose();
        }
        #endregion
    }
}