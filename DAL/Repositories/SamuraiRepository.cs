using Microsoft.EntityFrameworkCore;
using EntityFrameworkOpgave.DAL.Models;
using EntityFrameworkOpgave.DAL.Data;

namespace EntityFrameworkOpgave.DAL.Repositories
{
    public class SamuraiRepository : IRepository<Samurai>
    {
        private readonly SamuraiDbContext _context;

        public SamuraiRepository(SamuraiDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Samurai> GetAll()
        {
            return _context.Samurais
                .Include(s => s.Horse)
                .Include(s => s.Weapons)
                .Select(s => new Samurai
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    Horse = s.Horse,
                    Weapons = s.Weapons,
                    Battles = s.Battles.Select(b => new Battle
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Date = b.Date,
                        Location = b.Location,
                        Samurais = b.Samurais.Select(bs => new Samurai
                        {
                            Id = bs.Id,
                            Name = bs.Name
                        }).ToList()
                    }).ToList()
                })
                .ToList();
        }

        public Samurai GetById(int id)
        {
            var samurai = _context.Samurais
                .Include(s => s.Horse)
                .Include(s => s.Weapons)
                .Include(s => s.Battles)
                .FirstOrDefault(s => s.Id == id);

            if (samurai == null)
                throw new KeyNotFoundException($"Samurai with ID {id} was not found.");

            return samurai;
        }

        public void Add(Samurai entity)
        {
            _context.Samurais.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Samurai entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var samurai = _context.Samurais.Find(id);
            if (samurai != null)
            {
                _context.Samurais.Remove(samurai);
                _context.SaveChanges();
            }
        }
    }
}