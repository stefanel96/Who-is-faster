using System.Collections.Generic;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;
using WhoIsFaster.ApplicationServices.Interfaces;
using WhoIsFaster.Domain.Interfaces;
using WhoIsFaster.Domain.Entities;
using System.Linq;

namespace WhoIsFaster.ApplicationServices.Services
{
    public class TextService : ITextService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TextService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateTextAsync(string source, string textContent)
        {
            var Text = new Text(source, textContent);
            await _unitOfWork.TextRepository.AddTextAsync(Text);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteTextAsync(int id)
        {
            Text text = await _unitOfWork.TextRepository.GetByIdAsync(id);
            text.Delete();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TextDTO>> GetAllTextsAsync()
        {
            return (await _unitOfWork.TextRepository.GetAllTexts()).ToTextDTOs().ToList();
        }
    }
}

