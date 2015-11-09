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
        /// Whether the chatroom form is opened or not
        /// </summary>
        public bool ScreenOpen { get; set; }
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
            ScreenOpen = false;
            this.ID = ID;
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
                }
            }

            //Compare the ID's, remove a chatroom when an id in 'ids' is not found
            foreach (Chatroom c in loadedRooms)
            {
                if (ids.Select(i => i == c.ID) == null)
                {
                    Chatrooms.Remove(c);
                }
            }

            return Chatrooms;
        }

        /// <summary>
        /// Creates a room for both users
        /// </summary>
        /// <param name="userid1">User ID 1</param>
        /// <param name="userid2">User ID 2</param>
        public static void CreateRoom(int userid1, int userid2)
        {
            Database_Layer.ChatDatabase.CreateRoom(userid1, userid2);
        }

        /// <summary>
        /// Adds the user to a room
        /// </summary>
        /// <param name="userid">The userID</param>
        public static void JoinRoom(int userid, int roomid)
        {
            Database_Layer.ChatDatabase.AddToRoom(roomid, userid);
        }

        /// <summary>
        /// Removes the user from a room
        /// </summary>
        /// <param name="userid"></param>
        public static void LeaveRoom(int userid, int roomid)
        {
            Database_Layer.ChatDatabase.RemoveFromRoom(roomid, userid);
        }
    }
}
