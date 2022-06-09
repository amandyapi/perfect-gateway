using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerfectGateway.CustomModels
{
    public class KiosksModel
    {
        public string Id { get; set; }
        public string SkId { get; set; }
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Note_10000 { get; set; }
        public int Note_5000 { get; set; }
        public int Note_2000 { get; set; }
        public int Note_1000 { get; set; }
    }
}