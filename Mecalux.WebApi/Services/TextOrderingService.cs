using Mecalux.Domain.Enums;
using Mecalux.Domain.Interfaces;
using Mecalux.Domain.Models;

namespace Mecalux.WebApi.Services
{
    public class TextOrderingService : ITextOrderingService
    {
        public List<string> OrderText(OrderTextRequest request)
        {
            var words = request.TextToOrder.Split(' ').ToList();

            switch (request.OrderOption)
            {
                case OrderOption.AlphabeticAsc:
                    words = [.. words.OrderBy(w => w.Trim())];
                    break;
                case OrderOption.AlphabeticDesc:
                    words = [.. words.OrderByDescending(w => w.Trim())];
                    break;
                case OrderOption.LengthAsc:
                    words = [.. words.OrderBy(w => w.Length)];
                    break;
            }

            return words;
        }
    }
}