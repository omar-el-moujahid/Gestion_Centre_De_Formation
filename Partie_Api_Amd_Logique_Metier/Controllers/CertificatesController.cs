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
    public class CertificatesController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Certificates
        public IQueryable<Certificate> GetCertificates()
        {
            return db.Certificates;
        }

        // GET: api/Certificates/5
        [ResponseType(typeof(Certificate))]
        public IHttpActionResult GetCertificate(int id)
        {
            Certificate certificate = db.Certificates.Find(id);
            if (certificate == null)
            {
                return NotFound();
            }

            return Ok(certificate);
        }

        // PUT: api/Certificates/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCertificate(int id, Certificate certificate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != certificate.Id)
            {
                return BadRequest();
            }

            db.Entry(certificate).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificateExists(id))
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

        // POST: api/Certificates
        [ResponseType(typeof(Certificate))]
        public IHttpActionResult PostCertificate(Certificate certificate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Certificates.Add(certificate);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = certificate.Id }, certificate);
        }

        // DELETE: api/Certificates/5
        [ResponseType(typeof(Certificate))]
        public IHttpActionResult DeleteCertificate(int id)
        {
            Certificate certificate = db.Certificates.Find(id);
            if (certificate == null)
            {
                return NotFound();
            }

            db.Certificates.Remove(certificate);
            db.SaveChanges();

            return Ok(certificate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CertificateExists(int id)
        {
            return db.Certificates.Count(e => e.Id == id) > 0;
        }
    }
}