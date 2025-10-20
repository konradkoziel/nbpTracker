using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using nbpTracker.Common;
using nbpTracker.Controllers;
using nbpTracker.Model.Dto;
using nbpTracker.Services;
using Xunit;

namespace nbpTracker.Tests.Controllers
{
    public class CurrencyRatesControllerTests
    {
        [Fact]
        public async Task GetTable_ReturnsOk_WhenServiceReturnsSuccessWithValue()
        {
            // Arrange
            var dto = new ExchangeTableDto
            {
                Id = 1,
                TableName = "B",
                No = "123",
                EffectiveDate = DateTime.UtcNow
            };

            var serviceMock = new Mock<ICurrencyRatesService>();
            serviceMock
                .Setup(s => s.GetTableAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ExchangeTableDto>.Ok(dto));

            var controller = new CurrencyRatesController(serviceMock.Object);

            // Act
            var result = await controller.GetTable(CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(dto, ok.Value);
        }

        [Fact]
        public async Task GetTable_ReturnsNotFound_WhenServiceReturnsSuccessButValueIsNull()
        {
            // Arrange
            var serviceMock = new Mock<ICurrencyRatesService>();
            serviceMock
                .Setup(s => s.GetTableAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ExchangeTableDto>.Ok(null));

            var controller = new CurrencyRatesController(serviceMock.Object);

            // Act
            var result = await controller.GetTable(CancellationToken.None);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No table found.", notFound.Value);
        }

        [Fact]
        public async Task GetTable_Returns500_WhenServiceReturnsFailure()
        {
            // Arrange
            var serviceMock = new Mock<ICurrencyRatesService>();
            serviceMock
                .Setup(s => s.GetTableAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ExchangeTableDto>.Fail("some error"));

            var controller = new CurrencyRatesController(serviceMock.Object);

            // Act
            var result = await controller.GetTable(CancellationToken.None);

            // Assert
            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, obj.StatusCode);
            Assert.Equal("some error", obj.Value);
        }
    }
}