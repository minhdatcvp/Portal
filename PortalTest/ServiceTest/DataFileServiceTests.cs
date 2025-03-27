using Microsoft.Extensions.Logging;
using Moq;
using PortalCommon.Model;
using PortalRepository.Repository;
using PortalService.Services;
using System.Reflection;

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
        public void GetChartData_ReturnsEmptyData_WhenNoDataAvailable()
        {
            _mockRepository.Setup(repo => repo.GetData()).Returns(new List<DataPoint>());

            var result = _service.GetChartData(null, null);

            Assert.NotNull(result);
            Assert.Empty(result.Data);
            Assert.NotNull(result.DataSummary);
        }

        [Fact]
        public void GetChartData_CalculatesCorrectSummary()
        {
            var data = new List<DataPoint>
        {
            new DataPoint { Date = DateTime.Parse("2025-03-25 10:00"), Price = 100 },
            new DataPoint { Date = DateTime.Parse("2025-03-25 11:00"), Price = 200 },
            new DataPoint { Date = DateTime.Parse("2025-03-25 12:00"), Price = 150 }
        };

            _mockRepository.Setup(repo => repo.GetData()).Returns(data);

            var result = _service.GetChartData(null, null);

            Assert.Equal(100, result.DataSummary.Min);
            Assert.Equal(200, result.DataSummary.Max);
            Assert.Equal(150, result.DataSummary.Average);
            Assert.NotNull(result.DataSummary.BestBuyTime);
            Assert.NotNull(result.DataSummary.BestSellTime);
        }

        [Fact]
        public void PagingData_ReturnsPagedResults()
        {
            var data = Enumerable.Range(1, 10).Select(i => new DataPoint
            {
                Date = DateTime.Parse("2025-03-25").AddHours(i),
                Price = i * 10
            }).ToList();

            _mockRepository.Setup(repo => repo.GetData()).Returns(data);

            var (total, pagedData) = _service.PagingData(2, 3, "Price", "asc", null);

            Assert.Equal(10, total);
            Assert.Equal(3, pagedData.Count);
            Assert.Equal(40, pagedData.First().Price);
        }

        [Fact]
        public void PagingData_ThrowsException_WhenInvalidSortBy()
        {
            var data = new List<DataPoint> { new DataPoint { Date = DateTime.Now, Price = 100 } };
            _mockRepository.Setup(repo => repo.GetData()).Returns(data);

            Assert.Throws<ArgumentException>(() => _service.PagingData(1, 10, "InvalidProperty", "asc", null));
        }

        [Fact]
        public void GetBestBuySellTimes_ReturnsCorrectTimes()
        {
            var data = new List<DataPoint>
        {
            new DataPoint { Date = DateTime.Parse("2025-03-25 10:00"), Price = 100 },
            new DataPoint { Date = DateTime.Parse("2025-03-25 11:00"), Price = 50 },
            new DataPoint { Date = DateTime.Parse("2025-03-25 12:00"), Price = 200 }
        };

            var result = _service.GetType().GetMethod("GetBestBuySellTimes", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(_service, new object[] { data });

            var (bestBuy, bestSell) = ((string, string))result;

            Assert.Equal("2025-03-25 11:00", bestBuy);
            Assert.Equal("2025-03-25 12:00", bestSell);
        }
    }
}