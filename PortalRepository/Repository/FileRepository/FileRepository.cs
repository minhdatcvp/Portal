using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PortalCommon;
using PortalCommon.Model;
using PortalRepository.Repository.DBRepository;
using System.Globalization;

namespace PortalRepository.Repository.FileRepository
{
    public class FileRepository : BaseRepository<DataPoint>, IRepository
    {
        private readonly ILogger<FileRepository> _logger;

        public FileRepository(ILogger<FileRepository> logger) : base(AppSettings.FilePath)
        {
            _logger = logger;
        }

        public List<DataPoint> GetData()
        {
            try
            {
                _logger.LogInformation("Attempting to read data from file: {FilePath}", _connectionStringOrPath);

                if (!File.Exists(_connectionStringOrPath))
                {
                    _logger.LogError("File not found: {FilePath}", _connectionStringOrPath);
                    throw new FileNotFoundException($"Data file not found at {_connectionStringOrPath}");
                }

                var data = File.ReadAllLines(_connectionStringOrPath)
                    .Skip(1)
                    .Select(line => line.Split(','))
                    .Select(parts => new DataPoint
                    {
                        Date = DateTime.ParseExact(parts[0], new[] { "dd/MM/yyyy HH:mm", "dd/MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None),
                        Price = double.Parse(parts[1])
                    })
                    .ToList();

                _logger.LogInformation("Successfully read {Count} data points from file.", data.Count);

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while reading data from file: {FilePath}", _connectionStringOrPath);
                throw;
            }

        }
    }
}
