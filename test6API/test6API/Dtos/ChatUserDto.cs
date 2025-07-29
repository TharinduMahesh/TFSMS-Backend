namespace test6API.Dtos
{
    public class ChatUserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserType { get; set; } // "Grower" or "Collector"
    }
}
