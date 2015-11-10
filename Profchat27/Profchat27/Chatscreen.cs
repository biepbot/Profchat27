using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Administration;

namespace Profchat27
{
    public partial class Chatscreen : Form
    {
        public Administrator Admin;
        BackgroundWorker BGWuser;
        BackgroundWorker BGWmsg;
        private bool closing;
        private List<string> usernames;
        private List<string> messages;
        Form Previous;

        public Chatscreen(Administrator a, List<string> users, Form previous)
        {
            InitializeComponent();
            this.Admin = a;
            this.usernames = users;
            messages = new List<string>();
            this.Previous = previous;

            //Check online users
            BGWuser = new BackgroundWorker();
            BGWuser.DoWork += BGWuser_DoWork;
            BGWuser.RunWorkerCompleted += BGWuser_RunWorkerCompleted;
            BGWuser.ProgressChanged += BGWuser_ProgressChanged;
            BGWuser.WorkerReportsProgress = true;
            BGWuser.RunWorkerAsync();

            BGWmsg = new BackgroundWorker();
            BGWmsg.DoWork += BGWmsg_DoWork;
            BGWmsg.RunWorkerCompleted += BGWmsg_RunWorkerCompleted;
            BGWmsg.ProgressChanged += BGWmsg_ProgressChanged;
            BGWmsg.WorkerReportsProgress = true;
            BGWmsg.RunWorkerAsync();
        }

        #region Background worker for chat users
        private void BGWuser_DoWork(object sender, DoWorkEventArgs e)
        {
            //repeat every 500ms
            Stopwatch sw = new Stopwatch();
            sw.Start();
            usernames = new List<string>();
            if (Admin.UpdateUsers(Convert.ToInt32(this.Name), out usernames) == true)
            {
                //In case of change, call function to show screen with users
                BGWuser.ReportProgress(1);
            }
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            if (time < 501)
            {
                Thread.Sleep((501 - Convert.ToInt32(time)));
            }
        }

        private void BGWuser_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Change CB
            cbOnlineUsers.DataSource = null;
            cbOnlineUsers.DataSource = usernames;
        }

        private void BGWuser_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!closing)
            {
                if (!BGWuser.IsBusy)
                {
                    BGWuser.RunWorkerAsync();
                }
            }
        }
        #endregion

        #region Background worker for messages
        private void BGWmsg_DoWork(object sender, DoWorkEventArgs e)
        {
            //repeat every 500ms
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (Admin.UpdateMessages(Convert.ToInt32(this.Name), out messages) == true)
            {
                //In case of change, call function to show screen with users
                BGWmsg.ReportProgress(1);
            }
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            if (time < 501)
            {
                Thread.Sleep((501 - Convert.ToInt32(time)));
            }
        }

        private void BGWmsg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //AddMessage
            foreach (string s in messages)
            {
                tbBerichten.AppendText(s + "\n");
            }

        }

        private void BGWmsg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!closing)
            {
                if (!BGWmsg.IsBusy)
                {
                    BGWmsg.RunWorkerAsync();
                }
            }
        }
        #endregion

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbMessage.Text))
            {
                Admin.SendMessage(tbMessage.Text, Convert.ToInt32(this.Name));
                tbMessage.Clear();
            }
        }

        private void Chatscreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing = true;
            Admin.LeaveChat(this.Name);
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend.PerformClick();
            }
        }
    }
}
