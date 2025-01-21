using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagement.Models;

namespace HotelManagement.Controllers
{
    public class BookingController : Controller
    {
        private HotelDbContext db = new HotelDbContext();

        // GET: Booking
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.Room);
            return View(bookings.ToList());
        }

        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Booking/Create
        public ActionResult Create()
        {
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomType");
            return View();
        }


        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomId,CustomerName,CheckIn,CheckOut")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Check for overlapping bookings for the selected room
                bool isAvailable = !db.Bookings.Any(b =>
                    b.RoomId == booking.RoomId &&
                    b.CheckIn < booking.CheckOut && // Overlap condition
                    b.CheckOut > booking.CheckIn);

                if (!isAvailable)
                {
                    ModelState.AddModelError("", "The selected room is not available for the specified dates.");
                    PopulateRoomDropdown();
                    return View(booking);
                }

                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateRoomDropdown();
            return View(booking);
        }

        private void PopulateRoomDropdown()
        {
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomType");
        }


        // GET: Booking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "RoomType", booking.RoomId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,RoomId,CustomerName,CheckIn,CheckOut")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Check for overlapping bookings, excluding the current booking
                bool isAvailable = !db.Bookings.Any(b =>
                    b.RoomId == booking.RoomId &&
                    b.BookingId != booking.BookingId &&
                    b.CheckIn < booking.CheckOut &&
                    b.CheckOut > booking.CheckIn);

                if (!isAvailable)
                {
                    ModelState.AddModelError("", "The selected room is not available for the specified dates.");
                    PopulateRoomDropdown();
                    return View(booking);
                }

                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateRoomDropdown();
            return View(booking);
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Booking/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking != null)
            {
                db.Bookings.Remove(booking);
                db.SaveChanges();
            }
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

    }
}
