using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalCommon.Model
{
    public class DataResponse
    {
        public List<DataPoint> Data { get; set; }
        public DataSummary DataSummary { get; set; }
    }

    //public class DataResponseBuilder
    //{
    //    private List<DataPoint> _data = new List<DataPoint>();
    //    private DataSummary _dataSummary = new DataSummary();

    //    public DataResponseBuilder SetData(List<DataPoint> data)
    //    {
    //        _data = data;
    //        return this;
    //    }

    //    public DataResponseBuilder SetDataSummary(DataSummary dataSummary)
    //    {
    //        _dataSummary = dataSummary;
    //        return this;
    //    }

    //    public DataResponse Build()
    //    {
    //        return new DataResponse
    //        {
    //            Data = _data,
    //            DataSummary = _dataSummary
    //        };
    //    }
    //}

}
