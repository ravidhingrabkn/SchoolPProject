using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;
using System.Diagnostics;

namespace SchoolProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }
        // GET : /Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers =  controller.ListTeachers(SearchKey);
            return View(Teachers);
        }

        //GET : Teacher/Show/{id}

        public ActionResult Show(int id)
        {
            TeacherDataController controller =new TeacherDataController();
            Teacher newTeacher = controller.FindTeacher(id);
           
            return View(newTeacher);
        }
        // GET : /Author/New
        public ActionResult New()
        {
            return View();
        }

        //POST : /Teacher/Create
        [HttpPost]

        public ActionResult Create(string teacherfname, string teacherlname, string employeenumber)
        {
            Debug.WriteLine("the teacher info is : " + teacherfname + " " + teacherlname + " " + employeenumber);

            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = teacherfname;
            NewTeacher.TeacherLname = teacherlname;
            NewTeacher.TeacherNumber = employeenumber;

            TeacherDataController controller = new TeacherDataController();

            controller.AddTeacher(NewTeacher);


            
            return RedirectToAction("List");
        }
        //GET: /Teacher/DeleteConfirm/{id}
        //[Route("/Teacher/DeleteConfirm/{TeacherId}")]
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher newTeacher = controller.FindTeacher(id);

            return View(newTeacher);
        }
        //POST: /Teacher/Delete/{id}
        [HttpPost]
        
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);

            return RedirectToAction("List");

        }

    }
}