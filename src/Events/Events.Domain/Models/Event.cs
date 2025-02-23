using Events.Shared.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Domain.Models
{
    public class Event
    {
        [Column("eventId")]
        public string? EventId { get; private set; }
        [Column("title")]
        public string? Title { get; private set; }
        [Column("description")]
        public string? Description { get; private set; }
        [Column("organizer")]
        public string? Organizer { get; private set; }
        [Column("eventDate")]
        public DateTime EventDate { get; private set; } 
        [Column("link")]
        public string? Link { get; private set; }
        [Column("createdAt")]
        public DateTime CreatedAt { get; private set; }
        [Column("updatedAt")]
        public DateTime UpdatedAt { get; private set; }


        public Event()
        {
            EventId = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = CreatedAt;

           
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Название не может быть пустым!");

            Title = title;
            UpdatedAt = DateTime.Now;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException("Описание не может быть пустым!");

            Description = description;
            UpdatedAt = DateTime.Now;
        }

        public void SetOrganizer(string organizer)
        {
            if (string.IsNullOrEmpty(organizer))
                throw new ArgumentException("Поле организаторов не может быть пустым!");

            Organizer = organizer;
            UpdatedAt = DateTime.Now;

        }
        public void SetEventDate(DateTime date)
        {
            if (date < DateTime.Now)
                throw new ArgumentException("Дата мероприятия не может быть в прошлом!");

            EventDate = date.ToUniversalTime();
           
            UpdatedAt = DateTime.Now;

        }

        public void SetLink(string link)
        {
            Link = link;
            UpdatedAt = DateTime.Now;
        }
    }
}
