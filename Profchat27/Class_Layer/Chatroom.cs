using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Layer
{
    public class Chatroom
    {
        /// <summary>
        /// The ID of the chatroom, as stated in the chat database
        /// </summary>
        public int ID {get; private set;}
        /// <summary>
        /// The list of users in this chatroom
        /// </summary>
        public List<Account> Accountlist { get; private set; }
        /// <summary>
        /// The list of messages in this chatroom
        /// </summary>
        public List<Message> MessageList { get; private set; }

        /// <summary>
        /// Initializes a new instance of a chatroom, as loaded in from the database
        /// </summary>
        /// <param name="ID">The ID of the chatroom</param>
        private Chatroom(int ID)
        {
            //TODO
            //Call database to load in all the users in this chatroom
            //Call database to load in all the messages of this chatroom
        }

        /// <summary>
        /// Yields all the chatrooms matching the userID
        /// </summary>
        /// <param name="userid">the ID of the user</param>
        /// <returns>All the found chatrooms</returns>
        public static List<Chatroom> GetList(int userid)
        {
            //TODO
            //Call database to load in all the chatrooms for this userID
            return null;
        }
    }
}
