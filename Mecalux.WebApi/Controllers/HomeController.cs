using Mecalux.Domain.Enums;
using Mecalux.Domain.Interfaces;
using Mecalux.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mecalux.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class HomeController(ITextOrderingService textOrderingService, ITextStatisticsService textStatisticsService) : Controller
    {
        private readonly ITextOrderingService _textOrderingService = textOrderingService;
        private readonly ITextStatisticsService _textStatisticsService = textStatisticsService;

        [HttpGet("order-options")]
        public IActionResult GetOrderOptions()
            => Ok(new List<string>
            {
                OrderOption.AlphabeticAsc.ToString(),
                OrderOption.AlphabeticDesc.ToString(),
                OrderOption.LengthAsc.ToString()
            });

        [HttpPost("ordered-text")]
        public IActionResult OrderedText([FromBody] OrderTextRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(request.TextToOrder))
                return BadRequest("Text to order cannot be null or empty.");

            var orderedWords = _textOrderingService.OrderText(request);
            return Ok(orderedWords);
        }

        [HttpPost("statistics")]
        public IActionResult GetStatistics([FromBody] string textToAnalyze)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(textToAnalyze))
                return BadRequest("Text to analyze cannot be null or empty.");

            var statistics = _textStatisticsService.GetStatistics(textToAnalyze);
            return Ok(statistics);
        }
    }
}