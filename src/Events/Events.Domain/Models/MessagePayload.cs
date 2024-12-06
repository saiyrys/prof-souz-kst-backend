namespace Events.Domain.Models
{
    public class MessagePayload
    {
        public Event Event { get; set; }
        public ICollection<string> CategoriesId { get; set; }
    }
}