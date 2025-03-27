using PortalCommon.Model;

namespace PortalService.Services
{
    public interface IDataFileService
    {
        DataResponse GetChartData(DateTime? startDate, DateTime? endDate);

        (int, List<DataPoint>) PagingData(int page, int pageSize, string sortBy, string sortOrder, string? search);
    }
}
