using System.Linq;
using System.Web.Mvc;
using HotelManagement.Filters;
using HotelManagement.Models;
using HotelManagement.Models.ViewModels;

namespace HotelManagement.Controllers
{
    //[AdminAuthorize]
    public class AdminController : Controller
    {
        private HotelDbContext db = new HotelDbContext();

        // GET: Admin/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var admin = db.Admins.SingleOrDefault(a => a.Username == username && a.Password == password);
            if (admin != null)
            {
                Session["AdminLoggedIn"] = true;
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid credentials!";
            return View();
        }

        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            if (Session["AdminLoggedIn"] == null || !(bool)Session["AdminLoggedIn"])
            {
                return RedirectToAction("Login");
            }

            var totalRooms = db.Rooms.Count();
            var bookedRooms = db.Bookings.Select(b => b.RoomId).Distinct().Count();
            //var totalRevenue = db.Bookings.Sum(b => b.Revenue);

            ViewBag.TotalHotels = db.Hotels.Count();
            ViewBag.TotalRooms = totalRooms;
            ViewBag.BookedRooms = bookedRooms;
            ViewBag.TotalBookings = db.Bookings.Count();
            //ViewBag.TotalRevenue = totalRevenue;
            ViewBag.OccupancyRate = totalRooms > 0 ? (bookedRooms / (double)totalRooms) * 100 : 0;

            return View();
        }



        // GET: Admin/Logout
        public ActionResult Logout()
        {
            Session["AdminLoggedIn"] = null;
            return RedirectToAction("Login");
        }

        [RoleAuthorize(RequiredRole = "SuperAdmin")]
        public ActionResult ManageAdmins()
        {
            return View(db.Admins.ToList());
        }

        [RoleAuthorize(RequiredRole = "SuperAdmin")]
        public ActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [RoleAuthorize(RequiredRole = "SuperAdmin")]
        public ActionResult CreateAdmin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("ManageAdmins");
            }
            return View(admin);
        }

    }
}
