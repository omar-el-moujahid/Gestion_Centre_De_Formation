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
    public class AdminsController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/Admins
        public IQueryable<Admin> GetUsers()
        {
            return db.Admins.Include(a=>a.Role);
        }

        // GET: api/Admins/5
        [ResponseType(typeof(Admin))]
        public IHttpActionResult GetAdmin(int id)
        {
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return NotFound();
            }

            return Ok(admin);
        }

        // PUT: api/Admins/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAdmin(int id, Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != admin.Id)
            {
                return BadRequest();
            }

            db.Entry(admin).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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


        // POST: api/Admins
        [ResponseType(typeof(Admin))]
        public IHttpActionResult PostAdmin(Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            admin.Role=db.Roles.FirstOrDefault(role => role.Id == admin.RoleId);
            db.Admins.Add(admin);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = admin.Id }, admin);
        }





        // DELETE: api/Admins/5
        [ResponseType(typeof(Admin))]
        public IHttpActionResult DeleteAdmin(int id)
        {
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return NotFound();
            }

            db.Admins.Remove(admin);
            db.SaveChanges();

            return Ok(admin);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AdminExists(int id)
        {
            return db.Admins.Count(e => e.Id == id) > 0;
        }


        // GET: api/Admin by mail paasword
        [ResponseType(typeof(Admin))]
        public IHttpActionResult GetAdminbymailpassword(string mail, string password)
        {
            Admin admin = db.Admins.Include(p => p.Role).FirstOrDefault(p => p.Email == mail && p.Password == password);
            if (admin == null)
            {
                return NotFound();
            }

            return Ok(admin);
        }


        [ResponseType(typeof(Admin))]
        public IHttpActionResult GetAdminbymail(string mail)
        {
            Admin admin = db.Admins.Include(p => p.Role).FirstOrDefault(p => p.Email == mail);
            if (admin == null)
            {
                return NotFound();
            }

            return Ok(admin);
        }
    }
}