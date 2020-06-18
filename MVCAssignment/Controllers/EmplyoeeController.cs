using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCAssignment;
using PagedList;

namespace MVCAssignment.Controllers
{
    public class EmplyoeeController : Controller
    {
        private UserEntities Context = new UserEntities();


        //public ActionResult Index()
        //{
        //    return View(Context.Emplyoees.ToList());
        //}

        public ActionResult Index(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
        {
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "FirstName" : "";
            ViewBag.SortingDate = Sorting_Order == "LastName" ? "City" : "Country";

            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;

            var students = from stu in Context.Emplyoees select stu;

            if (!String.IsNullOrEmpty(Search_Data))
            {
                students = students.Where(stu => stu.FirstName.ToUpper().Contains(Search_Data.ToUpper())
                    || stu.LastName.ToUpper().Contains(Search_Data.ToUpper()));
            }
            switch (Sorting_Order)
            {
                case "FirstName":
                    students = students.OrderByDescending(stu => stu.FirstName);
                    break;
                case "LastName":
                    students = students.OrderBy(stu => stu.LastName);
                    break;
                case "Date_Description":
                    students = students.OrderByDescending(stu => stu.EmailID);
                    break;
                case "City":
                    students = students.OrderByDescending(stu => stu.City);
                    break;
                case "Country":
                    students = students.OrderByDescending(stu => stu.Country);
                    break;
                default:
                    students = students.OrderBy(stu => stu.FirstName);
                    break;
            }

            int Size_Of_Page = 4;
            int No_Of_Page = (Page_No ?? 1);
            return View(students.ToPagedList(No_Of_Page, Size_Of_Page));
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplyoee emplyoee = Context.Emplyoees.Find(id);
            if (emplyoee == null)
            {
                return HttpNotFound();
            }
            return View(emplyoee);
        }

        public ActionResult Create()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailID,City,Country")] Emplyoee emplyoee)
        {
            if (ModelState.IsValid)
            {
                Context.Emplyoees.Add(emplyoee);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emplyoee);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplyoee emplyoee = Context.Emplyoees.Find(id);
            if (emplyoee == null)
            {
                return HttpNotFound();
            }
            return View(emplyoee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailID,City,Country")] Emplyoee emplyoee)
        {
            if (ModelState.IsValid)
            {
                Context.Entry(emplyoee).State = EntityState.Modified;
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emplyoee);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emplyoee emplyoee = Context.Emplyoees.Find(id);
            if (emplyoee == null)
            {
                return HttpNotFound();
            }
            return View(emplyoee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emplyoee emplyoee = Context.Emplyoees.Find(id);
            Context.Emplyoees.Remove(emplyoee);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
