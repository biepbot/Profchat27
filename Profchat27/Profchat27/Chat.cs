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

        private string Lastchatname;
        private List<string> Lastchatusernames;

        private List<Chatscreen> screens;
        //List<bool> userstati;

        public Chat()
        {
            InitializeComponent();
            screens = new List<Chatscreen>();
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
                //In case of change, call function to update the lisbox of users
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
            //repeat every 500ms
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Lastchatname = "";
            Lastchatusernames = new List<string>();
            if (Admin.UpdateChatrooms(out Lastchatname, out Lastchatusernames) == true)
            {
                //In case of change, call function to show screen with users
                BGWchatroom.ReportProgress(1);
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

        void BGWchatroom_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Chatscreen child = new Chatscreen(Admin, Lastchatusernames);
            child.Name = Lastchatname;
            screens.Add(child);
            child.Show();

            cbChatroom.Items.Clear();
            foreach (Chatscreen c in screens)
            {
                cbChatroom.Items.Add(c.Name);
            }
        }

        void BGWchatroom_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Check if app is closed
            if (!closing)
            {
                if (!BGWchatroom.IsBusy)
                {
                    BGWchatroom.RunWorkerAsync();
                }
            }
        }

        #endregion

        /// <summary>
        /// Starts a chat with the selected person
        /// </summary>
        private void btnStartChat_Click(object sender, EventArgs e)
        {
            if (Admin.CheckOnline(lbContacts.SelectedIndex))
            {
                //Call for admin
                if (!Admin.CreateChat(lbContacts.SelectedIndex))
                {
                    MessageBox.Show("Je kan geen gesprek met jezelf starten!");
                }
            }
            else
            {
                MessageBox.Show("Deze gebruiker is niet online!");
            }
        }

        /// <summary>
        /// Updates the user list box to match the online and offline users
        /// </summary>
        /// <param name="users"></param>
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

        /// <summary>
        /// Invites a user to the chatroom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbChatroom.SelectedIndex != -1)
            {
                //TODO
                //Administrator admintodostuff = ((Chatscreen)(this.MdiChildren.First(f => f.Name == cbChatroom.Text))).admin;
                //admintodostuff.AddUser(lbContacts.SelectedIndex, cbChatroom.Text);
            }
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing = true;
            Admin.GoOffline();
        }
    }
}
