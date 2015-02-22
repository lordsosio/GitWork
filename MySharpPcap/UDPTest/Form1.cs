using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPTest
{

    public partial class Form1 : Form
    {
        /// <summary> 
        /// 字符串转16进制字节数组 
        /// </summary> 
        /// <param name="hexString"></param> 
        /// <returns></returns> 
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += "0";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        } 


        public Form1()
        {
            InitializeComponent();
        }

        delegate void SetAppendCallback(string text);//后加的，好好想一想,参数是SetText带的参数。

        private void AppendText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetAppendCallback d = new SetAppendCallback(AppendText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.AppendText(text);
                this.textBox1.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReceiveMessage._logAction = AppendText;
            ReceiveMessage._sendBack = SendBack;
            ReceiveMessage.ReceiveStart(textBox4.Text, textBox2.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void SendBack(string content)
        {
            if (textBox6.Text != "")
            {
                SendMessage sm = new SendMessage();
                //sm.SendMsgStart(content, textBox6.Text, textBox7.Text);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ReceiveMessage._logAction = AppendText;
            ReceiveMessage._sendBack = SendBack;
            ReceiveMessage.CloseReceiveUdpClient();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendMessage._logAction = AppendText;
            string sendText = textBox5.Text;
            byte[] data = strToToHexByte(sendText);
            //sendText = string.Join(",", data.Select(t => t.ToString()).ToArray());
            //string str = System.Text.Encoding.Default.GetString(data);
            SendMessage sm1 = new SendMessage();
            SendMessage sm2 = new SendMessage();
            sm1.SendMsgStart(data, textBox8.Text, textBox3.Text);

            sm2.SendMsgStart(data, textBox9.Text, textBox10.Text);
        }
    }
}
