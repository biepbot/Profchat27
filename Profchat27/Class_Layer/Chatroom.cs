using System;
using System.Collections.Generic;
using System.Data;
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
        public int ID { get; private set; }
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
            //Load in all the users in this chatroom
            this.Accountlist = Account.GetList(ID);
            //Load in all the messages of this chatroom
            this.MessageList = Message.GetList(ID);
        }

        /// <summary>
        /// Yields all the chatrooms matching the userID
        /// </summary>
        /// <param name="userid">the ID of the user</param>
        /// <returns>All the found chatrooms</returns>
        public static List<Chatroom> GetList(int userid, List<Chatroom> loadedRooms)
        {
            //TODO
            List<Chatroom> Chatrooms = loadedRooms;
            //Call database to load in all the chatrooms for this userID
            DataTable ChatroomTable = Database_Layer.ChatDatabase.RetrieveQuery("SELECT * FROM Chatroom WHERE UserID = " + userid);

            //Save all the ID's which are found
            List<int> ids = new List<int>();
            foreach (DataRow ChatroomInformation in ChatroomTable.Rows)
            {
                ids.Add(Convert.ToInt32(ChatroomInformation["ID"]));
            }

            //Compare the ID's, create a chatroom when an id in 'ids' is not found
            foreach (int id in ids)
            {
                if (loadedRooms.FirstOrDefault(c => c.ID == id) == null)
                {
                    Chatrooms.Add(new Chatroom(id));
                    //Add user to room
                    Database_Layer.ChatDatabase.AddToRoom(id, userid);
                }
            }

            //Compare the ID's, remove a chatroom when an id in 'ids' is not found
            foreach (Chatroom c in loadedRooms)
            {
                if (ids.Select(i => i == c.ID) == null)
                {
                    Chatrooms.Remove(c);
                    //Remove user from room
                    Database_Layer.ChatDatabase.RemoveFromRoom(c.ID, userid);
                }
            }

            return Chatrooms;
        }
    }
}
