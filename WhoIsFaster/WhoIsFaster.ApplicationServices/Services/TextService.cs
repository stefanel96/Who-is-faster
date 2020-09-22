using System.Collections.Generic;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;
using WhoIsFaster.ApplicationServices.Interfaces;
using WhoIsFaster.Domain.Interfaces;
using WhoIsFaster.Domain.Entities;
using System.Linq;
using WhoIsFaster.ApplicationServices.Exceptions;

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
            if (text == null)
            {
                throw new WhoIsFasterException($"Text with id: {id} - doesn't exist.");
            }
            text.Delete();
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<TextDTO>> GetAllTextsAsync()
        {

            List<Text> texts = await _unitOfWork.TextRepository.GetAllTexts();
            if (texts == null)
            {
                throw new WhoIsFasterException($"Texts couldn't be gathered from the database.");
            }
            return texts.ToTextDTOs().ToList();
        }
    }
}

