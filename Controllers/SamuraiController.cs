using Microsoft.AspNetCore.Mvc;
using EntityFrameworkOpgave.DAL.Models;
using EntityFrameworkOpgave.DAL.Models.DTOs;
using EntityFrameworkOpgave.DAL.Repositories;
using EntityFrameworkOpgave.DAL.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkOpgave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SamuraiController : ControllerBase
    {
        private readonly IRepository<Samurai> _samuraiRepository;
        private readonly SamuraiDbContext _context;
        private readonly ILogger<SamuraiController> _logger;

        public SamuraiController(IRepository<Samurai> samuraiRepository, SamuraiDbContext context, ILogger<SamuraiController> logger)
        {
            _samuraiRepository = samuraiRepository;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Samurai>> GetAll()
        {
            try
            {
                var samurais = _samuraiRepository.GetAll();
                return Ok(samurais);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all samurais");
                return StatusCode(500, "An error occurred while retrieving the data. Error: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Samurai> GetById(int id)
        {
            try
            {
                return Ok(_samuraiRepository.GetById(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting samurai with id {id}");
                return StatusCode(500, "An error occurred while retrieving the data. Error: " + ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<Samurai> Create(CreateSamuraiDto dto)
        {
            try
            {
                var samurai = new Samurai
                {
                    Name = dto.Name,
                    Age = dto.Age
                };

                // First save the samurai to get its ID
                _samuraiRepository.Add(samurai);

                // Add horse if provided
                if (dto.Horse != null)
                {
                    samurai.Horse = new Horse
                    {
                        Name = dto.Horse.Name,
                        Breed = dto.Horse.Breed,
                        SamuraiId = samurai.Id
                    };
                }

                // Add weapons
                foreach (var weaponDto in dto.Weapons)
                {
                    samurai.Weapons.Add(new Weapon
                    {
                        Name = weaponDto.Name,
                        Type = weaponDto.Type,
                        SamuraiId = samurai.Id
                    });
                }

                // Add battle references
                var battles = _context.Battles.Where(b => dto.BattleIds.Contains(b.Id)).ToList();
                foreach (var battle in battles)
                {
                    samurai.Battles.Add(battle);
                }

                // Update the samurai with its relationships
                _samuraiRepository.Update(samurai);

                return CreatedAtAction(nameof(GetById), new { id = samurai.Id }, samurai);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating samurai");
                return StatusCode(500, "An error occurred while creating the samurai. Error: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateSamuraiDto dto)
        {
            try
            {
                // Get existing samurai with all relationships
                var samurai = _context.Samurais
                    .Include(s => s.Horse)
                    .Include(s => s.Weapons)
                    .Include(s => s.Battles)
                    .FirstOrDefault(s => s.Id == id);

                if (samurai == null)
                    return NotFound();

                // Update basic properties
                samurai.Name = dto.Name;
                samurai.Age = dto.Age;

                // Update or create horse
                if (dto.Horse != null)
                {
                    if (samurai.Horse != null)
                    {
                        // Update existing horse
                        samurai.Horse.Name = dto.Horse.Name;
                        samurai.Horse.Breed = dto.Horse.Breed;
                    }
                    else
                    {
                        // Create new horse
                        samurai.Horse = new Horse
                        {
                            Name = dto.Horse.Name,
                            Breed = dto.Horse.Breed,
                            SamuraiId = samurai.Id
                        };
                    }
                }
                else if (samurai.Horse != null)
                {
                    // Remove horse if not included in update
                    _context.Horses.Remove(samurai.Horse);
                }

                // Update weapons
                var existingWeaponIds = samurai.Weapons.Select(w => w.Id).ToList();
                var updatedWeaponIds = dto.Weapons.Where(w => w.Id.HasValue).Select(w => w.Id.Value).ToList();

                // Remove weapons not in the update
                var weaponsToRemove = samurai.Weapons.Where(w => !updatedWeaponIds.Contains(w.Id)).ToList();
                foreach (var weapon in weaponsToRemove)
                {
                    samurai.Weapons.Remove(weapon);
                }

                // Update existing and add new weapons
                foreach (var weaponDto in dto.Weapons)
                {
                    if (weaponDto.Id.HasValue)
                    {
                        // Update existing weapon
                        var weapon = samurai.Weapons.FirstOrDefault(w => w.Id == weaponDto.Id.Value);
                        if (weapon != null)
                        {
                            weapon.Name = weaponDto.Name;
                            weapon.Type = weaponDto.Type;
                        }
                    }
                    else
                    {
                        // Add new weapon
                        samurai.Weapons.Add(new Weapon
                        {
                            Name = weaponDto.Name,
                            Type = weaponDto.Type,
                            SamuraiId = samurai.Id
                        });
                    }
                }

                // Update battles
                samurai.Battles.Clear();
                var battles = _context.Battles.Where(b => dto.BattleIds.Contains(b.Id)).ToList();
                foreach (var battle in battles)
                {
                    samurai.Battles.Add(battle);
                }

                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating samurai {id}");
                return StatusCode(500, "An error occurred while updating the samurai. Error: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                // Get samurai with all relationships
                var samurai = _context.Samurais
                    .Include(s => s.Horse)
                    .Include(s => s.Weapons)
                    .Include(s => s.Battles)
                    .FirstOrDefault(s => s.Id == id);

                if (samurai == null)
                    return NotFound();

                // Remove horse if exists
                if (samurai.Horse != null)
                {
                    _context.Horses.Remove(samurai.Horse);
                }

                // Remove all weapons
                foreach (var weapon in samurai.Weapons.ToList())
                {
                    _context.Weapons.Remove(weapon);
                }

                // Clear battle relationships
                samurai.Battles.Clear();

                // Remove the samurai
                _context.Samurais.Remove(samurai);

                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting samurai {id}");
                return StatusCode(500, "An error occurred while deleting the samurai. Error: " + ex.Message);
            }
        }
    }
}