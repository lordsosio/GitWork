using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace UDPTest
{
    public class ReceiveMessage
    {
        public static Action<string> _logAction;
        public static Action<string> _sendBack;
        private static UdpClient receiveUdpClient;

        //private static SystemLog systemLog = new SystemLog();
        #region 接受消息
        public static void ReceiveStart(string localip, string localPort)
        {
            //创建接受套接字
            IPAddress localIP = IPAddress.Parse(localip);
            IPEndPoint localIPEndPoint = new IPEndPoint(localIP, int.Parse(localPort));
            receiveUdpClient = new UdpClient(localIPEndPoint);
            //启动接受线程
            Thread threadReceive = new Thread(ReceiveMessages);
            threadReceive.IsBackground = true;
            threadReceive.Start();
            //显示状态
            //ShwMsgForView.ShwMsgforView(listBox, "接受线程启动");
            //将数据存入数据库
            //systemLog.SaveSystemLog("", "接受线程启动", "管理员");
            _logAction("Thread start!\n");
        }

        private static void ReceiveMessages()
        {
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    //关闭receiveUdpClient时此句会产生异常

                    byte[] receiveBytes = receiveUdpClient.Receive(ref remoteIPEndPoint);
                    for (int i = 0; i < receiveBytes.Length; i++)
                    {
                        //ShwMsgForView.ShwMsgforView(listBox, string.Format("{0}[{1}]", remoteIPEndPoint, receiveBytes[i].ToString()));
                    }
                    // string message = Encoding.Unicode.GetString(receiveBytes, 0, receiveBytes.Length);
                    string message = Encoding.ASCII.GetString(receiveBytes, 0, receiveBytes.Length);
                    //显示接受到的消息内容
                    //ShwMsgForView.ShwMsgforView(listBox, 
                    
                    _logAction(string.Format("{0}[{1}]\n", remoteIPEndPoint, message));
                    _sendBack(message);

                    //send back
                    //receiveUdpClient.Send(receiveBytes, receiveBytes.Length);
                }
                catch
                {
                    break;
                }
            }
        }

        public static void CloseReceiveUdpClient()
        {
            receiveUdpClient.Close();
            //ShwMsgForView.ShwMsgforView(listBox, "接收线程停止");
            //systemLog.SaveSystemLog("", "接收线程停止", "管理员");
            _logAction("Thread stop\n");
        }
        #endregion
    }
}
