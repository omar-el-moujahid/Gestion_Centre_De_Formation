using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Partie_Api_Amd_Logique_Metier.Models;

namespace Partie_Api_Amd_Logique_Metier.Controllers
{
    public class FormateursController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Formateurs
        public IQueryable<Formateur> GetUsers()
        {
            return db.Formateurs.Include(f=>f.Role);
        }

        // GET: api/Formateurs/5
        [ResponseType(typeof(Formateur))]
        public IHttpActionResult GetFormateur(int id)
        {
            Formateur formateur = db.Formateurs.Find(id);
            if (formateur == null)
            {
                return NotFound();
            }

            return Ok(formateur);
        }

        // PUT: api/Formateurs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFormateur(int id, Formateur formateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != formateur.Id)
            {
                return BadRequest();
            }

            db.Entry(formateur).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormateurExists(id))
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

        // POST: api/Formateurs
        [ResponseType(typeof(Formateur))]
        public IHttpActionResult PostFormateur(Formateur formateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int roleid = formateur.RoleId;

            var role = db.Roles.FirstOrDefault(r => r.Id == formateur.RoleId);
            if (role == null)
            {
                return NotFound(); 
            }
            formateur.Role = role;
            db.Formateurs.Add(formateur);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = formateur.Id }, formateur);
        }

        // DELETE: api/Formateurs/5
        [ResponseType(typeof(Formateur))]
        public IHttpActionResult DeleteFormateur(int id)
        {
            Formateur formateur = db.Formateurs.Find(id);
            if (formateur == null)
            {
                return NotFound();
            }

            db.Formateurs.Remove(formateur);
            db.SaveChanges();

            return Ok(formateur);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FormateurExists(int id)
        {
            return db.Formateurs.Count(e => e.Id == id) > 0;
        }

        // GET: api/Frmateur by mail paasword
        [ResponseType(typeof(Formateur))]
        public IHttpActionResult GetFormateurbymailparrticipant(string mail, string password)
        {
            Formateur formateur = db.Formateurs.Include(p => p.Role).FirstOrDefault(p => p.Email == mail && p.Password == password);
            if (formateur == null)
            {
                return NotFound();
            }

            return Ok(formateur);
        }



        [ResponseType(typeof(Formateur))]
        public IHttpActionResult GetAdminbymail(string mail)
        {
            Formateur formateur = db.Formateurs.Include(p => p.Role).FirstOrDefault(p => p.Email == mail);
            if (formateur == null)
            {
                return NotFound();
            }
            return Ok(formateur);
        }

        

    }
}