namespace EntityFrameworkOpgave.DAL.Models.DTOs
{
    public class UpdateSamuraiDto
    {
        public required string Name { get; set; }
        public int Age { get; set; }
        public UpdateHorseDto? Horse { get; set; }
        public List<int> BattleIds { get; set; } = new();
        public List<UpdateWeaponDto> Weapons { get; set; } = new();
    }

    public class UpdateHorseDto
    {
        public int? Id { get; set; }  // Optional, in case updating existing horse
        public required string Name { get; set; }
        public required string Breed { get; set; }
    }

    public class UpdateWeaponDto
    {
        public int? Id { get; set; }  // Optional, in case updating existing weapon
        public required string Name { get; set; }
        public required string Type { get; set; }
    }
}