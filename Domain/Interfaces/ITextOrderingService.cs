using Mecalux.Domain.Models;

namespace Mecalux.Domain.Interfaces
{
    public interface ITextOrderingService
    {
        List<string> OrderText(OrderTextRequest request);
    }
}