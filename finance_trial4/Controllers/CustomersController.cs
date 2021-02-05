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
    public class CustomersController : ApiController
    {
        private financedbEntities9 db = new financedbEntities9();

        // GET: api/Customers
        public IHttpActionResult  GetCustomers()
        { 
            return Ok(db.Customers.ToList());
        }
 
        // GET: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.Where(x=>x.customer_id==id).FirstOrDefault();
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, Customer customerobject)
        {
            if (!ModelState.IsValid)
            {
                return Ok(0);
            }

            if (id != customerobject.customer_id)
            {
                return Ok(0);
            }

            

            try
            {
                Customer temp = db.Customers.SingleOrDefault(x => x.customer_id == customerobject.customer_id);
                temp.isapproved = customerobject.isapproved;
                //using (financedbEntities9 context = new financedbEntities9())
                //{
                    
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                //}
               
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return Ok(0);
                }
                else
                {
                    throw;
                }
            }

            return Ok(1);
        }

        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach(Customer c in db.Customers)
            {
                if (c.user_name == customer.user_name)
                    return Ok(-1);
                if (c.user_email == customer.user_email)
                    return Ok(0);
                if (c.phone_number == customer.phone_number)
                    return Ok(-2);
            }



            db.Customers.Add(customer);
            db.SaveChanges();

            return Ok(1);
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }


        //[Route("customers/userlogin")]
        //public IHttpActionResult PostUserLogin(Logincredentials credentials)
        //{
        //    Customer temp= db.Customers.Where(x => x.user_name == credentials.user_name && x.user_password == credentials.user_password).FirstOrDefault();
        //    if (temp == null)
        //    {
        //        return Ok("Invalid username or password");
        //    }

        //    return Ok("Login successful");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.customer_id == id) > 0;
        }
    }
}