using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.ApplicationServices.DTOs;

namespace WhoIsFaster.ApplicationServices.Interfaces
{
    public interface ITextService
    {
        Task<List<TextDTO>> GetAllTextsAsync();
        Task DeleteTextAsync(int id);
        Task CreateTextAsync(string source, string textContent);
    }
}
