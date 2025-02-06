using EntityFrameworkOpgave.DAL.Models;

namespace EntityFrameworkOpgave.DAL.Models
{
    public class Battle
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime Date { get; set; }
        public required string Location { get; set; }
        public List<Samurai> Samurais { get; set; } = new List<Samurai>();
    }
}