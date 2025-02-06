using EntityFrameworkOpgave.DAL.Models;

namespace EntityFrameworkOpgave.DAL.Models
{
    public class Horse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Breed { get; set; }

        // Foreign key for one-to-one relationship
        public required int SamuraiId { get; set; }
        public Samurai? Samurai { get; set; }
    }
}