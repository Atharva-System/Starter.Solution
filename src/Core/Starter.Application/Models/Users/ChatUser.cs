namespace Starter.Application.Models.Users;
public class ChatUser
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Time { get; set; } 
    public DateTime Date { get; set; }
    public string Preview { get; set; }
    public List<ChatMessage> Messages { get; set; }
    public bool Active { get; set; }
    public string ConnectionId { get; set; }
}
