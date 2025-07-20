namespace paymentManager.DTOs;

public class ChatMessageDto
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string MessageText { get; set; } = string.Empty;
}
