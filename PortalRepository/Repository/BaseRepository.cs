using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalRepository.Repository
{
    public abstract class BaseRepository<T>
    {
        protected readonly string _connectionStringOrPath;

        protected BaseRepository(string connectionStringOrPath)
        {
            _connectionStringOrPath = connectionStringOrPath;
        }
    }

}
