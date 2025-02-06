namespace EntityFrameworkOpgave.DAL.Models
{
    public class Samurai
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
        public Horse? Horse { get; set; }
        public List<Battle> Battles { get; set; } = new List<Battle>();
        public List<Weapon> Weapons { get; set; } = new List<Weapon>();
    }
}