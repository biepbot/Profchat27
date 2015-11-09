using System;
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
        public Account MainUser;

        public Administrator(int userID)
        {
            //Load in everything
            LoadedAccounts = new List<Account>();
            LogIn(userID);
            LoadedChatrooms = Chatroom.GetList(MainUser.ID, new List<Chatroom>());
            //Open chatrooms?

            //Go online
            LogIn(MainUser.ID);
            GoOnline();
        }

        public void LogIn(int userID)
        {
            //Call account to get new main user
            MainUser = Account.GetMainUser(userID);
        }

        public void GoOnline()
        {
            //Call account to set the main user online
            Account.SetOnline(MainUser.ID);
        }

        public void GoOffline()
        {
            //Call account to set the main user offline
            Account.SetOffline(MainUser.ID);
        }

        public void AddUser(int index)
        {

        }

        /// <summary>
        /// Updates the user list
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
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
        /// Updates the message list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newmessages"></param>
        /// <returns></returns>
        public bool UpdateMessages(int id, out List<string> newmessages)
        {
            //Move to chatroom?
            //Call messages for get list, compares this one with the current list
            //Upon difference: return true
            //Else false
            //In case of network failure, return null
            newmessages = null;
            return false;
        }

        /// <summary>
        /// Updates the chatroom list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newrooms"></param>
        /// <returns></returns>
        public bool UpdateChatrooms(int id, out List<int> newrooms)
        {
            //Call chatroom for get list, compares this one with the current list
            //Upon difference: return true
            //Else false
            //In case of network failure, return null
            newrooms = null;
            return false;
        }
    }
}
