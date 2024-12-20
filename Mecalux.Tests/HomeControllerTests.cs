using Mecalux.Domain.Enums;
using Mecalux.Domain.Interfaces;
using Mecalux.Domain.Models;
using Mecalux.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Mecalux.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<ITextOrderingService> _mockTextOrderingService;
        private readonly Mock<ITextStatisticsService> _mockTextStatisticsService;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockTextOrderingService = new Mock<ITextOrderingService>();
            _mockTextStatisticsService = new Mock<ITextStatisticsService>();
            _controller = new HomeController(_mockTextOrderingService.Object, _mockTextStatisticsService.Object);
        }

        [Fact]
        public void GetOrderOptions_ReturnsOkResult_WithOrderOptions()
        {
            // Act
            var result = _controller.GetOrderOptions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orderOptions = Assert.IsType<List<string>>(okResult.Value);
            Assert.Contains(OrderOption.AlphabeticAsc.ToString(), orderOptions);
            Assert.Contains(OrderOption.AlphabeticDesc.ToString(), orderOptions);
            Assert.Contains(OrderOption.LengthAsc.ToString(), orderOptions);
        }

        [Fact]
        public void OrderedText_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("TextToOrder", "Required");

            // Act
            var result = _controller.OrderedText(new OrderTextRequest() { TextToOrder = "My text to be ordered." });

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void OrderedText_EmptyTextToOrder_ReturnsBadRequest()
        {
            // Act
            var result = _controller.OrderedText(new OrderTextRequest { TextToOrder = "" });

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Text to order cannot be null or empty.", badRequestResult.Value);
        }

        [Fact]
        public void OrderedText_ValidRequest_ReturnsOkResult_WithOrderedWords()
        {
            // Arrange
            var request = new OrderTextRequest { TextToOrder = "word1 word2", OrderOption = OrderOption.AlphabeticAsc };
            var orderedWords = new List<string> { "word1", "word2" };
            _mockTextOrderingService.Setup(service => service.OrderText(request)).Returns(orderedWords);

            // Act
            var result = _controller.OrderedText(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(orderedWords, okResult.Value);
        }

        [Fact]
        public void GetStatistics_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("textToAnalyze", "Required");

            // Act
            var result = _controller.GetStatistics("");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void GetStatistics_EmptyTextToAnalyze_ReturnsBadRequest()
        {
            // Act
            var result = _controller.GetStatistics("");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Text to analyze cannot be null or empty.", badRequestResult.Value);
        }

        [Fact]
        public void GetStatistics_ValidTextToAnalyze_ReturnsOkResult_WithStatistics()
        {
            // Arrange
            var textToAnalyze = "This is a test.";
            var statistics = new TextStatistics
            {
                WordCount = 4,
                HyphenCount = 0,
                SpaceCount = 3
            };
            _mockTextStatisticsService.Setup(service => service.GetStatistics(textToAnalyze)).Returns(statistics);

            // Act
            var result = _controller.GetStatistics(textToAnalyze);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(statistics, okResult.Value);
        }
    }
}