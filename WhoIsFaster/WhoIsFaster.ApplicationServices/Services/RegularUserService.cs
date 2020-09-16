using System;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;
using WhoIsFaster.ApplicationServices.Interfaces;
using WhoIsFaster.Domain.Interfaces;

namespace WhoIsFaster.ApplicationServices.Services
{
    public class RegularUserService : IRegularUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegularUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateRegularUserAsync(string userName, string firstName, string lastName)
        {
            throw new NotImplementedException();
        }

        public async Task<RegularUserDTO> GetRegularUserByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateRegularUserAsync(string userName, string firstName, string lastName)
        {
            throw new NotImplementedException();
        }
    }
}
