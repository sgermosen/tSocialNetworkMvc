namespace Tetas.Infraestructure.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Tetas.Domain.Entities;

    public class SeedDb
    {
        private readonly ApplicationDbContext context;
        private readonly Random random;

        public SeedDb(ApplicationDbContext context)
        {
            this.context = context;
            this.random = new Random();
        }
        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();
                        
            if (!this.context.GroupTypes.Any())
            {
                this.AddGroupType("_General Purpose", "");
                this.AddGroupType("University", "");
                this.AddGroupType("Class Mates", "");
                this.AddGroupType("Polytics", "");
                await this.context.SaveChangesAsync();
            }

            if (!this.context.Privacies.Any())
            {
                this.AddPrivacy("Public");
                this.AddPrivacy("Contacs");
                this.AddPrivacy("Private (Just Me)");
                await this.context.SaveChangesAsync();
            }
        }

        private void AddGroupType(string name,string description)
        {
            this.context.GroupTypes.Add(new GroupType
            {
                Name = name,
                Description=description              
            });
        }

        private void AddPrivacy(string name)
        {
            this.context.Privacies.Add(new Privacy
            {
                Name = name
            });
        }
    }
}
 
