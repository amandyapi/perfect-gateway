using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerfectGateway.CustomModels
{
    public class KiosksUpdateModel
    {
        public string ClientId { get; set; }
        public int Note_10000 { get; set; }
        public int Note_5000 { get; set; }
        public int Note_2000 { get; set; }
        public int Note_1000 { get; set; }
        public int UpdateType { get; set; }
    }
}