using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContactWeb.Models;
using Microsoft.AspNet.Identity;

namespace ContactWeb.Controllers
{
    public class ContactsController : Controller
    {
        private ContactWebContext db = new ContactWebContext();

        // GET: Contacts
        [Authorize] //Attribute
        public ActionResult Index()
        {

            var userId = GetCurrentUserId();


            /*
            try //This is just for checking or getting the userid and username
            {
                var userId = new Guid(User.Identity.GetUserId());
                var userName = User.Identity.GetUserName();

                //TWO WAYS:

                //One Way:
                ViewBag.UserId = userId;
                ViewBag.UserName = userName;

                //Other Way:
                ViewData["User Id"] = userId;
                ViewData["UserName"] = userName;

            }
            catch (System.Exception ex) { }
            */

            return View(db.Contacts.Where(x => x.UserId == userId).ToList());
        }

        // GET: Contacts/Details/5
        [Authorize] //Attribute
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null || !EnsureIsUserContact(contact))
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        [Authorize] //Attribute
        public ActionResult Create()
        {
            ViewBag.UserId = GetCurrentUserId();
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] //Attribute
        public ActionResult Create([Bind(Include = "Id,UserId,FirstName,LastName,Email,PhonePrimary,PhoneSecondary,Birthday,StreetAddress1,StreetAddress2,City,State,Zip")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = GetCurrentUserId(); //In case the contact object is invalid and user hits submit then we save the reference
            return View(contact);
        }

        // GET: Contacts/Edit/5
        [Authorize] //Attribute
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null || !EnsureIsUserContact(contact))
            {
                return HttpNotFound();
            }
            ViewBag.UserId = GetCurrentUserId(); //In case the contact object is invalid and user hits submit then we save the reference
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] //Attribute
        public ActionResult Edit([Bind(Include = "Id,UserId,FirstName,LastName,Email,PhonePrimary,PhoneSecondary,Birthday,StreetAddress1,StreetAddress2,City,State,Zip")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        [Authorize] //Attribute
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null || !EnsureIsUserContact(contact))
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [Authorize] //Attribute
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            if (!EnsureIsUserContact(contact))
            {
                return HttpNotFound();
            }
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public Guid GetCurrentUserId()
        {
            return new Guid(User.Identity.GetUserId());
        }

        private bool EnsureIsUserContact(Contact contact)
        {
            return contact.UserId == GetCurrentUserId();
        }
    }
}
