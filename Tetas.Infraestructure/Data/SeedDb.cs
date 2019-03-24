namespace Tetas.Infraestructure.Data
{
    //using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities;

    public class SeedDb
    {
        private readonly ApplicationDbContext _context;
        //private readonly Random random;

        public SeedDb(ApplicationDbContext context)
        {
            _context = context;
            //random = new Random();
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
                        
            if (!_context.GroupTypes.Any())
            {
                AddGroupType("_General Purpose", "");
                AddGroupType("University", "");
                AddGroupType("Class Mates", "");
                AddGroupType("Polytics", "");
                await _context.SaveChangesAsync();
            }

            if (!_context.Privacies.Any())
            {
                AddPrivacy("Public");
                AddPrivacy("Contacs");
                AddPrivacy("Private (Just Me)");
                await _context.SaveChangesAsync();
            }
        }

        private void AddGroupType(string name,string description)
        {
            _context.GroupTypes.Add(new GroupType
            {
                Name = name,
                Description=description              
            });
        }

        private void AddPrivacy(string name)
        {
            _context.Privacies.Add(new Privacy
            {
                Name = name
            });
        }
    }
}
 
