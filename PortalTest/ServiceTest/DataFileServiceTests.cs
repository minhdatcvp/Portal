using Microsoft.Extensions.Logging;
using Moq;
using PortalCommon.Model;
using PortalRepository.Repository;
using PortalService.Services;

namespace PortalTest.ServiceTest
{
    public class DataFileServiceTests
    {
        private readonly Mock<IRepository> _mockRepository;
        private readonly Mock<ILogger<DataFileService>> _mockLogger;
        private readonly DataFileService _service;

        public DataFileServiceTests()
        {
            _mockRepository = new Mock<IRepository>();
            _mockLogger = new Mock<ILogger<DataFileService>>();
            _service = new DataFileService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void GetData_ShouldReturnEmptyResponse_WhenNoDataAvailable()
        {
            _mockRepository.Setup(repo => repo.GetData()).Returns(new List<DataPoint>());

            var result = _service.GetData();

            Assert.NotNull(result);
            Assert.Empty(result.Data);
            Assert.NotNull(result.DataSummary);
        }

        [Fact]
        public void GetData_ShouldReturnValidSummary_WhenDataAvailable()
        {
            var testData = new List<DataPoint>
        {
            new DataPoint { Date = DateTime.Parse("2025-03-25 10:00"), Price = 100 },
            new DataPoint { Date = DateTime.Parse("2025-03-25 11:00"), Price = 200 },
            new DataPoint { Date = DateTime.Parse("2025-03-25 12:00"), Price = 150 }
        };
            _mockRepository.Setup(repo => repo.GetData()).Returns(testData);

            var result = _service.GetData();

            Assert.Equal(100, result.DataSummary.Min);
            Assert.Equal(200, result.DataSummary.Max);
            Assert.Equal(150, result.DataSummary.Average);
            Assert.NotEqual("N/A", result.DataSummary.MostExpensiveHour);
        }

        [Fact]
        public void PagingData_ShouldReturnPagedData()
        {
            var testData = new List<DataPoint>
        {
            new DataPoint { Date = DateTime.Parse("2025-03-25 10:00"), Price = 100 },
            new DataPoint { Date = DateTime.Parse("2025-03-25 11:00"), Price = 200 },
            new DataPoint { Date = DateTime.Parse("2025-03-25 12:00"), Price = 150 }
        };
            _mockRepository.Setup(repo => repo.GetData()).Returns(testData);

            var (total, pagedData) = _service.PagingData(1, 2, "Price", "asc", null);

            Assert.Equal(3, total);
            Assert.Equal(2, pagedData.Count);
            Assert.Equal(100, pagedData.First().Price);
        }
    }
}