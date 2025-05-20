namespace SarajevoGuide.Models
{
    public class RegistrovaniKorisnik
    {
        public int id { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string email { get; set; }
        public string lozinka { get; set; }
        public string username { get; set; }
    public RegistrovaniKorisnik(int id, string ime, string prezime, string email, string lozinka, string username)
        {
            this.id = id;
            this.ime = ime;
            this.prezime = prezime;
            this.email = email;
            this.lozinka = lozinka;
            this.username = username;
        }

    }

    }
