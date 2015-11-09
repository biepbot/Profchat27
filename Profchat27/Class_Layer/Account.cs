using System;
using System.Collections.Generic;
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
        /// Initializes an instance of the account class
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <param name="name">The name of the user</param>
        /// <param name="isOnline">The online status of the user</param>
        private Account(int id, string name, int isOnline)
        {
            this.ID = id;
            this.Name = name;
            this.IsOnline = isOnline == 0 ? false : true;
        }

        /// <summary>
        /// Initializes an instance of the account class through its ID
        /// </summary>
        /// <param name="id">The ID of the user</param>
        private Account(int id)
        {
            //TODO
            //Fetch Account from database
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
            //TODO
            //Call database to set user online (updateaccount)
        }

        /// <summary>
        /// Sets the status of the account to offline
        /// </summary>
        /// <param name="id">the ID of the user</param>
        public static void SetOffline(int id)
        {
            //TODO
            //Call database to set user offline (updateaccount)
        }

        /// <summary>
        /// Yields all the users
        /// </summary>
        /// <returns>All the found users</returns>
        public static List<Account> GetList()
        {
            //TODO
            //Call database for all the users
            //Create all the accounts for them
            return null;
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
