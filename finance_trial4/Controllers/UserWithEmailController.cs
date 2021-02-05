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
    public class UserWithEmailController : ApiController
    {
        private financedbEntities9 db = new financedbEntities9();

        // GET: api/UserWithEmail
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers;
        }

        // GET: api/UserWithEmail/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomerwithemail(string email)
        {
            LoginResponseModel loginres = new LoginResponseModel();
            Customer customer = db.Customers.Where(x => x.user_email == email).FirstOrDefault();
            if (customer == null)
            {
                loginres.StatusCode = 0;
                loginres.Message = "Invalid email";
                loginres.CustomerId = 0;
                return Ok(loginres);
            }
            else
            {
                loginres.StatusCode = 1;
                loginres.Message = "Mail sent";
                loginres.CustomerId = customer.customer_id;
                return Ok(loginres);
            }
        }

        // PUT: api/UserWithEmail/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.customer_id)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/UserWithEmail
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Passwordcredentials cred)
        {
            Customer temp = db.Customers.Where(x => x.customer_id == cred.customer_id).FirstOrDefault();
            if (temp == null)
            {
                return Ok(0);
            }
            else
            {
                temp.user_password = cred.password;
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(1);
            }
        }

        // DELETE: api/UserWithEmail/5
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.customer_id == id) > 0;
        }
    }
}