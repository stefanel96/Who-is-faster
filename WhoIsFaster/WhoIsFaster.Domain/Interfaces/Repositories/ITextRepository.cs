using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.Domain.Entities;

namespace WhoIsFaster.Domain.Interfaces.Repositories {
    public interface ITextRepository {
        Task AddTextAsync (Text text);
        Task DeleteAsync (int id);
        Task<Text> GetByIdAsync (int id);
        Task<Text> GetRandomTextAsync ();
    }
}