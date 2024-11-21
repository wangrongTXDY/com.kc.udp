using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace KC
{
    public class UDP
    {
        private UdpClient _udpClient;
        
        /// <summary>
        /// 对方网络点
        /// </summary>
        private IPEndPoint _ipEndPoint;

        public event EventHandler<string> UDPReceiveData;
        
        /// <summary>
        /// 创建一个UDP连接
        /// </summary>
        /// <param name="endPoint">对方点</param>
        /// <param name="myPort">端口</param>
        public void Create(IPEndPoint endPoint,int myPort)
        {
            _ipEndPoint = endPoint;
            _udpClient = new UdpClient(myPort);
            _udpClient.BeginReceive(Receive,null);
        }

        private void Receive(IAsyncResult asyncResult)
        {
            try
            {
                var data = Encoding.UTF8.GetString(_udpClient.EndReceive(asyncResult, ref _ipEndPoint));
                UDPReceiveData?.Invoke(this,data);
                _udpClient.BeginReceive(Receive, _udpClient);
            }
            catch (Exception)
            {
                Console.WriteLine("网络异常");
            }
            
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="ipEndPoint">终点</param>
        /// <param name="str">数据</param>
        public void Send(IPEndPoint ipEndPoint, string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            _udpClient.BeginSend(bytes,bytes.Length, ipEndPoint,SendCallBack,"消息发送成功");
        }

        private void SendCallBack(IAsyncResult asyncResult)
        {
            //Debug.Log(asyncResult.AsyncState.ToString());
        }

        public void Dispose()
        {
            _udpClient.Dispose();
            _ipEndPoint=null;
            UDPReceiveData=null;
        }
    }
}