namespace RealState.Core.Application.ViewModels.Message
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime SentAt { get; set; }
        public bool IsFromAgent { get; set; }
    }
}
