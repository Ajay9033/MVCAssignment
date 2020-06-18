using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCAssignment;
using System.Linq.Dynamic;

namespace MVCAssignment.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private UserEntities db = new UserEntities();

        // GET: Employees
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Emplyoees.ToList());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult LoadData()
        {
            //jQuery DataTables Param
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find order columns info
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault()
                                    + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            //find search columns info
            var firstName = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var city = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;


            using (UserEntities dc = new UserEntities())
            {
                // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
                var v = (from a in dc.Emplyoees select a);

                //SEARCHING...
                if (!string.IsNullOrEmpty(firstName))
                {
                    v = v.Where(a => a.FirstName.Contains(firstName));
                }
                if (!string.IsNullOrEmpty(city))
                {
                    v = v.Where(a => a.Country == city);
                }
                //SORTING...  (For sorting we need to add a reference System.Linq.Dynamic)
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn + " " + sortColumnDir);
                }

                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data },
                    JsonRequestBehavior.AllowGet);

            }
        }

        // GET: Employees/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplyoee emplyoee = db.Emplyoees.Find(id);
            if (emplyoee == null)
            {
                return HttpNotFound();
            }
            return View(emplyoee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailID,City,Country")] Emplyoee emplyoee)
        {
            if (ModelState.IsValid)
            {
                db.Emplyoees.Add(emplyoee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emplyoee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplyoee emplyoee = db.Emplyoees.Find(id);
            if (emplyoee == null)
            {
                return HttpNotFound();
            }
            return View(emplyoee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailID,City,Country")] Emplyoee emplyoee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emplyoee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emplyoee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplyoee emplyoee = db.Emplyoees.Find(id);
            if (emplyoee == null)
            {
                return HttpNotFound();
            }
            return View(emplyoee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emplyoee emplyoee = db.Emplyoees.Find(id);
            db.Emplyoees.Remove(emplyoee);
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
    }
}
