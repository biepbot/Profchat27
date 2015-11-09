namespace Database_Layer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Oracle.DataAccess.Client;

    /// <summary>
    /// The database class to communicate between the application and the database
    /// </summary>
    public static class ChatDatabase
    {
        /// <summary>
        /// The connection string of the chat database
        /// </summary>
        private static string connectionstring = "User Id=test;Password=test;Data Source= //192.168.20.27:1521/XE;";

        /// <summary>
        /// Selects and retrieves values from the database 
        /// </summary>
        /// <param name="query">The selection statement</param>
        /// <returns>A DataTable with the retrieved values></returns>
        public static DataTable RetrieveQuery(string query)
        {
            using (OracleConnection c = new OracleConnection(@connectionstring))
            {
                try
                {
                    c.Open();
                    OracleCommand cmd = new OracleCommand(@query);
                    cmd.Connection = c;
                    try
                    {
                        OracleDataReader r = cmd.ExecuteReader();
                        DataTable result = new DataTable();
                        result.Load(r);
                        c.Close();
                        return result;
                    }
                    catch (OracleException e)
                    {
                        Console.Write(e.Message);
                        throw;
                    }
                }
                catch (OracleException e)
                {
                    Console.Write(e.Message);
                    return new DataTable();
                }
            }
        }

        /// <summary>
        /// Updates the user to the desired username and online status
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <param name="namestr">The name of the user (to update to)</param>
        /// <param name="isOnline">The status of the user (to update to)</param>
        public static void UpdateUser(int id, string namestr, bool isOnline)
        {
            string idstr = id.ToString();
            string isOnlinestr = isOnline ? "1" : "0";

            using (OracleConnection c = new OracleConnection(@connectionstring))
            {
                c.Open();
                OracleCommand cmd = new OracleCommand("UPDATE ACC SET Name = :nam, IsOnline = :ion WHERE ID = :ids");
                cmd.Parameters.Add(new OracleParameter("nam", namestr));
                cmd.Parameters.Add(new OracleParameter("ion", isOnlinestr));
                cmd.Parameters.Add(new OracleParameter("ids", idstr));
                cmd.Connection = c;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                c.Close();
            }
        }

        /// <summary>
        /// Changes the status of a user
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <param name="newvalue">The status of the user</param>
        public static void ChangeUserStatus(int id, bool newvalue)
        {
            string idstr = id.ToString();
            string newvaluestr = newvalue ? "1" : "0";

            using (OracleConnection c = new OracleConnection(@connectionstring))
            {
                c.Open();
                OracleCommand cmd = new OracleCommand("UPDATE ACC SET IsOnline = :ion WHERE ID = :ids");
                cmd.Parameters.Add(new OracleParameter("ion", newvaluestr));
                cmd.Parameters.Add(new OracleParameter("ids", idstr));
                cmd.Connection = c;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                c.Close();
            }
        }

        /// <summary>
        /// Inserts the user to the chatdatabase
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <param name="namestr">The name of the user</param>
        /// <param name="isOnline">The status of the user</param>
        public static void InsertUser(int id, string namestr, bool isOnline)
        {
            string idstr = id.ToString();
            string isOnlinestr = isOnline ? "1" : "0";

            using (OracleConnection c = new OracleConnection(@connectionstring))
            {
                c.Open();
                OracleCommand cmd = new OracleCommand("INSERT INTO ACC (ID, Name, IsOnline) VALUES (:ids, :nam, :ion)");
                cmd.Parameters.Add(new OracleParameter("ids", idstr));
                cmd.Parameters.Add(new OracleParameter("nam", namestr));
                cmd.Parameters.Add(new OracleParameter("ion", isOnlinestr));
                cmd.Connection = c;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                c.Close();
            }
        }

        /// <summary>
        /// Inserts a message to the chatdatabase
        /// </summary>
        /// <param name="userid">The ID of the user</param>
        /// <param name="chatroomid">The ID of the chatroom</param>
        /// <param name="text">The text of the message</param>
        public static void InsertMessage(int userid, int chatroomid, string text)
        {
            string useridstr = userid.ToString();
            string chatroomidstr = chatroomid.ToString();

            using (OracleConnection c = new OracleConnection(@connectionstring))
            {
                c.Open();
                OracleCommand cmd = new OracleCommand("INSERT INTO Messages (UserID, ChatroomID, MessageBody) VALUES(:uid, :cid, :txt)");
                cmd.Parameters.Add(new OracleParameter("uid", useridstr));
                cmd.Parameters.Add(new OracleParameter("cid", chatroomidstr));
                cmd.Parameters.Add(new OracleParameter("txt", text));
                cmd.Connection = c;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                c.Close();
            }
        }

        /// <summary>
        /// Creates a room with the user / adds a user to a room
        /// </summary>
        /// <param name="roomid">The ID of the room</param>
        /// <param name="userid">The ID of the user</param>
        public static void AddToRoom(int roomid, int userid)
        {
            string roomidstr = roomid.ToString();
            string useridstr = userid.ToString();

            using (OracleConnection c = new OracleConnection(@connectionstring))
            {
                c.Open();
                OracleCommand cmd = new OracleCommand("INSERT INTO Chatroom (ID, UserID) VALUES (:rid, :uid)");
                cmd.Parameters.Add(new OracleParameter("rid", roomidstr));
                cmd.Parameters.Add(new OracleParameter("uid", useridstr));
                cmd.Connection = c;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                c.Close();
            }
        }

        /// <summary>
        /// Deletes a room with the user / Removes a user from a room
        /// </summary>
        /// <param name="roomid">The ID of the room</param>
        /// <param name="userid">The ID of the user</param>
        public static void RemoveFromRoom(int roomid, int userid)
        {
            string roomidstr = roomid.ToString();
            string useridstr = userid.ToString();

            using (OracleConnection c = new OracleConnection(@connectionstring))
            {
                c.Open();
                OracleCommand cmd = new OracleCommand("DELETE Chatroom WHERE ID = :rid AND UserID = :uid");
                cmd.Parameters.Add(new OracleParameter("rid", roomidstr));
                cmd.Parameters.Add(new OracleParameter("uid", useridstr));
                cmd.Connection = c;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                c.Close();
            }
        }
    }
}