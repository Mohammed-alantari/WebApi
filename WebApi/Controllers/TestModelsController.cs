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
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/TestModels")]
    public class TestModelsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TestModels
        public IEnumerable<TestModel> GetTestModels()
        {
            return db.TestModels;
        }

        // GET: api/TestModels/5
        [ResponseType(typeof(TestModel))]
        public IHttpActionResult GetTestModel(Guid id)
        {
            TestModel testModel = db.TestModels.Find(id);
            if (testModel == null)
            {
                return NotFound();
            }

            return Ok(testModel);
        }

        // PUT: api/TestModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTestModel(Guid id, TestModel testModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != testModel.ID)
            {
                return BadRequest();
            }

            db.Entry(testModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(testModel.ID);
        }

        // POST: api/TestModels
        [Route("Create")]
        [ResponseType(typeof(TestModel))]
        public IHttpActionResult Create(TestModel testModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            testModel.ID = Guid.NewGuid();
            db.TestModels.Add(testModel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TestModelExists(testModel.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            var id = testModel.ID;
            return Ok(id);
        }

        // DELETE: api/TestModels/5
        [ResponseType(typeof(TestModel))]
        public IHttpActionResult DeleteTestModel(Guid id)
        {
            TestModel testModel = db.TestModels.Find(id);
            if (testModel == null)
            {
                return NotFound();
            }

            db.TestModels.Remove(testModel);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TestModelExists(Guid id)
        {
            return db.TestModels.Count(e => e.ID == id) > 0;
        }
    }
}