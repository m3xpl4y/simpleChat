namespace csharp.Chat
{
    public class ChatMessage
    {
        public int id { get; set; }
        
        public string Message { get; set; }
        
        public string Username { get; set; }

        public ChatMessage(string message, string username)
        {
            Message = message;
            Username = username;
        }
    }
}