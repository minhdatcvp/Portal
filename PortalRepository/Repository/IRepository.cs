using PortalCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalRepository.Repository
{
    public interface IRepository
    {
        List<DataPoint> GetData();
    }

}
