using System.Data;
using System;
using MySql.Data.MySqlClient;
using csharp.Chat;
using System.Timers;

namespace Chat
{
    class Program
    {
            static string message = "";
            static int lastKnwonId = GetLastIdFromDB();
            static int newestId = GetLastIdFromDB();
        static void Main(string[] args)
        {
            SetTimer();

            System.Console.WriteLine("Bitte einen Benutzername eingeben: ");
            string username = Console.ReadLine();
            System.Console.WriteLine("Ihr Benutzername lautet: " + username);
            
            RunApplication(message, username);            

        }

        private static void InsertMessageIntoDatabase(string username, string message)
        {
                                
                    MySqlConnection conn = new MySqlConnection("SERVER=localhost;DATABASE=chatDB;UID=max;PASSWORD=123456;port=3336");
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into message(message, messagersname) values(?1,?2)";
                    cmd.Parameters.Add("?1",MySqlDbType.VarChar).Value = message;
                    cmd.Parameters.Add("?2",MySqlDbType.VarChar).Value = username;
                    cmd.Prepare();
                    MySqlDataReader mySqlDataReader;

                    mySqlDataReader = cmd.ExecuteReader();
                    conn.Close();
        }
        private static string DisplayLastMessageWithUsername()
        {
                string messages = "";
                string myConnectionString = "SERVER=localhost;DATABASE=chatDB;UID=max;PASSWORD=123456;port=3336";
                MySqlConnection connection = new MySqlConnection(myConnectionString);
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from message";
                MySqlDataReader reader;
                connection.Open();
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    string message  = (string)reader.GetValue(1).ToString();
                    string username = (string)reader.GetValue(2).ToString();
                    ChatMessage cm = new ChatMessage(message, username);
                    messages =  cm.Username + ": " + cm.Message;
                }
                
                return messages;
        }
                private static string DisplayLastMessage()
        {
                string messages = "";
                string myConnectionString = "SERVER=localhost;DATABASE=chatDB;UID=max;PASSWORD=123456;port=3336";
                MySqlConnection connection = new MySqlConnection(myConnectionString);
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from message";
                MySqlDataReader reader;
                connection.Open();
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    string message  = (string)reader.GetValue(1).ToString();
                    string username = (string)reader.GetValue(2).ToString();
                    ChatMessage cm = new ChatMessage(message, username);
                    messages = cm.Message;
                }
                return messages;
        }
        private static int GetLastIdFromDB()//Get last ID for Displaying
        {
                int idDB = 0;
                string myConnectionString = "SERVER=localhost;DATABASE=chatDB;UID=max;PASSWORD=123456;port=3336";
                MySqlConnection connection = new MySqlConnection(myConnectionString);
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from message";
                MySqlDataReader reader;
                connection.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                     idDB = Convert.ToInt32(reader["id"]);
                }
                connection.Close();
                return idDB;
        }
        private static string CheckForNewMessages()
        {
                string messages = "";
                string myConnectionString = "SERVER=localhost;DATABASE=chatDB;UID=max;PASSWORD=123456;port=3336";
                MySqlConnection connection = new MySqlConnection(myConnectionString);
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from message";
                MySqlDataReader reader;
                connection.Open();
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    string message  = (string)reader.GetValue(1).ToString();
                    string username = (string)reader.GetValue(2).ToString();
                    ChatMessage cm = new ChatMessage(message, username);
                    messages = cm.Message;
                }
                return messages;
        }

        private static void RunApplication(string message, string username)
        {
            while(true)
            {
                message = Console.ReadLine();
                lastKnwonId = GetLastIdFromDB();
                InsertMessageIntoDatabase(username, message);
                newestId = GetLastIdFromDB();
            }
        }

        private static void SetTimer()
        {
            Timer timer = new Timer(1500);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            newestId = GetLastIdFromDB();
            if(newestId > lastKnwonId)
            {
                Console.WriteLine(DisplayLastMessageWithUsername());
                lastKnwonId = GetLastIdFromDB();
            }
        }

    }
}
