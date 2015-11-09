using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Class_Layer
{
    public class Message
    {
        /// <summary>
        /// The ID of the message, as stated in the chat database
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// The text of the message, as stated in the chat database
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// ID of the room the message is send in
        /// </summary>
        public int RoomID { get; private set; }
        /// <summary>
        /// ID of the user inside a room
        /// </summary>
        public int UserID { get; private set; }
        /// <summary>
        /// The name of the User
        /// </summary>
        public string Username { get; private set; }
        /// <summary>
        /// Date on which the message has been send
        /// </summary>
        public DateTime SendDate { get; private set; }

        private Message(int id, string text, int roomID, int userID, DateTime sendOn, string user)
        {
            this.ID = id;
            this.Text = text;
            this.RoomID = roomID;
            this.UserID = userID;
            this.SendDate = sendOn;
            this.Username = user;
        }

        /// <summary>
        /// Sends a message to the database, waiting to be received later upon update
        /// </summary>
        /// <param name="text">The message to send</param>
        public static void Send(string text, int roomID, int userID)
        {
            Database_Layer.ChatDatabase.InsertMessage(userID, roomID, text);
        }

        /// <summary>
        /// Yields all the messages matching the chatroomID
        /// </summary>
        /// <param name="roomid">The ID of the chatroom</param>
        /// <returns>All the found messages</returns>
        public static List<Message> GetList(int roomid)
        {
            List<Message> msg = new List<Message>();
            DataTable dt = Database_Layer.ChatDatabase.RetrieveQuery(
                "SELECT * FROM Messages m " +
                "JOIN Acc a " +
                "ON a.ID = m.UserID " +
                "WHERE ChatroomID = " + roomid);
            foreach (DataRow row in dt.Rows)
            {
                msg.Add(new Message(
                    Convert.ToInt32(row["ID"]),
                    Convert.ToString(row["Messagebody"]),
                    Convert.ToInt32(row["chatroomID"]),
                    Convert.ToInt32(row["UserID"]),
                    Convert.ToDateTime(row["SendDate"]),
                    row["Name"].ToString()
                    ));
            }
            return msg;
        }
    }
}
