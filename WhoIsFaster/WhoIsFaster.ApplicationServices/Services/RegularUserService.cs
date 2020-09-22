using System;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;
using WhoIsFaster.ApplicationServices.Exceptions;
using WhoIsFaster.ApplicationServices.Interfaces;
using WhoIsFaster.Domain.Entities;
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
            await _unitOfWork.RegularUserRepository.AddRegularUserAsync(new RegularUser(firstName, lastName, userName));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRegularUserAsync(string userName)
        {
            await _unitOfWork.RegularUserRepository.DeleteRegularUserAsync(userName);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<RegularUserDTO> GetRegularUserByUserNameAsync(string userName)
        {
            var regularUser = await _unitOfWork.RegularUserRepository.GetByUserNameAsync(userName);
            if (regularUser == null)
            {
                throw new WhoIsFasterException($"User with given username {userName} doesn't exist.");
            }
            return new RegularUserDTO(regularUser);

        }

        public async Task UpdateRegularUserAsync(string userName, string firstName, string lastName)
        {
            var regularUser = await _unitOfWork.RegularUserRepository.GetByUserNameAsync(userName);
            if (regularUser == null)
            {
                throw new WhoIsFasterException($"User with given username {userName} doesn't exist.");
            }
            regularUser.UpdateFirstName(firstName);
            regularUser.UpdateLastName(lastName);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}