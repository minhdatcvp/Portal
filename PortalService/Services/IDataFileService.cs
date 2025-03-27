using PortalCommon.Model;

namespace PortalService.Services
{
    public interface IDataFileService
    {
        DataResponse GetData();

        (int, List<DataPoint>) PagingData(int page, int pageSize, string sortBy, string sortOrder, string? search);
    }
}
