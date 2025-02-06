namespace EntityFrameworkOpgave.DAL.Models.DTOs
{
    public class CreateSamuraiDto
    {
        public required string Name { get; set; }
        public int Age { get; set; }
        public CreateHorseDto? Horse { get; set; }
        public List<int> BattleIds { get; set; } = new();
        public List<CreateWeaponDto> Weapons { get; set; } = new();
    }

    public class CreateHorseDto
    {
        public required string Name { get; set; }
        public required string Breed { get; set; }
    }

    public class CreateWeaponDto
    {
        public required string Name { get; set; }
        public required string Type { get; set; }
    }
}