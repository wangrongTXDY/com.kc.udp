using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

namespace KC
{
    /// <summary>
    /// udp管理组件
    /// </summary>
    public class UDPManagerComponent
    {
        private Dictionary<string, UDP> _udpClients;
        
        /// <summary>
        /// 构造表记录
        /// </summary>
        public void Init()
        {
            _udpClients = new Dictionary<string, UDP>();
        }
        
        /// <summary>
        /// 创建UDP连接
        /// </summary>
        /// <param name="endIp"></param>
        /// <param name="endPort"></param>
        /// <param name="myPort"></param>
        /// <param name="udpName"></param>
        public UDP CreateUdp(string endIp,int endPort,int myPort,string udpName)
        {
            var udp = new UDP();
            udp.Create(new IPEndPoint(IPAddress.Parse(endIp),endPort),myPort);
            _udpClients.Add(udpName,udp);
            return udp;
        }

        /// <summary>
        /// 获取UDP
        /// </summary>
        /// <param name="udpName">UDP名称</param>
        /// <returns></returns>
        public UDP GetUdp(string udpName)
        {
            if (_udpClients.TryGetValue(udpName, out var udp))
            {
                return udp;
            }

            Debug.LogError("请先创建该网络!");
            return default;
        }
        
        
        public void RemoveUdp(string udpName)
        {
            if (!_udpClients.ContainsKey(udpName)) return;
            _udpClients[udpName].Dispose();
            _udpClients.Remove(udpName);
        }
        
        public void RemoveAllUdp()
        {
            var keys = _udpClients.Keys.ToArray();
            for (int i = keys.Length-1; i >=0; i--)
            {
                _udpClients[keys[i]].Dispose();
                _udpClients.Remove(keys[i]);
            }
        }

        public void Destroy()
        {
            RemoveAllUdp();
            _udpClients = null;
        }
    }
}