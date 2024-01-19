namespace Starter.Application.Models.Users;
public class ChatMessage : INotifyParam
{
    public string FromUserId { get; set; }
    public string ToUserId { get; set; }
    public string Text { get; set; }
    public string Time { get; set; }
    public DateTime Date { get; set; }
}
