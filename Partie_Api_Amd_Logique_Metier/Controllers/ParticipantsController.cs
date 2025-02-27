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
    public class ParticipantsController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Participants
        public IQueryable<Participant> GetUsers()
        {
            return db.Participants.Include(f=>f.Role);
        }

        // GET: api/Participants/5
        [ResponseType(typeof(Participant))]
        public IHttpActionResult GetParticipant(int id)
        {
            Participant participant = db.Participants
                    .Include(p => p.Role).Include(p=>p.Inscriptions).Include(p => p.Certificates).Include(p => p.Payments)
                    .FirstOrDefault(p => p.Id == id);       
            if (participant == null)
            {
                return NotFound();
            }

            return Ok(participant);
        }

        // PUT: api/Participants/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutParticipant(int id, Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != participant.Id)
            {
                return BadRequest();
            }

            db.Entry(participant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(id))
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

        // POST: api/Participants
        [ResponseType(typeof(Participant))]
        public IHttpActionResult PostParticipant(Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            participant.Role= db.Roles.FirstOrDefault(role=>role.Id == participant.RoleId);
            db.Participants.Add(participant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = participant.Id }, participant);
        }

        // DELETE: api/Participants/5
        [ResponseType(typeof(Participant))]
        public IHttpActionResult DeleteParticipant(int id)
        {
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return NotFound();
            }

            db.Participants.Remove(participant);
            db.SaveChanges();

            return Ok(participant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParticipantExists(int id)
        {
            return db.Participants.Count(e => e.Id == id) > 0;
        }





        // GET: api/Participants by mail paasword
        [ResponseType(typeof(Participant))]
        public IHttpActionResult GetParticipantbymailparrticipant(string mail, string password )
        {
            Participant participant =  db.Participants.Include(p => p.Role).FirstOrDefault(p => p.Email == mail && p.Password == password);
            if (participant == null)
            {
                return NotFound();
            }

            return Ok(participant);
        }
    }
}