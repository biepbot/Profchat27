using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Layer
{
    public class Account
    {
        /// <summary>
        /// The ID of the user as stated in both of the databases
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// The name of the user as stated in the chat database
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The online status of the user as stated in the chat database
        /// </summary>
        public bool IsOnline { get; private set; }

        /// <summary>
        /// The last seen status of the user
        /// </summary>
        public DateTime LastSeen { get; private set; }

        /// <summary>
        /// Initializes an instance of the account class
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <param name="name">The name of the user</param>
        /// <param name="isOnline">The online status of the user</param>
        private Account(int id, string name, int isOnline, DateTime lastseen)
        {
            this.ID = id;
            this.Name = name;
            this.IsOnline = isOnline == 0 ? false : true;
            this.LastSeen = lastseen;
        }

        /// <summary>
        /// Initializes an instance of the account class through its ID
        /// </summary>
        /// <param name="id">The ID of the user</param>
        private Account(int id)
        {
            //Fetch Account from database
            DataTable AccountTable = Database_Layer.ChatDatabase.RetrieveQuery("SELECT * FROM ACC WHERE ID = " + id);
            foreach (DataRow AccountInformation in AccountTable.Rows)
            {
                this.ID = Convert.ToInt32(AccountInformation["ID"]);
                this.Name = AccountInformation["Name"].ToString();
                this.IsOnline = AccountInformation["IsOnline"].ToString() == "0" ? false : true;
                this.LastSeen = Convert.ToDateTime(AccountInformation["LastSeen"]);
            }
        }

        /// <summary>
        /// Retrieves the account information of the user logging in
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns></returns>
        public static Account GetMainUser(int id)
        {
            //Validates the ID, making sure the user exists and is up-to-date
            return Validate(id);
        }

        /// <summary>
        /// Sets the status of the account to online
        /// </summary>
        /// <param name="id">the ID of the user</param>
        public static void SetOnline(int id)
        {
            //Call database to set user online
            Database_Layer.ChatDatabase.ChangeUserStatus(id, true);
        }

        /// <summary>
        /// Sets the status of the account to offline
        /// </summary>
        /// <param name="id">the ID of the user</param>
        public static void SetOffline(int id)
        {
            //Call database to set user offline
            Database_Layer.ChatDatabase.ChangeUserStatus(id, false);
        }

        /// <summary>
        /// Yields all the users
        /// </summary>
        /// <returns>All the found users</returns>
        public static List<Account> GetList()
        {
            List<Account> AllAccounts = new List<Account>();
            //Call database for all the users
            DataTable AllAccountsTable = Database_Layer.ChatDatabase.RetrieveQuery("SELECT * FROM ACC");
            foreach (DataRow AccountInformation in AllAccountsTable.Rows)
            {
                //Create all the accounts for them
                AllAccounts.Add(new Account(
                    Convert.ToInt32(AccountInformation["ID"]),
                    AccountInformation["Name"].ToString(),
                    Convert.ToInt32(AccountInformation["IsOnline"]),
                    Convert.ToDateTime(AccountInformation["LastSeen"])
                ));
            }
            return AllAccounts;
        }

        /// <summary>
        /// Yields all the users matching the chatroomID
        /// </summary>
        /// <param name="roomid">The ID of the chatroom</param>
        /// <returns>All the found users</returns>
        public static List<Account> GetList(int roomid)
        {
            //TODO
            //Call database to load in all the messages for this chatroomID
            return null;
        }

        /// <summary>
        /// Validates the user and makes sure it is up to date for all the other users
        /// </summary>
        /// <param name="id">the ID of the user</param>
        /// <returns>The account after it's been verified</returns>
        private static Account Validate(int id)
        {
            //TODO
            //Call database to check if user exists
            //If exists: 
            //Check if user matches
            //Else:
            //Insert new user into database
            //If exists but does not match
            //Update user into database

            //Finally:
            //Create an account instance
            //Return this one
            return null;
        }
    }
}
