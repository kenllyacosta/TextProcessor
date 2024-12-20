using Mecalux.Domain.Interfaces;
using Mecalux.Domain.Models;

namespace Mecalux.WebApi.Services
{
    public class TextStatisticsService : ITextStatisticsService
    {
        public TextStatistics GetStatistics(string textToAnalyze)
        {
            return new TextStatistics
            {
                HyphenCount = textToAnalyze.Count(c => c == '-'),
                WordCount = textToAnalyze.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length,
                SpaceCount = textToAnalyze.Count(c => c == ' ')
            };
        }
    }
}