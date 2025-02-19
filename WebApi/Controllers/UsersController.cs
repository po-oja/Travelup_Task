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
using WebApi.Models;

namespace WebApi.Controllers
{
    public class UsersController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IQueryable PutUser(int id, User user)
        {

            if (id != user.UserID)
            {
                return db.Users;
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return db.Users;
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IQueryable PostUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();

            return db.Users;
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IQueryable DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            //if (user == null)
            //{
            //    return "Not found";
            //}

            db.Users.Remove(user);
            db.SaveChanges();

            return db.Users;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
    }
}