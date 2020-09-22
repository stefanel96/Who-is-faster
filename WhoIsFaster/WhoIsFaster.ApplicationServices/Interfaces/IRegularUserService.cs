using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;

namespace WhoIsFaster.ApplicationServices.Interfaces
{
    public interface IRegularUserService
    {
        Task<RegularUserDTO> GetRegularUserByUserNameAsync(string userName);
        Task UpdateRegularUserAsync(string userName, string firstName, string lastName);
        Task CreateRegularUserAsync(string userName, string firstName, string lastName);

        Task DeleteRegularUserAsync(string userName);
    }
}
