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
using EmployeeAPI.Models;

namespace EmployeeAPI.Controllers
{
    
    [RoutePrefix("api/People")]
    public class PeopleController : ApiController
    {
        private EmployeeContext db = new EmployeeContext();

        // GET: api/People
        [HttpGet]
        [Route("GetPeopleDetails")]
        public IQueryable<Person> GetPeople()
        {
            //var person = db.People.Include(per => per.Projects).ToList();
            return db.People.OrderByDescending(x => x.Code);
        }

        // GET: api/People
        [HttpGet]
        [Route("GetPeopleDetailWithProjects")]
        public IQueryable<Person> GetPeopleWithProject()
        {
            //var person = db.People.Include(per => per.Projects).ToList();
            return db.People.Include(pr => pr.Projects).OrderByDescending(x => x.Code);
        }


        // GET: api/People/5
        [HttpGet]
        [Route("GetPeopleDetailById/{id}")]
        [ResponseType(typeof(Person))]
        public IHttpActionResult GetPerson(int id)
        {
            // Project project = new Project();
            //project = db.Projects.Find(id);

            //if (project.Code != id) {
            //    project = db.Projects.Find(id);
            //}

            var person = db.People.Include(per => per.Projects).Where(pr => pr.Code == id).ToList().OrderByDescending(x => x.Code);
            
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/People/5
        [HttpPut]
        [Route("UpdatePeopleDetails/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPerson(int id, Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.Code)
            {
                return BadRequest();
            }
            person.Projects = person.Projects;
            db.People.Add(person);
            db.Entry(person).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(person);
        }


        // POST: api/People
        [HttpPost]
        [Route("InsertPeopleDetails")]
        [ResponseType(typeof(Person))]
        public IHttpActionResult PostPerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //List<Project> project = new List<Project>();
            //project.Add(new Project { Name = "da", Description = "da"});
            //project.Add(new Project { Name = "da", Description = "da"});
            
            person.Projects = person.Projects;
            db.People.Add(person);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { controller= "PeopleController", id = person.Code }, person);
        }

        // DELETE: api/People/5
        [HttpDelete]
        [Route("DeletePeopleDetails/{id}")]
        [ResponseType(typeof(Person))]
        public IHttpActionResult DeletePerson(int id)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            db.People.Remove(person);
            db.SaveChanges();

            return Ok(person);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonExists(int id)
        {
            return db.People.Count(e => e.Code == id) > 0;
        }
    }
}