using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.Domain.Entities;
using WhoIsFaster.Domain.Interfaces.Repositories;

namespace Data.Persistance.Respositories
{
    public class RegularUserRepository : IRegularUserRepository
    {
        private readonly WhoIsFasterDbContext whoIsFasterDbContext;

        public RegularUserRepository(WhoIsFasterDbContext whoIsFasterDbContext)
        {
            this.whoIsFasterDbContext = whoIsFasterDbContext;
        }

        public async Task AddRegularUserAsync(RegularUser regularUser)
        {
            await whoIsFasterDbContext.RegularUsers.AddAsync(regularUser);
        }

        public async Task<RegularUser> GetByIdAsync(int id)
        {
            return await whoIsFasterDbContext.RegularUsers.FirstOrDefaultAsync(ru => ru.Id == id);
        }

        public async Task<RegularUser> GetByUserNameAsync(string userName)
        {
            return await whoIsFasterDbContext.RegularUsers
                .FirstOrDefaultAsync(ru => ru.UserName.Equals(userName));
        }
    }
}