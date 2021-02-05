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
    public class TransactionsController : ApiController
    {
        private financedbEntities9 db = new financedbEntities9();

        // GET: api/Transactions
        public IQueryable<Transaction> GetTransactions()
        {
            return db.Transactions;
        }

        // GET: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult GetPayNowStatusByOrderId(int OrderId)
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions = db.Transactions.Where(x => x.order_id == OrderId).ToList();
            if (transactions == null)
            {
                return Ok(false);
            }
            else
            {
                Transaction trans = transactions.Where(x => x.Transaction_status == false).FirstOrDefault();
                trans.Transaction_status = true;
                trans.Payment_date = DateTime.Now;
                db.Entry(trans).State = EntityState.Modified;
                db.SaveChanges();
                order tempOrder = db.orders.Where(x => x.order_id == OrderId).FirstOrDefault();
                EMIcard emicard = db.EMIcards.Where(x => x.customer_id == tempOrder.customer_id).FirstOrDefault();
                emicard.used_credit = emicard.used_credit - trans.Transaction_amount;
                emicard.remaining_credit = emicard.remaining_credit + trans.Transaction_amount;
                db.Entry(emicard).State = EntityState.Modified;
                db.SaveChanges();



                int TransCount = transactions.Where(x => x.Transaction_status == false).ToList().Count;
                if (TransCount == 0)
                {

                    tempOrder.order_status = true;
                    db.Entry(tempOrder).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Ok(true);



            }
            //List<Transaction> transactions = new List<Transaction>();
            //transactions = db.Transactions.Where(x => x.order_id == OrderId).ToList();
            //if (transactions == null)
            //{
            //    return Ok(false);
            //}
            //else
            //{
            //    Transaction trans = transactions.Where(x => x.Transaction_status == false).FirstOrDefault();
            //    if (trans == null)
            //    {
            //        order tempOrder = db.orders.Where(x => x.order_id == OrderId).FirstOrDefault();
            //        tempOrder.order_status = true;

            //        db.Entry(tempOrder).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }
            //    else
            //    {
            //        trans.Transaction_status = true;
            //        trans.Payment_date = DateTime.Now;
            //        db.Entry(trans).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }
            //    return Ok(true);

            //}

        }

        // PUT: api/Transactions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransaction(int id, Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.Transaction_id)
            {
                return BadRequest();
            }

            db.Entry(transaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/Transactions
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transactions.Add(transaction);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = transaction.Transaction_id }, transaction);
        }

        // DELETE: api/Transactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult DeleteTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(e => e.Transaction_id == id) > 0;
        }
    }
}