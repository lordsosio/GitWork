using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace ProtocolBlock
{
    public partial class Form1 : Form
    {
        BindingList<UserInfo> userlist;

        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
            button3.Enabled = true;
            timer1.Start();
            userlist = new BindingList<UserInfo>();
            this.dataGridView1.DataSource = userlist;
            this.dataGridView1.Columns[0].Width = 30;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            PcapHelper.WinCapInstance._logAction = Console.WriteLine;
            //PcapHelper.WinCapInstance.StopAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PcapHelper.WinCapInstance.StopAll();
            button1.Enabled = false;
            button3.Enabled = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        delegate void SetAddUserCallback(string ip, string channel);

        private void AddUser(string ip, string channel)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetAddUserCallback d = new SetAddUserCallback(AddUser);
                this.Invoke(d, new object[] { ip , channel });
            }
            else
            {
                for (int i=0;i< userlist.Count;i++)
                {
                    if (userlist[i].IPAddress == ip && userlist[i].ChannelID == channel)
                    {
                        userlist[i].LastUpdate = System.DateTime.Now;
                        userlist[i].Time = ((userlist[i].LastUpdate - userlist[i].FirstConnect).Ticks / TimeSpan.TicksPerSecond).ToString();
                        dataGridView1.Refresh();
                        return;
                    }
                }
                UserInfo info = new UserInfo();
                //info.ChannelID = protocol.Channel;
                info.IPAddress = ip;
                info.ChannelID = channel;
                info.LastUpdate = System.DateTime.Now;
                info.FirstConnect = System.DateTime.Now;
                info.Time = "0";
                userlist.Add(info);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PcapHelper.WinCapInstance._logAction = AppendText;
            PcapHelper.WinCapInstance._addUser = AddUser;
            PcapHelper.WinCapInstance.Listen();
            button3.Enabled = false;
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //UserInfo info = new UserInfo();
            
            //string key = info.IPAddress + "|" + info.ChannelID;
            //if(!userlist.Contains(key))
           //     userlist.Add(info);
           // System.Windows.Forms.BindingSource bs = new System.Windows.Forms.BindingSource();
           // bs.DataSource = userlist.Values;
           // this.dataGridView2.Co
            //this.dataGridView1.DataSource = userlist;
            //DoSendEmptyPackages();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           // this.dataGridView1.DataSource = new BindingList<UserInfo>(userlist);
           // this.dataGridView1.Refresh();
            for (int i = 0; i < userlist.Count; i++)
            {
                userlist[i].Time = ((System.DateTime.Now - userlist[i].FirstConnect).Ticks / TimeSpan.TicksPerSecond).ToString();
                if(Convert.ToInt32(userlist[i].Time) > Options.timeout)
                {
                    AppendText(string.Format("{0} Timeout, Send Empty Package.\n", userlist[i].IPAddress));
                }
            }
            dataGridView1.Refresh();
        }

        private void DoSendEmptyPackages(string ip,string channel)
        {


        }
    }
}
