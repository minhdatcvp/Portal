using Microsoft.Extensions.Logging;
using PortalCommon.Model;
using PortalRepository.Repository;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PortalService.Services
{
    public class DataFileService : IDataFileService
    {
        private readonly IRepository _dataRepository;
        private readonly ILogger<DataFileService> _logger;

        public DataFileService(IRepository dataRepository, ILogger<DataFileService> logger)
        {
            _dataRepository = dataRepository;
            _logger = logger;
        }

        public DataResponse GetChartData(DateTime? startDate, DateTime? endDate)
        {
            var result = new DataResponse();

            try
            {
                var data = _dataRepository.GetData();

                if (data == null || !data.Any())
                {
                    _logger.LogWarning("No data available.");
                    return new DataResponse { Data = new List<DataPoint>(), DataSummary = new DataSummary() };
                }

                // Lọc dữ liệu theo khoảng thời gian nếu có tham số
                if (startDate.HasValue)
                {
                    data = data.Where(d => d.Date >= startDate.Value).ToList();
                }
                if (endDate.HasValue)
                {
                    data = data.Where(d => d.Date < endDate.Value.AddDays(1)).ToList();
                }

                // Tính toán giá trị min, max, avg
                var minPrice = data.Min(d => d.Price);
                var maxPrice = data.Max(d => d.Price);
                var avgPrice = data.Average(d => d.Price);

                string mostExpensiveTime = "N/A";
                double maxTotalPrice = 0;

                if (data.Count > 1)
                {
                    var expensiveWindow = data.Zip(data.Skip(1), (first, second) => new
                    {
                        TimeWindow = $"{first.Date:yyyy-MM-dd HH:mm} - {second.Date:yyyy-MM-dd HH:mm}",
                        TotalPrice = first.Price + second.Price
                    }).OrderByDescending(x => x.TotalPrice).FirstOrDefault();

                    if (expensiveWindow != null)
                    {
                        mostExpensiveTime = expensiveWindow.TimeWindow;
                        maxTotalPrice = expensiveWindow.TotalPrice;
                    }
                }

                // Tìm thời điểm nên mua và nên bán
                var (bestBuyTime, bestSellTime) = GetBestBuySellTimes(data);

                result.Data = data;
                result.DataSummary = new DataSummary()
                {
                    Min = minPrice,
                    Max = maxPrice,
                    Average = avgPrice,
                    MostExpensiveHour = mostExpensiveTime,
                    BestBuyTime = bestBuyTime,
                    BestSellTime = bestSellTime

                };

                _logger.LogInformation("Successfully processed data. Min: {Min}, Max: {Max}, Avg: {Avg}, Expensive: {ExpensiveTime}",
                    minPrice, maxPrice, avgPrice, mostExpensiveTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing data.");
                throw;
            }

            return result;
        }

        public (int, List<DataPoint>) PagingData(int page, int pageSize, string sortBy, string sortOrder, string? search)
        {
            try
            {
                _logger.LogInformation("PagingData called with page={Page}, pageSize={PageSize}, sortBy={SortBy}, sortOrder={SortOrder}, search={Search}",
                                        page, pageSize, sortBy, sortOrder, search);

                var data = _dataRepository.GetData();
                if (data == null || !data.Any())
                {
                    _logger.LogWarning("No data available in repository.");
                    return (0, new List<DataPoint>());
                }

                var query = data.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    if (DateTime.TryParseExact(search, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
                    {
                        query = query.Where(d => d.Date.Date == searchDate.Date);
                    }
                    else
                    {
                        _logger.LogWarning("Invalid date format: {Search}", search);
                        throw new ArgumentException("Invalid date format. Expected format: dd-MM-yyyy");
                    }
                }


                var propertyInfo = typeof(DataPoint).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    _logger.LogWarning("Invalid sortBy parameter: {SortBy}", sortBy);
                    throw new ArgumentException($"Invalid sortBy parameter: {sortBy}");
                }

                query = sortOrder.ToLower() == "asc"
                    ? query.OrderBy(d => propertyInfo.GetValue(d, null))
                    : query.OrderByDescending(d => propertyInfo.GetValue(d, null));

                var totalRecords = query.Count();
                var pagedData = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                _logger.LogInformation("Successfully retrieved {TotalRecords} records.", totalRecords);

                return (totalRecords, pagedData);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input parameter in PagingData.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching market data.");
                throw new Exception("An unexpected error occurred while fetching data. Please try again later.");
            }
        }

        // Tìm thời điểm nên mua và nên bán
        private (string BestBuyTime, string BestSellTime) GetBestBuySellTimes(List<DataPoint> data)
        {
            if (data.Count < 2) return ("N/A", "N/A");

            double minPrice = double.MaxValue;
            string bestBuyTime = "N/A";
            string bestSellTime = "N/A";
            double maxProfit = 0;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Price < minPrice)
                {
                    minPrice = data[i].Price;
                    bestBuyTime = data[i].Date.ToString("yyyy-MM-dd HH:mm");
                }

                double profit = data[i].Price - minPrice;
                if (profit > maxProfit)
                {
                    maxProfit = profit;
                    bestSellTime = data[i].Date.ToString("yyyy-MM-dd HH:mm");
                }
            }

            if (bestBuyTime == bestSellTime) return ("N/A", "N/A");
            return (bestBuyTime, bestSellTime);
        }

    }
}
