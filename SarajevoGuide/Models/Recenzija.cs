namespace SarajevoGuide.Models
{
    public class Recenzija
    {
        public int id { get; set; }
        public int korisnikId { get; set; }
        public int eventId { get; set; }
        public string komentar { get; set; }
        public int ocjena { get; set; }

        public Recenzija(int id, int korisnikId, int eventId, string komentar, int ocjena)
        {
            this.id = id;
            this.korisnikId = korisnikId;
            this.eventId = eventId;
            this.komentar = komentar;
            this.ocjena = ocjena;
        }
    }
}
