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

        //public delegate Nullable<bool> UpdateUser(string info);
        //public event UpdateUser doUpdate = new UpdateUser(UpdateUsers);

        public Administrator(int userID)
        {
            //Load in everything
            LoadedAccounts = Account.GetList();
            LogIn(userID);
            LoadedChatrooms = Chatroom.GetList(MainUser.ID, new List<Chatroom>());
            //Open chatrooms?

            //Go online
            LogIn(MainUser.ID);
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

        public Nullable<bool> UpdateUsers(string h)
        {
            //Call account for get list, compares this one with the current list
            //Upon difference: return true
            //Else false
            //In case of network failure, return null
            return null;
        }

        public Nullable<bool> UpdateMessages(int id)
        {
            //Move to chatroom?
            //Call messages for get list, compares this one with the current list
            //Upon difference: return true
            //Else false
            //In case of network failure, return null
            return null;
        }

        public Nullable<bool> UpdateChatrooms(int id)
        {
            //Call chatroom for get list, compares this one with the current list
            //Upon difference: return true
            //Else false
            //In case of network failure, return null
            return null;
        }
    }
}
