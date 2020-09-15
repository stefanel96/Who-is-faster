using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.Domain.Entities;
using WhoIsFaster.Domain.Interfaces.Repositories;

namespace WhoIsFaster.Infrastructure.Data.Persistance.Respositories
{
    public class TextRepository : ITextRepository
    {
        private readonly WhoIsFasterDbContext whoIsFasterDbContext;

        public TextRepository(WhoIsFasterDbContext whoIsFasterDbContext)
        {
            this.whoIsFasterDbContext = whoIsFasterDbContext;
        }

        public async Task AddTextAsync(Text text)
        {
            await whoIsFasterDbContext.Texts.AddAsync(text);
        }

        public async Task DeleteAsync(int id)
        {
            var text = await GetByIdAsync(id);
            if (text != null)
            {
                text.Delete();
            }
        }

        public async Task<Text> GetByIdAsync(int id)
        {
            return await whoIsFasterDbContext.Texts.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Text> GetRandomTextAsync()
        {
            return await whoIsFasterDbContext.Texts.OrderBy(r => Guid.NewGuid()).Take(1).FirstOrDefaultAsync();
        }
    }
}