using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using finance_trial4.Models;
    
namespace finance_trial4.Controllers
{
    
    public class RegistrationStatusController : ApiController
    {
        private financedbEntities9 db = new financedbEntities9();

        public IHttpActionResult GetRegistrationStatus(int id)
        {
            Customer temp=db.Customers.Where(x => x.customer_id == id).FirstOrDefault();
            if(temp==null)
            {
                return Ok(false);
            }
            else
            {
                if(temp.isapproved==true)
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
        }

    }
}
