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
using Partie_Api_Amd_Logique_Metier.Models;

namespace Partie_Api_Amd_Logique_Metier.Controllers
{
    public class InscriptionsController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Inscriptions
        public IQueryable<Inscription> GetInscriptions()
        {
            return db.Inscriptions;
        }

        // GET: api/Inscriptions/5
        [ResponseType(typeof(Inscription))]
        public IHttpActionResult GetInscription(int id)
        {
            Inscription inscription = db.Inscriptions.Find(id);
            if (inscription == null)
            {
                return NotFound();
            }

            return Ok(inscription);
        }

        // PUT: api/Inscriptions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInscription(int id, Inscription inscription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inscription.Id)
            {
                return BadRequest();
            }

            db.Entry(inscription).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscriptionExists(id))
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

        // POST: api/Inscriptions
        [ResponseType(typeof(Inscription))]
        public IHttpActionResult PostInscription(Inscription inscription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Inscriptions.Add(inscription);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = inscription.Id }, inscription);
        }

        // DELETE: api/Inscriptions/5
        [ResponseType(typeof(Inscription))]
        public IHttpActionResult DeleteInscription(int id)
        {
            Inscription inscription = db.Inscriptions.Find(id);
            if (inscription == null)
            {
                return NotFound();
            }

            db.Inscriptions.Remove(inscription);
            db.SaveChanges();

            return Ok(inscription);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InscriptionExists(int id)
        {
            return db.Inscriptions.Count(e => e.Id == id) > 0;
        }
        // GET: api/Inscriptions/5
        [ResponseType(typeof(Inscription))]
        public IHttpActionResult GetInscriptionby2id(int id_formation , int participant_id)
        {
            Inscription inscription = db.Inscriptions.FirstOrDefault(i => i.FormationId == id_formation && i.ParticipaantId==participant_id);
            if (inscription == null)
            {
                return NotFound();
            }

            return Ok(inscription);
        }
    }
}