using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.Domain.Entities;

namespace WhoIsFaster.Domain.Interfaces.Repositories {
    public interface IRegularUserRepository {
        Task AddRegularUserAsync (RegularUser regularUser);
        Task<RegularUser> GetByIdAsync (int id);
        Task<RegularUser> GetByUserNameAsync (string userName);
        Task<RegularUser> SecureGetByUserNameAsync(string userName);
        Task DeleteRegularUserAsync(string userName);
    }
}