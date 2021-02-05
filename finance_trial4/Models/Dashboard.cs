using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finance_trial4.Models
{
    public class Dashboard
    {
        public int customer_id { get; set; }
        public int EMIcard_number { get; set; }
        public string customer_name { get; set; }
        public decimal used_credit { get; set; }
        public decimal remaining_credit { get; set; }
        public DateTime EMIcard_expiry { get; set; }
        public string EMIcardtype_name { get; set; }
        public decimal total_limit { get; set; }



        public List<DashBoardProduct> products { get; set; }
    }



    public class DashBoardProduct
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Details { get; set; }
        public int OrderId { get; set; }
        public decimal Cost { get; set; }



        public decimal PaidAmount { get; set; }



        public decimal BalanceAmount { get; set; }



        public bool PaymentStatus { get; set; }
    }




}