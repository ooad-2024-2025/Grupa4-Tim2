namespace SarajevoGuide.Models
{
    public class Event
    {
        public Event()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }

        public string Time { get; set; }
        //public decimal Price { get; set; }
        //public int Capacity { get; set; }
        //public int BookedSeats { get; set; }
        // Navigation properties

        public Event(int id, string name, string description, DateTime startDate, DateTime endDate, string location,string time)
        {
            Id = id;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Location = location;
            Time = time;
        }

    }
}
