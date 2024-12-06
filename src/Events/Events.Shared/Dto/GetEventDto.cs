
namespace Events.Shared.Dto
{
    public class GetEventDto
    {
        public string eventId { get; set; }
       /* public List<GetUploadsDto> images { get; set; }*/
        public string? title { get; set; }
        public string organizer { get; set; }
        public string? description { get; set; }
        public string eventDate { get; set; }
        public string link { get; set; }
        public ICollection<string> categories { get; set; }
        
        public int totalTickets { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }/* public bool isActive { get; set; }
         public bool status { get; set; }*/
}
