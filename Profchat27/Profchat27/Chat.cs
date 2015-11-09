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
    public partial class Chat : Form
    {
        private BackgroundWorker BGWuser;
        private BackgroundWorker BGWchatroom;
        private Administrator Admin;
        private bool closing;
        List<string> users;
        //List<bool> userstati;

        public Chat()
        {
            InitializeComponent();
            Admin = new Administrator(1);
            //Check online users
            BGWuser = new BackgroundWorker();
            BGWuser.DoWork += BGWuser_DoWork;
            BGWuser.RunWorkerCompleted += BGWuser_RunWorkerCompleted;
            BGWuser.ProgressChanged += BGWuser_ProgressChanged;
            BGWuser.WorkerReportsProgress = true;
            BGWuser.RunWorkerAsync();

            //Check active chats
            BGWchatroom = new BackgroundWorker();
            BGWchatroom.DoWork += BGWchatroom_DoWork;
            BGWchatroom.RunWorkerCompleted += BGWchatroom_RunWorkerCompleted;
            BGWchatroom.ProgressChanged += BGWchatroom_ProgressChanged;
            BGWchatroom.WorkerReportsProgress = true;
            BGWchatroom.RunWorkerAsync();
        }

        #region Background working for users
        void BGWuser_DoWork(object sender, DoWorkEventArgs e)
        {
            //repeat every 500ms
            Stopwatch sw = new Stopwatch();
            sw.Start();
            users = new List<string>();
            //userstati = new List<bool>();
            if (Admin.UpdateUsers(out users/*, out userstati*/) == true)
            {
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
            if (sw.ElapsedMilliseconds > 3000)
            {
                MessageBox.Show("De verbinding is te traag, chat wordt gedisconnect!");
            }

        }

        void BGWuser_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdateListbox(users/*, userstati*/);
        }

        void BGWuser_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Check if app is closed
            if (!closing)
            {
                if (!BGWuser.IsBusy)
                {
                    BGWuser.RunWorkerAsync();
                }
            }
        }
        #endregion

        #region Background working for chatrooms
        void BGWchatroom_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        void BGWchatroom_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void BGWchatroom_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void btnStartChat_Click(object sender, EventArgs e)
        {

        }

        void UpdateListbox(List<string> users/*, List<bool> userstati*/)
        {
            //Refresh to new users
            lbContacts.DataSource = null;
            lbContacts.DataSource = users;

            //Set new status colours
            /*
            foreach (bool b in userstati)
            {
                lbContacts.ForeColor = b ? Color.ForestGreen : Color.Maroon;
            }
            */
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbChatroom.SelectedIndex != -1)
            {
                Administrator admintodostuff = ((Chatscreen)(this.MdiChildren.First(f => f.Name == cbChatroom.Text))).a;
                admintodostuff.AddUser(lbContacts.SelectedIndex);
            }
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing = true;
            Admin.GoOffline();
        }
    }
}
