using Mecalux.Domain.Models;

namespace Mecalux.Domain.Interfaces
{
    public interface ITextStatisticsService
    {
        TextStatistics GetStatistics(string textToAnalyze);
    }
}