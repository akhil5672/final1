using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finance_trial4.Models
{
    public class DashboardResponse
    {
        public bool status { get; set; }
        public Dashboard dashboard { get; set; }
    }
}