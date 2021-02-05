using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finance_trial4.Models
{
    public class RecentTransactionsResponse
    {
        public RecentTrans recent { get; set; }
        public productsMaster product { get; set; }
    }
}