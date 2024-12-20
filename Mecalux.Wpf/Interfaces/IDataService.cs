using Mecalux.Domain.Models;

namespace Mecalux.Wpf.Interfaces
{
    public interface IDataService
    {
        Task<string> GetOrderOptions();
        Task<string[]> OrderedText(OrderTextRequest orderTextRequest);
        Task<TextStatistics> GetStatistics(string textToAnalyze);
    }
}