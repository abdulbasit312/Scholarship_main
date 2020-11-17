using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oracle.ManagedDataAccess.Client;
using scholarship.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace scholarship.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        IEnumerable<SelectListItem> EDUCATIONAL = new List<SelectListItem>() {
            new SelectListItem{ Text="8th Class",Value="8th Class" },
            new SelectListItem{ Text="9th Class",Value="9th Class" },
            new SelectListItem{ Text="10th Class",Value="10th Class" },
            new SelectListItem{ Text="11th Class",Value="11th Class" },
            new SelectListItem{ Text="12th Class",Value="12th Class" },
            new SelectListItem{ Text="Diploma",Value="Diploma" },
            new SelectListItem{ Text="Graduate",Value="Graduate" }};
        IEnumerable<SelectListItem> COURSE = new List<SelectListItem>()
            {
                new SelectListItem{Text="Diploma(Computer Enginering)",Value="Diploma(Computer Engineering)"},
                new SelectListItem{Text="Diploma(Civil Engineering)",Value="Diploma(Civil Engineering)"},
                new SelectListItem{Text="Diploma(Mechanical Enginering)",Value="Diploma(Mechanical Engineering)"},
                new SelectListItem{ Text="Diploma(Electrical Enginering)",Value="Diploma(Electrical Engineering)"},
                new SelectListItem{ Text="Diploma(EC Enginering)",Value="Diploma(EC Engineering)"},
                new SelectListItem{ Text="Para Med Yr1",Value="Para Med Yr1"},
                new SelectListItem{ Text="Para Med Yr2",Value="Para Med Yr2"},
                new SelectListItem{ Text="Para Med Yr3",Value="Para Med Yr3"},
            };
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(COLLEGE college)
        {
            if (ModelState.IsValid)
            {
                OracleConnection con = new OracleConnection("DATA SOURCE = 192.168.56.101:1521 / orcl; PERSIST SECURITY INFO = True; USER ID = ABDUL; PASSWORD= abdul");
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "Insert into College(college_id,name,address) values (genid.nextval,' " + college.NAME + " ',' " + college.ADDRESS + " ')";
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("Display_Colleges");
            }
            return View(college);
        }
        public ActionResult Display_Colleges()
        {
            

            Entities1 a = new Entities1();
            var emp = from e in a.COLLEGEs orderby e.COLLEGE_ID select e;
            return View(emp);
        }

        public ActionResult Edit_College(int id)
        {
            COLLEGE item = new COLLEGE();
            OracleConnection con = new OracleConnection("DATA SOURCE = 192.168.56.101:1521 / orcl; PERSIST SECURITY INFO = True; USER ID = ABDUL; PASSWORD= abdul");
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = "select * from college where college_id="+id.ToString();
            cmd.Connection = con;
            con.Open();
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            item.NAME =dr["name"].ToString();
            item.ADDRESS = dr["address"].ToString();
            item.COLLEGE_ID = id;
            con.Close();
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_College(COLLEGE item)
        {
            if (ModelState.IsValid)
            {
                OracleConnection con = new OracleConnection("DATA SOURCE = 192.168.56.101:1521 / orcl; PERSIST SECURITY INFO = True; USER ID = ABDUL; PASSWORD= abdul");
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "Update college set name='" + item.NAME.ToString() + "',address='" + item.ADDRESS.ToString() + "' where college_id=" + item.COLLEGE_ID.ToString();
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("Display_Colleges");
        }
        public ActionResult Delete_College(int id)
        {
            OracleConnection con = new OracleConnection("DATA SOURCE = 192.168.56.101:1521 / orcl; PERSIST SECURITY INFO = True; USER ID = ABDUL; PASSWORD= abdul");
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = "delete from college where college_id=" + id.ToString();
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Display_Colleges");
        }
        public ActionResult AddStudent()
        {
            /*ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin"))
                                  .ToList(), "Name", "Name");*/
            
            Entities1 context = new Entities1();
            var a = context.COLLEGEs.ToList();
            ViewBag.Name= new SelectList(a,"COLLEGE_ID","NAME");
            ViewBag.Course = COURSE;
            ViewBag.Grade = EDUCATIONAL;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent(STUDENT a)
        {
            
            
                var save = a.DOB.ToString().Split(' ');
                OracleConnection con = new OracleConnection("DATA SOURCE = 192.168.56.101:1521 / orcl; PERSIST SECURITY INFO = True; USER ID = ABDUL; PASSWORD= abdul");
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "Insert into student(student_id,name,course,educational,age,dob,college_id,m_name,f_name,address) values ('" + a.STUDENT_ID.ToString().ToUpper() + "','" + a.NAME.ToString().ToUpper() + "','" + a.COURSE.ToString().ToUpper() + "','" + a.EDUCATIONAL.ToUpper() + "'," + a.AGE + ",To_date('" + save[0] + "','mm/dd/yyyy')," + a.COLLEGE_ID + ",'" + a.M_NAME.ToUpper() + "','" + a.F_NAME.ToUpper() + "','" + a.ADDRESS.ToUpper() + "')";
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("DisplayStudent");
            
           
        }
        public ActionResult DisplayStudent()
        {
            Entities1 context = new Entities1();
            var record = from e in context.STUDENTs orderby e.STUDENT_ID select e;
            return View(record);
        }
        public ActionResult EditStudent(string id)
        {

            STUDENT item = new STUDENT();
            Entities1 context = new Entities1();
            var a = context.COLLEGEs.ToList();
            ViewBag.Name = new SelectList(a, "COLLEGE_ID", "NAME");
            ViewBag.Course = COURSE;
            ViewBag.Grade = EDUCATIONAL;
            item = context.STUDENTs.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent(STUDENT a)
        {
            Entities1 context = new Entities1();
            var data=context.STUDENTs.FirstOrDefault(x=>x.STUDENT_ID==a.STUDENT_ID);
            data.ADDRESS = a.ADDRESS;
            data.AGE = a.AGE;
            data.COLLEGE_ID = a.COLLEGE_ID;
            data.COURSE = a.COURSE;
            data.DOA = a.DOA;
            data.DOB = a.DOB;
            data.EDUCATIONAL = a.EDUCATIONAL;
            data.F_NAME = a.F_NAME;
            data.M_NAME = a.M_NAME;
            data.NAME = a.NAME;
            data.COLLEGE = a.COLLEGE;
            data.STUDENT_ID = a.STUDENT_ID;
            context.SaveChanges();
            return RedirectToAction("DisplayStudent");
        }
    }
}