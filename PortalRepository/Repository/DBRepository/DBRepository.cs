using Microsoft.Extensions.Configuration;
using PortalCommon;
using PortalCommon.Model;
using System.Globalization;

namespace PortalRepository.Repository.DBRepository
{
    public class DBRepository : BaseRepository<DataPoint>, IRepository
    {
        public DBRepository() :base(AppSettings.DBConnection)
        {
        }

        public List<DataPoint> GetData()
        {
            // using connectionStringOrPath to connect DB 
            return new List<DataPoint>();
        }
    }
}
