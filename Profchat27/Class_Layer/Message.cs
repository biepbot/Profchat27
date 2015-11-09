using System;
using System.Collections.Generic;
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
        /// Initializes a new instance of the message class
        /// </summary>
        /// <param name="id">the ID of the message</param>
        /// <param name="text">the text of the message</param>
        private Message(int id, string text)
        {
            this.ID = id;
            this.Text = text;
        }

        /// <summary>
        /// Sends a message to the database, waiting to be received later upon update
        /// </summary>
        /// <param name="text">The message to send</param>
        public static void Send(string text)
        {
            //TODO
            //Insert message into database
        }

        /// <summary>
        /// Yields all the messages matching the chatroomID
        /// </summary>
        /// <param name="roomid">The ID of the chatroom</param>
        /// <returns>All the found messages</returns>
        public static List<Message> GetList(int roomid)
        {
            //TODO
            //Call database to load in all the messages for this chatroomID
            return null;
        }
    }
}
