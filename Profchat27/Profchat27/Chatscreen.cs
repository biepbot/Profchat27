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
        private bool closing;
        private List<string> usernames;

        public Chatscreen(Administrator a, List<string> users)
        {
            InitializeComponent();
            this.Admin = a;
            this.usernames = users;

            //Check online users
            BGWuser = new BackgroundWorker();
            BGWuser.DoWork += BGWuser_DoWork;
            BGWuser.RunWorkerCompleted += BGWuser_RunWorkerCompleted;
            BGWuser.ProgressChanged += BGWuser_ProgressChanged;
            BGWuser.WorkerReportsProgress = true;
            BGWuser.RunWorkerAsync();
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
            if (sw.ElapsedMilliseconds < 501)
            {
                long time = sw.ElapsedMilliseconds;
                while (time < 500 && !closing)
                {
                    time++;
                }
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

        private void btnSend_Click(object sender, EventArgs e)
        {
            Admin.SendMessage(tbMessage.Text, Convert.ToInt32(this.Name));
        }

        private void Chatscreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing = true;
        }
    }
}
