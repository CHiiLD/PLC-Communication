﻿/*
작성자: CHILD	
기능: 통신에 필요한 최소한의 기능들을 인터페이스로 구현
날짜: 2015-03-25
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace DY.NET
{
    /// <summary>
    /// 추상 소켓 클래스
    /// </summary>
    public abstract class DYSocket : ITag, IDisposable
    {
        public DYSocket()
        {

        }

        public int Tag
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public object UserData
        {
            get;
            set;
        }

        /// <summary>
        /// 스레드 세이프 프로토콜 전송 대기 큐
        /// </summary>
        protected ConcurrentQueue<IProtocol> ProtocolStandByQueue = new ConcurrentQueue<IProtocol>();

        public abstract bool Connect();
        public abstract bool Close();
        public abstract void Send(IProtocol protocolFrame);

        public virtual void Dispose()
        {
            IProtocol temp;
            foreach (var p in ProtocolStandByQueue) //clear
                ProtocolStandByQueue.TryDequeue(out temp);
        }
    }
}