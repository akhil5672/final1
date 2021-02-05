using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finance_trial4.Models
{
    public class RecentTrans
    {
        public Nullable<int> customer_id { get; set; }
        public Nullable<int> product_id { get; set; }
        public Nullable<bool> Transaction_status { get; set; }
        public Nullable<DateTime> Payment_date { get; set; }
        public Nullable<decimal> Amount_paid { get; set; }
    }
}