﻿using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DY.NET;
using DY.NET.LSIS.XGT;
using NLog;

using DY.WPF.SYSTEM.COMM;

namespace DY.WPF.SYSTEM.IO
{
    public class XGTCommIOMonitoring : ACommIOMonitoringStrategy
    {
        public const int InvokeID = 00;

        public XGTCommIOMonitoring(CommClient cclient)
            : base(cclient)
        {
        }

        /// <summary>
        /// ObservableCollection<CommIODataGridItem> 정보로 프로토콜들을 생성한다
        /// </summary>
        /// <returns></returns>
        public override void UpdateProtocols(IList<ICommIOData> io_datas)
        {
            CommIODatas = io_datas;
            Dictionary<string, DataType> addrs = XGTProtocolHelper.Optimize(io_datas);
            ILookup<DataType, string> lookCollection = addrs.ToLookup(ad => ad.Value, ad => ad.Key);
            int cnt = 0;
            Protocols.Clear();
            Dictionary<string, object> datas = new Dictionary<string, object>();
            foreach (IGrouping<DataType, string> group in lookCollection)
            {
                foreach (string str in group)
                {
                    if (cnt % 16 == 0 && cnt != 0)
                    {
                        Protocols.Add(CreateProtocol(group.Key, datas));
                        cnt = 0;
                        datas = new Dictionary<string, object>();
                    }
                    datas.Add(str, null);
                    cnt++;
                }
                Protocols.Add(CreateProtocol(group.Key, datas));
                cnt = 0;
                datas = new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// XGT Protocol을 생성한다
        /// </summary>
        /// <param name="type">데이터 타입</param>
        /// <param name="datas">READ 목록</param>
        /// <returns></returns>
        private IProtocol CreateProtocol(DataType type, Dictionary<string, object> datas)
        {
            IProtocol protocol;
            switch (CClient.CommType)
            {
                case DYDeviceCommType.SERIAL:
                    ushort localPort = (ushort)(CClient.ExtraData[CommClient.EXTRA_XGT_CNET_LOCALPORT]);
                    protocol = XGTCnetProtocol.NewRSSProtocol(type.ToType(), localPort, datas);
                    break;
                case DYDeviceCommType.ETHERNET:
                    protocol = XGTFEnetProtocol.NewRSSProtocol(type.ToType(), InvokeID, datas);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return protocol;
        }

        /// <summary>
        /// IO Update by async
        /// </summary>
        /// <returns></returns>
        public override async Task UpdateIOAsync(IList<ICommIOData> io_datas)
        {
            foreach (var reqt in Protocols)
            {
                if (CClient.Socket.IsConnected())
                {
                    IPostAsync post = CClient.Socket as IPostAsync;
                    IProtocol resp = await post.PostAsync(reqt);
                    if (resp == null)
                        continue;
                    Dictionary<string, object> storage = resp.GetStorage();
                    if (storage == null || storage.Count == 0)
                        continue;
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        XGTProtocolHelper.Fill(storage, io_datas);
                    }), null);
                }
            }
        }
    }
}