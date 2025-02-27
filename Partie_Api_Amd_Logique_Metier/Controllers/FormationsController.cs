﻿using System;
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
    public class FormationsController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Formations
        public IQueryable<Formation> GetFormations()
        {
            return db.Formations
                .Include(f => f.Category)
                .Include(f => f.Formateur)
               ; 
            return db.Formations
                .Include(f => f.Category)
                .Include(f => f.Formateur)
                .Include(f => f.Inscriptions)
                .Include(f => f.Media)
                .Include(f => f.Evaluations);
        }

        // GET: api/Formations/5
        [ResponseType(typeof(Formation))]
        public IHttpActionResult GetFormation(int id)
        {
            Formation formation = db.Formations
                    .Include(f => f.Category)
                    .Include(f => f.Formateur)
                    .FirstOrDefault(f => f.Id == id); if (formation == null)
            {
                return NotFound();
            }

            return Ok(formation);
        }

        // PUT: api/Formations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFormation(int id, Formation formation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != formation.Id)
            {
                return BadRequest();
            }

            db.Entry(formation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormationExists(id))
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

        // POST: api/Formations
        [ResponseType(typeof(Formation))]
        public IHttpActionResult PostFormation(Formation formation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

             Formateur formateur= db.Formateurs.FirstOrDefault(f => f.Id == formation.FormateurId);
            if (formateur == null) return NotFound();
            formation.Formateur =formateur;
            db.Formations.Add(formation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = formation.Id }, formation);
        }







        
        // DELETE: api/Formations/5
        [ResponseType(typeof(Formation))]
        public IHttpActionResult DeleteFormation(int id)
        {
            Formation formation = db.Formations.Find(id);
            if (formation == null)
            {
                return NotFound();
            }

            db.Formations.Remove(formation);
            db.SaveChanges();

            return Ok(formation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FormationExists(int id)
        {
            return db.Formations.Count(e => e.Id == id) > 0;
        }
        // GET: api/Formations/ByFormateur/3
        [HttpGet]
        [Route("api/Formations/ByFormateur/{formateurId}")]
        public IHttpActionResult GetFormationsByFormateur(int formateurId)
        {
            var formations = db.Formations
                .Include(f => f.Category)
                .Include(f => f.Formateur)
                .Where(f => f.FormateurId == formateurId)
                .ToList();

            if (!formations.Any())
            {
                return NotFound(); // Aucun résultat trouvé
            }

            return Ok(formations); // Retourne la liste des formations
        }
        [HttpGet]
        [Route("api/Formations/{id}/Media")]
        public IHttpActionResult GetFormationWithMedia(int id)
        {
            var formation = db.Formations
                .Where(f => f.Id == id)
                .Select(f => new
                {
                    f.Id,
                    f.Titre,
                    f.url_image,
                    f.Description,
                    f.Prix,
                    f.EstimationDeDuree,
                    Media = f.Media.Select(m => new
                    {
                        m.Id,
                        m.Title,
                        m.Type,
                        m.nombredesequence,
                        m.Url
                    })
                })
                .FirstOrDefault();

            if (formation == null)
            {
                return NotFound();
            }

            return Ok(formation);
        }





        [HttpGet]
        [Route("api/Formations/ByIdFormateur/{formateurId}")]
        public IQueryable<Formation> GetFormationsByListFormateur(int formateurId)
        {
             return db.Formations
                .Where(f => f.FormateurId == formateurId);

           
        }

        [Route("api/Formations/ByIdCategory/{categoryId}")]
        public IQueryable<Formation> GetFormationsByIdCategory(int categoryId)
        {
            return db.Formations
               .Where(f => f.CategoryId == categoryId);


        }











    }
}