using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using finance_trial4.Models;

namespace finance_trial4.Controllers
{

    public class EMIcardsController : ApiController
    {
        private financedbEntities9 db = new financedbEntities9();
        Customer tempcustomer = new Customer();
       // [Route("api/EMIinsert")]
        public IHttpActionResult PostEMIinsertion(Customer tempcustomer)
        {
            
            EMIcard temp = db.EMIcards.Where(x => x.customer_id == tempcustomer.customer_id).FirstOrDefault();
            if (temp != null)
            {
                return Ok(0);
            }
            else
            {
                System.DateTime date = DateTime.Now;
                EMIcard emicard = new EMIcard();
                var tempCustomerDetails = db.Customers.ToList();
                CustomerCard a = new CustomerCard();
                a = (from ab in tempCustomerDetails
                     join ct in db.CardTypeMasters on ab.cardtype_id equals ct.EMIcardtype_id
                     select new CustomerCard
                     {
                         customer_id = ab.customer_id,
                         total_limit = ct.total_limit,
                         EMIcard_validity = ct.EMIcard_validity
                     }
                       ).Where(x => x.customer_id == tempcustomer.customer_id).SingleOrDefault();
                emicard.customer_id = a.customer_id;
                emicard.EMIcard_expiry = date.AddYears(a.EMIcard_validity);
                emicard.used_credit = 0;
                emicard.remaining_credit = a.total_limit;
                //int num = random.Next(1000);
                //Math.Floor((Math.random() * 100) + 1);

                Random random = new Random();
                emicard.EMIcard_number = random.Next(1000000000, int.MaxValue);
                db.EMIcards.Add(emicard);
                db.SaveChanges();

            }
            return Ok(1);
        }
        // GET: api/EMIcards
        public IQueryable<EMIcard> GetEMIcards()
        {
            return db.EMIcards;
        }

        // GET: api/EMIcards/5
        [ResponseType(typeof(EMIcard))]
        public IHttpActionResult GetEMIcardbyCustomerId(int customer_id)
        {



           
                Dashboard queryres = new Dashboard();
                queryres = (from emicards in db.EMIcards
                            join customers in db.Customers on emicards.customer_id equals customers.customer_id
                            join cardtype in db.CardTypeMasters on customers.cardtype_id equals cardtype.EMIcardtype_id
                            select new Dashboard
                            {

                                customer_name=customers.customer_name,
                                EMIcardtype_name = cardtype.EMIcardtype_name,
                                EMIcard_expiry = emicards.EMIcard_expiry,
                                customer_id = customers.customer_id,
                                total_limit = cardtype.total_limit,
                                used_credit = emicards.used_credit,
                                remaining_credit = emicards.remaining_credit,
                                EMIcard_number = emicards.EMIcard_number
                            }
                    ).Where(x => x.customer_id == customer_id).FirstOrDefault();
            queryres.products = GetProductBasedOnCustomerId(customer_id);

                return Ok(queryres);
            

        }

        [NonAction]
        public List<DashBoardProduct> GetProductBasedOnCustomerId(int customer_id)
        {
            List<DashBoardProduct> products = new List<DashBoardProduct>();
            List<order> orders = db.orders.Where(x => x.customer_id == customer_id).ToList();



            for (int i = 0; i < orders.Count; i++)
            {
                int orderId = orders[i].order_id;
                if (orders[i].order_status == false)
                {
                    DashBoardProduct dashBoardProduct = new DashBoardProduct();
                    List<Transaction> transactions = db.Transactions.Where(x => x.order_id == orderId).ToList();

                    for (int j = 0; j < transactions.Count; j++)
                    {
                        var startDate = DateTime.Now;
                        var EndDate = transactions[j].Transction_date;
                        int DateDifference = Convert.ToInt32((EndDate - startDate).TotalDays);
                        if (DateDifference < 30)
                        {
                            if (transactions[j].Transaction_status == false)
                            {
                                dashBoardProduct.PaymentStatus = false;
                                break;
                            }
                            else
                            {
                                dashBoardProduct.PaymentStatus = true;
                                dashBoardProduct.PaidAmount = dashBoardProduct.PaidAmount + transactions[j].Transaction_amount;
                            }
                        }
                        else
                        {
                            dashBoardProduct.PaymentStatus = true;
                        }



                    }
                    int productId = Convert.ToInt32(orders[i].product_id);
                    productsMaster product = db.productsMasters.Where(x => x.product_id == productId).FirstOrDefault();
                    dashBoardProduct.OrderId = orders[i].order_id;
                    dashBoardProduct.BalanceAmount = product.product_price - dashBoardProduct.PaidAmount;
                    dashBoardProduct.Details = product.product_details;
                    dashBoardProduct.Cost = product.product_price;
                    dashBoardProduct.Name = product.product_name;
                    dashBoardProduct.Image = product.product_image;
                    products.Add(dashBoardProduct);
                }
                else
                {
                    continue;
                }
            }



            //products = (from order in orders
            //            join pd in db.productsMasters on order.product_id equals pd.product_id
            //            select new DashBoardProduct
            //            {
            //                Details=pd.product_details,
            //                Cost=pd.product_price,
            //                PaymentStatus=
            //            }
            //          ).ToList();



            return products;
        
    }






        // PUT: api/EMIcards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEMIcard(int id, EMIcard eMIcard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eMIcard.EMIcard_id)
            {
                return BadRequest();
            }

            db.Entry(eMIcard).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EMIcardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/EMIcards
        [ResponseType(typeof(EMIcard))]
        //public IHttpActionResult PostEMIcard(EMIcard eMIcard)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.EMIcards.Add(eMIcard);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = eMIcard.EMIcard_id }, eMIcard);
        //}

        // DELETE: api/EMIcards/5
        [ResponseType(typeof(EMIcard))]
        public IHttpActionResult DeleteEMIcard(int id)
        {
            EMIcard eMIcard = db.EMIcards.Find(id);
            if (eMIcard == null)
            {
                return NotFound();
            }

            db.EMIcards.Remove(eMIcard);
            db.SaveChanges();

            return Ok(eMIcard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EMIcardExists(int id)
        {
            return db.EMIcards.Count(e => e.EMIcard_id == id) > 0;
        }
    }
}