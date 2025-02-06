using EntityFrameworkOpgave.DAL.Models;

namespace EntityFrameworkOpgave.DAL.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public int? SamuraiId { get; set; }
        public Samurai? Samurai { get; set; }
    }
}