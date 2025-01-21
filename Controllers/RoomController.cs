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
    public class RoomController : Controller
    {
        private HotelDbContext db = new HotelDbContext();

        // GET: Room
        public ActionResult Index()
        {
            ViewBag.Hotels = new SelectList(db.Hotels, "HotelId", "Name");
            return View(db.Rooms.ToList());
        }

        // GET: Room/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Room/Create
        public ActionResult Create()
        {
            ViewBag.HotelId = new SelectList(db.Hotels, "HotelId", "Name");
            return View();
        }

        // POST: Room/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomId,HotelId,RoomType,Price,IsAvailable")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelId = new SelectList(db.Hotels, "HotelId", "Name", room.HotelId);
            return View(room);
        }

        // GET: Room/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "HotelId", "Name", room.HotelId);
            return View(room);
        }

        // POST: Room/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomId,HotelId,RoomType,Price,IsAvailable")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "HotelId", "Name", room.HotelId);
            return View(room);
        }

        // GET: Room/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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
        public ActionResult FilterByHotel(int? hotelId)
        {
            if (hotelId == null)
            {
                return RedirectToAction("Index");
            }

            var rooms = db.Rooms.Where(r => r.HotelId == hotelId).ToList();
            ViewBag.Hotels = new SelectList(db.Hotels, "HotelId", "Name", hotelId);

            return View("Index", rooms);
        }

        public ActionResult ToggleAvailability(int id)
        {
            var room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            room.IsAvailable = !room.IsAvailable;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult BulkUpdate()
        {
            return View(db.Rooms.ToList());
        }

        [HttpPost]
        public ActionResult BulkUpdatePrices(int[] RoomIds, decimal[] NewPrices)
        {
            for (int i = 0; i < RoomIds.Length; i++)
            {
                var room = db.Rooms.Find(RoomIds[i]);
                if (room != null)
                {
                    room.Price = NewPrices[i];
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Search(string roomType)
        {
            var rooms = string.IsNullOrEmpty(roomType)
                ? db.Rooms.ToList()
                : db.Rooms.Where(r => r.RoomType.Contains(roomType)).ToList();

            return View("Index", rooms);
        }


    }
}
