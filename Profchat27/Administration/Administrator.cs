﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class_Layer;

namespace Administration
{
    public class Administrator
    {
        private List<Chatroom> LoadedChatrooms;
        private List<Account> LoadedAccounts;
        private List<Message> LoadedMessages;
        public Account MainUser;

        public Administrator()
        {
            //Load in everything
            LoadedAccounts = new List<Account>();
            LoadedChatrooms = new List<Chatroom>();
            LoadedMessages = new List<Message>();
        }

        /// <summary>
        /// Logs the main user in
        /// </summary>
        /// <param name="userID"></param>
        public string LogIn(int userID)
        {
            //Call account to get new main user
            MainUser = Account.GetMainUser(userID);
            return MainUser.Name;
        }

        /// <summary>
        /// Sets the main user to online
        /// </summary>
        public void GoOnline()
        {
            //Call account to set the main user online
            Account.SetOnline(MainUser.ID);
        }

        /// <summary>
        /// Sets the main user to offline
        /// </summary>
        public void GoOffline()
        {
            //Call account to set the main user offline
            Account.SetOffline(MainUser.ID);
        }

        /// <summary>
        /// Checks if the user is online or not
        /// </summary>
        /// <param name="index">The index of the loaded user</param>
        /// <returns></returns>
        public bool CheckOnline(int index)
        {
            return LoadedAccounts[index].IsOnline;
        }

        /// <summary>
        /// Creates a chat with both the user selected as well as the main user
        /// </summary>
        /// <param name="index">The index of the loaded user</param>
        /// <returns>Whether the main user is the same as the selected user</returns>
        public bool CreateChat(int index)
        {
            //If not with yourself
            if (MainUser.ID != LoadedAccounts[index].ID)
            {
                //Create chat with main user and indexed user
                Chatroom.CreateRoom(MainUser.ID, LoadedAccounts[index].ID);
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds a user to the chat
        /// </summary>
        /// <param name="index">The index of the loaded user</param>
        /// <param name="chatname">The ID of the chat, aka the chatname</param>
        /// <returns>Whether it succeeded or not</returns>
        public bool AddUser(int index, string chatname, out string error)
        {
            error = string.Empty;
            if (LoadedAccounts[index].IsOnline)
            {
                if (LoadedAccounts[index].ID != MainUser.ID)
                {
                    //Join room with user
                    Chatroom.JoinRoom(LoadedAccounts[index].ID, Convert.ToInt32(chatname));
                    return true;
                }
                error = "Je kan geen gesprek met jezelf starten!";
                return false;
            }
            error = "De gebruiker is niet online!";
            return false;
        }

        /// <summary>
        /// Leaves the specified chat
        /// </summary>
        /// <param name="chatname">The ID or name of the chat</param>
        public void LeaveChat(string chatname)
        {
            Chatroom.LeaveRoom(MainUser.ID, Convert.ToInt32(chatname));
        }

        /// <summary>
        /// Sends a message to the database
        /// </summary>
        /// <param name="text">The text of the message</param>
        /// <param name="roomid">The ID of the room</param>
        public void SendMessage(string text, int roomid)
        {
            Message.Send(text.Trim(), roomid, MainUser.ID);
        }

        /// <summary>
        /// Updates the user list
        /// </summary>
        /// <param name="users"></param>
        /// <returns>Whether there is a update to show or execute</returns>
        public bool UpdateUsers(out List<string> users/*, out List<bool> userstati*/)
        {
            //Call account for get list, compares this one with the current list
            List<Account> newlist = Account.GetList();

            bool changed = false;

            //Add account
            foreach (Account c in newlist)
            {
                if (LoadedAccounts.Count != 0)
                {
                    Account find = LoadedAccounts.FirstOrDefault(a => a.ID == c.ID);

                    //if the account doesn't exist in the loaded list
                    if (find == null)
                    {
                        //add account
                        LoadedAccounts.Add(c);

                        changed = true;
                    }
                    else
                    {
                        //if the account does not match status
                        if (find.IsOnline != c.IsOnline)
                        {
                            //Edit status
                            find.IsOnline = c.IsOnline;

                            changed = true;
                        }
                    }
                }
                else
                {
                    LoadedAccounts.Add(c);
                    changed = true;
                }
            }

            users = new List<string>();

            foreach (Account a in LoadedAccounts)
            {
                string online = a.IsOnline ? "<ONLINE> " : "<OFFLINE> ";
                users.Add(online + a.Name);
            };
            //userstati = LoadedAccounts.Select(c => c.IsOnline).ToList();
            return changed;
        }

        /// <summary>
        /// Updates the user combo box
        /// </summary>
        /// <param name="room">the room name or ID, in int</param>
        /// <param name="users"></param>
        /// <returns>Whether there is a update to show or execute</returns>
        public bool UpdateUsers(int room, out List<string> users)
        {
            bool changes = false;

            //Get all the accounts
            List<Account> accounts = Account.GetList(room);

            Chatroom chat = LoadedChatrooms.Find(ch => ch.ID == room);

            //compare to loaded in accounts
            foreach (Account ac in accounts)
            {
                if (!chat.Accountlist.Contains(ac))
                {
                    changes = true;
                }
            }
            if (changes)
            {
                users = accounts.Select(a => a.Name).ToList();
            }
            else
            {
                users = LoadedAccounts.Select(a => a.Name).ToList();
            }
            return changes;
        }

        /// <summary>
        /// Updates the message list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newmessages"></param>
        /// <returns>Whether there is a update to show or execute</returns>
        public bool UpdateMessages(int id, out List<string> newmessages)
        {
            bool changes = false;
            newmessages = new List<string>();
            //Call messages for get list, compares this one with the current list
            List<Message> newmsgs = Message.GetList(id);
            List<Message> added = new List<Message>();

            //Check if exists
            foreach (Message m in newmsgs)
            {
                //If no messages exist
                if (LoadedMessages.Count != 0)
                {
                    Message find = LoadedMessages.FirstOrDefault(msg => msg.ID == m.ID);
                    //If message doesn't exist
                    if (find == null)
                    {
                        LoadedMessages.Add(m);
                        added.Add(m);
                        changes = true;
                    }
                }
                else
                {
                    LoadedMessages.Add(m);
                    added.Add(m);
                    changes = true;
                }
            }

            foreach (Message m in added.OrderBy(msg => msg.SendDate))
            {
                newmessages.Add(String.Format("<{0}> {1}: {2}", m.SendDate.ToString("HH:mm:ss"), m.Username, m.Text));
            }
            return changes;
        }

        /// <summary>
        /// Updates the chatroom list
        /// </summary>
        /// <returns>Whether there is a update to show or execute</returns>
        public bool UpdateChatrooms(out string chatname, out List<string> usernames, out List<string> closedApps)
        {
            bool changes = false;
            chatname = "";
            usernames = new List<string>();
            closedApps = new List<string>();
            List<Chatroom> added;
            List<Chatroom> removed;

            //Call chatroom for get list, compares this one with the current list
            bool roomsChanged = Chatroom.GetList(MainUser.ID, LoadedChatrooms, out added, out removed);
            if (roomsChanged)
            {
                //Add joined rooms
                foreach (Chatroom c in added)
                {
                    LoadedChatrooms.Add(c);
                    chatname = c.ID.ToString();
                    usernames = c.Accountlist.Select(a => a.Name).ToList();
                    break;
                }

                //Remove left rooms
                foreach (Chatroom c in removed)
                {
                    LoadedChatrooms.Remove(c);
                    closedApps.Add(c.ID.ToString());
                }
                changes = true;
            }

            return changes;
        }
    }
}
