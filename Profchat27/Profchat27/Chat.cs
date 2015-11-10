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
        private List<string> closedChats;

        public List<Chatscreen> screens;
        //List<bool> userstati;

        public Chat(int id)
        {
            InitializeComponent();
            screens = new List<Chatscreen>();
            Admin = new Administrator();
            lblName.Text = Admin.LogIn(id);

            //Check online users
            BGWuser = new BackgroundWorker();
            BGWuser.DoWork += BGWuser_DoWork;
            BGWuser.RunWorkerCompleted += BGWuser_RunWorkerCompleted;
            BGWuser.ProgressChanged += BGWuser_ProgressChanged;
            BGWuser.WorkerReportsProgress = true;
            BGWuser.RunWorkerAsync();

            Admin.GoOnline();

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
            long time = sw.ElapsedMilliseconds;
            if (time < 501)
            {
                Thread.Sleep((501 - Convert.ToInt32(time)));
            }
            if (sw.ElapsedMilliseconds > 3000)
            {
                MessageBox.Show("De verbinding is te traag! \nMogelijk hebt u uw netwerkverbinding verloren.");
                //TODO
                //Do stuff like, disconnecting
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
            closedChats = new List<string>();
            if (Admin.UpdateChatrooms(out Lastchatname, out Lastchatusernames, out closedChats) == true)
            {
                //In case of change, call function to show screen with users
                BGWchatroom.ReportProgress(1);
            }
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            if (time < 501)
            {
                Thread.Sleep((501 - Convert.ToInt32(time)));
            }
            sw.Stop();
        }

        void BGWchatroom_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Lastchatname))
            {
                Chatscreen child = new Chatscreen(Admin, Lastchatusernames, this);
                child.Name = Lastchatname;
                child.Text = Lastchatname;
                screens.Add(child);
                child.Show();
            }
            foreach (string s in closedChats)
            {
                screens.Remove(
                    screens.FirstOrDefault(c => c.Name == s)
                    );
            }

            cbChatroom.Items.Clear();
            UpdateCB();
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

        public void UpdateCB()
        {
            int i = cbChatroom.SelectedIndex;
            foreach (Chatscreen c in screens)
            {
                cbChatroom.Items.Add(c.Name);
            }
            cbChatroom.SelectedIndex = i;
        }

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
            int i = lbContacts.SelectedIndex;
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
            lbContacts.SelectedIndex = i;
        }

        /// <summary>
        /// Invites a user to the chatroom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            if (cbChatroom.SelectedIndex != -1 && lbContacts.SelectedIndex != -1)
            {
                if (!Admin.AddUser(lbContacts.SelectedIndex, cbChatroom.Text, out error))
                {
                    MessageBox.Show(error);
                }
            }
            else
            {
                MessageBox.Show("Selecteer een gebruiker en chatroom om deze gebruiker aan toe te voegen.");
            }
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing = true;
            Admin.GoOffline();
        }
    }
}
