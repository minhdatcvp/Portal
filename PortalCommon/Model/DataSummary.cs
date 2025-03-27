using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalCommon.Model
{
    public class DataSummary
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Average { get; set; }
        public string MostExpensiveHour { get; set; }
        public string BestBuyTime { get; set; }  
        public string BestSellTime { get; set; } 
    }
}
