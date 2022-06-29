using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerfectGateway.Models
{
    public class TransactionModel
    {
        public string clientCode { get; set; }
        public string clientAccount { get; set; }
        public string creditAccount { get; set; }
        public string operationRef { get; set; }
        public int status { get; set; }
        public int amount { get; set; }
        public int note_10000 { get; set; }
        public int note_5000 { get; set; }
        public int note_2000 { get; set; }
        public int note_1000 { get; set; }
        public string kioskId { get; set; }
        public int operationType { get; set; }
        public string serviceId { get; set; }
        public string description { get; set; }

    }
}
