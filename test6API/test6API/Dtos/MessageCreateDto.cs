namespace test6API.Dtos
{
    public class MessageCreateDto
    {
        public int ConversationId { get; set; }
        public string SenderType { get; set; }
        public string SenderEmail { get; set; }
        public string MessageText { get; set; }
    }
}