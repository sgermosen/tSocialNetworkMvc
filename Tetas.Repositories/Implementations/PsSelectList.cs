using Tetas.Domain.Entities;

namespace Tetas.Repositories.Implementations
{
    using Contracts;
    using Domain.Helpers;
    using Infraestructure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PsSelectList : IPsSelectList 
    {
        private readonly ApplicationDbContext _context;

        public PsSelectList(ApplicationDbContext context)
        {
            _context = context;
        }

        //public long Id { get; set; }
        //public string Name { get; set; }
        //public bool Deleted { get; set; }

        public async Task<List<Privacy>> GetListPrivacies()
        {
            var privacies = new List<Privacy>
            {
                new Privacy
                {
                    Id = 0,
                    Name = "Choose One Privacy Level"
                }
            };
            //privacies.AddRange(_privacyRepo.GetAll().ToList());
            privacies.AddRange(await _context.Privacies.ToListAsync());

            return privacies;
        }

        public async Task<List<GroupType>> GetListGroupTypes()
        {
            var groupTypes = new List<GroupType>
            {
                new GroupType
                {
                    Id = 0,
                    Name = "Choose One Group Type"
                }
            };
            //privacies.AddRange(_privacyRepo.GetAll().ToList());
            groupTypes.AddRange(await _context.GroupTypes.ToListAsync());

            return groupTypes;
        }

        public async Task<Privacy> GetPrivacyAsync(long id)
        {

            return await _context.Privacies.FindAsync(id);
                 
        }

        public async Task<GroupType> GetGroupTypeAsync(long id)
        {

            return await _context.GroupTypes.FindAsync(id);

        }
    }
}
