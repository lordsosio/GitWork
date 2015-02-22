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
    public class SendMessage
    {
        public static Action<string> _logAction;
        private  UdpClient sendUdpClient;
        private  string sendIP;
        private  string sendToPort;
        private  ListBox listBox;
        #region 发送消息
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sendMsg"></param>
        /// <param name="sendIp"></param>
        /// <param name="sendPort"></param>
        public void SendMsgStart(byte[] sendMsg, string sendIp, string sendPort)
        {
            //给参数赋值
            sendIP = sendIp;
            sendToPort = sendPort;

            //选择发送模式
            //固定为匿名模式（套接字绑定的端口由系统自动分配）
            sendUdpClient = new UdpClient(0);
            //启动发送线程
            Thread threadSend = new Thread(SendMessages);
            threadSend.IsBackground = true;
            threadSend.Start(sendMsg);
            //显示状态
            //ShwMsgForView.ShwMsgforView(listBox, "发送线程启动");
            //将数据存入数据库
            //systemLog.SaveSystemLog("", "发送线程启动", "管理员");
        }

        private  void SendMessages(object obj)
        {
            byte[] sendbytes = (byte[])obj;
            //byte[] sendbytes = Encoding.Unicode.GetBytes(message);
            //使用Default编码，如果使用Unicode编码的话，每个字符中间都会有个0，影响解码
            //byte[] sendbytes = Encoding.Default.GetBytes(message);
            IPAddress remoteIp = IPAddress.Parse(sendIP);
            IPEndPoint remoteIPEndPoint = new IPEndPoint(remoteIp, int.Parse(sendToPort));
            sendUdpClient.Send(sendbytes, sendbytes.Length, remoteIPEndPoint);
            while (true)
            {
                try
                {
                    byte[] receiveBytes = sendUdpClient.Receive(ref remoteIPEndPoint);
                    
                    for (int i = 0; i < receiveBytes.Length; i++)
                    {
                        //ShwMsgForView.ShwMsgforView(listBox, string.Format("{0}[{1}]", remoteIPEndPoint, receiveBytes[i].ToString()));
                        _logAction(string.Format("{0}[{1}]", remoteIPEndPoint, receiveBytes[i].ToString()));
                    }
                }catch(Exception e)
                {
                    break;
                }
            }
            sendUdpClient.Close();
            _logAction("Message Send\n");
            //ShwMsgForView.ShwMsgforView(listBox, "发送消息：" + message);
            //systemLog.SaveSystemLog("", "发送消息,目标：" + remoteIPEndPoint + ",消息内容为:" + message + "", "管理员");
        }
        #endregion
    }
}

