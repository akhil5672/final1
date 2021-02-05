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
    public class orders1Controller : ApiController
    {
        private financedbEntities9 db = new financedbEntities9();

        // GET: api/orders1
        public IQueryable<order> Getorders()
        {
            return db.orders;
        }

        // GET: api/orders1/5
        [ResponseType(typeof(order))]
        //public IHttpActionResult GetOrdersByCustomerId(int customer_id)
        //{
        //    var orders = db.orders.Where(x => x.customer_id == customer_id &&x.order_status==false).ToList();
        //    return Ok(orders);
        //}
        //public IHttpActionResult Getproductsbycustomerid(int id)
        //{
        //    order order = db.orders.Find(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(order);
        //}

        // PUT: api/orders1/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putorder(int id, order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.order_id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!orderExists(id))
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

        // POST: api/orders1
        [ResponseType(typeof(order))]
        //public IHttpActionResult Postorder(order order)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.orders.Add(order);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = order.order_id }, order);
        //}
        public IHttpActionResult Postbuynowcredentials(buynowcredentials buynowcred)
        {
            var orders = db.orders.Where(x => x.customer_id == buynowcred.customer_id && x.order_status == false).ToList();
            foreach(var v in orders)
            {
                if(v.product_id==buynowcred.product_id)
                {
                    return Ok(0);
                }
            }
            return Ok(1);
        }
        // DELETE: api/orders1/5
        [ResponseType(typeof(order))]
        public IHttpActionResult Deleteorder(int id)
        {
            order order = db.orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool orderExists(int id)
        {
            return db.orders.Count(e => e.order_id == id) > 0;
        }
    }
}