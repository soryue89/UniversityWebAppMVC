using Park_University_MVC.BusinessLayer;
using Park_University_MVC.Models;
using Park_University_MVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;

namespace Park_University_MVC.Controllers
{
    public class RegistrarController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        public ActionResult ShowEnrollment()
        {
            EnrollmentVM evm = new EnrollmentVM();
            evm.CList = BusinessRegistrar.GetCourses();
            evm.EList = BusinessRegistrar.GetEnrollmentForACourse(evm.CList[0].Value);
            evm.CourseSelected = evm.CList[0].Value;
            return View(evm);
        }

        [HttpPost]
        public ActionResult ShowEnrollment(EnrollmentVM evm)
        {
            evm.EList = BusinessRegistrar.GetEnrollmentForACourse(evm.CourseSelected);
            evm.CList = BusinessRegistrar.GetCourses();
            return View(evm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetEnrollmentsPartial(string id)
        {
            List<Enrollment> EList = BusinessRegistrar.GetEnrollmentForACourse(id);            
            if (Request.IsAjaxRequest())
            {
                return ReturnJsonGet("200", SerializeControl("~/Views/Registrar/_ShowEnrollment.cshtml", EList), "");
            }
            return PartialView("_ShowEnrollent", EList);
        }

        [Authorize(Roles = "Admin, Student")]
        public ActionResult _DeleteEnrollment(long studentId, string courseName)
        {
            bool res = BusinessRegistrar.DeleteEnrollmentForStudent(studentId, courseName);
            if (res == true)
                ViewBag.Message = courseName + " for " + studentId.ToString() + " is deleted.";
            else
                ViewBag.Message = "Cannot delete the enrollment. Contact system administrator.";
            EnrollmentVM evm = new EnrollmentVM();
            evm.CList = BusinessRegistrar.GetCourses();
            evm.CourseSelected = "";
            if (evm.CList.Count != 0)
            {
                evm.CourseSelected = evm.CList[0].Value;
                evm.EList = BusinessRegistrar.GetEnrollmentForACourse(evm.CourseSelected);
            }
            else
                evm.EList = BusinessRegistrar.GetEnrollmentForACourse();
            if (HttpContext.User.IsInRole("Admin"))
                return RedirectToAction("ShowEnrollment", evm);
            else
            {
                return RedirectToAction("CourseRegister");
            }                
        }

        [Authorize(Roles ="Admin")]
        public ActionResult Courses()
        {
            CoursesManagementVM cmv = new CoursesManagementVM();
            cmv.Course = new Courses();
            cmv.CList = BusinessRegistrar.GetExistingCourses();
            return View(cmv);
        }

        [HttpPost]
        public ActionResult Courses(CoursesManagementVM cmv)
        {
            return View(cmv);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult _AddCourse()
        {
            Courses c = new Courses();
            return View(c);
        }

        [HttpPost]
        public ActionResult _AddCourse(Courses c)
        {
            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                bool res = BusinessRegistrar.CreateCourse(c);
                if (res == true)
                {
                    c = new Courses();
                    ViewBag.Message = c.CourseNum + "created successfully";
                }
                else
                {
                    ViewBag.Message = "Error Occured. Try again.";
                }
            }
            return View(c);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetEditCoursePartial(string id)
        {
            Courses c = new Courses();
            c = BusinessRegistrar.GetCourseDetail(id);
            return PartialView("_UpdateStudent", c);
        }

        [HttpPost]
        public ActionResult GetEditCoursePartial(Courses c)
        {
            if (!ModelState.IsValid)
                return View(c);
            bool res = BusinessRegistrar.UpdateCourse(c);
            if (res)
                return Json(new { result = true, responseText = "Update successfully" });
            else
                return Json(new { result = false, responseText = "couldn't Update the course" });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult _UpdateCourse(string id)
        {
            Courses c = new Courses();
            if (ModelState.IsValid)
            {
                c = BusinessRegistrar.GetCourseDetail(id);
            }
            return View(c);
        }

        [HttpPost]
        public ActionResult _UpdateCourse(Courses c)
        {
            if (!ModelState.IsValid)
                return View(c);
            bool res = BusinessRegistrar.UpdateCourse(c);
            if (res)
                return Json(new { result = true, responseText = "Update successfully" });
            else
                return Json(new { result = false, responseText = "couldn't Update the course" });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Students()
        {
            StudentsManagementVM sm = new StudentsManagementVM();
            sm.STDList = BusinessRegistrar.GetStudents();
            return View(sm);
        }

        [HttpPost]
        public ActionResult Students(StudentsManagementVM sm)
        {
            sm.STDList = BusinessRegistrar.GetStudents();
            return View(sm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult _CreateStudent()
        {
            Students s = new Students();
            return View(s);
        }

        [HttpPost]
        public ActionResult _CreateStudent(Students s)
        {
            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                bool res = BusinessRegistrar.CreateStudent(s);
                if (res == true)
                {
                    ViewBag.Message = s.StudentId + " created successfully";
                    s = new Students();
                }
                else
                {
                    ViewBag.Message = "Error Occured. Try again.";
                }
            }
            return View(s);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult _UpdateStudent(long studentId)
        {
            Students s = new Students();
            if (ModelState.IsValid)
            {
                s = BusinessRegistrar.GetStudentDetail(studentId);
            }
            return PartialView(s);
        }

        [HttpPost]
        public ActionResult _UpdateStudent(Students s)
        {
            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                bool update = BusinessRegistrar.UpdateStudent(s);
                if (update == true)
                {
                    ViewBag.Message = s.FirstName + "'s information updated successfully";
                }
                else
                {
                    ViewBag.Message = "Error Occured. Try again.";
                }
            }
            return Json(new { result = true, responseText = "update successfully" });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteStudent(string id)
        {
            try
            {
                bool ret = BusinessRegistrar.DeleteStudent(long.Parse(id));
                if (ret)
                    return new HttpStatusCodeResult(200);
                else
                    return new HttpStatusCodeResult(500);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return new HttpStatusCodeResult(500);
            }
        }

        [Authorize(Roles = "Student")]
        public ActionResult ShowTranscript()
        {
            ShowTranscriptVM tlist = new ShowTranscriptVM();
            tlist.TranscriptList = new List<StudentTranscript>();
            if (ModelState.IsValid)
            {
                string user = HttpContext.User.Identity.Name;
                if (user != null)
                {
                    tlist.TranscriptList = BusinessRegistrar.GetTranscript(user);
                    tlist.GPA = GetGPA(tlist.TranscriptList);
                }
                else
                    Redirect("~/Auth/Login");
            }            
            return View(tlist);
        }

        [HttpPost]
        public ActionResult ShowTranscript(ShowTranscriptVM tlist)
        {
            if (ModelState.IsValid)
            {
                string user = HttpContext.User.Identity.Name;
                tlist.TranscriptList = BusinessRegistrar.GetTranscript(user);
            }
            return View(tlist);
        }

        [Authorize(Roles = "Student")]
        public ActionResult CourseRegister(string register = null, CourseRegisterVM c = null)
        {
            CourseRegisterVM crvm = c;
            if (register == null)
            {
                crvm = new CourseRegisterVM();
                crvm.SList = new List<SelectListItem>();
                crvm.CList = new List<SelectListItem>();
                crvm.EList = new List<Enrollment>();
                crvm.SelectedCoursesNum = "";
                crvm.SelectedSemester = "";
                crvm.AddCourse = false;
            }
            //if there is any data for each list
            if (ModelState.IsValid)
            {
                crvm.SList = BusinessRegistrar.GetSemester();
                if (crvm.SList.Count != 0)
                    crvm.SelectedSemester = crvm.SList[0].Value;
                crvm.CList = BusinessRegistrar.GetCoursesForSemester(crvm.SelectedSemester);
                if (crvm.CList.Count != 0)
                    crvm.SelectedCoursesNum = crvm.CList[0].Value;
                string username = HttpContext.User.Identity.Name;
                if (username != null)
                    crvm.EList = BusinessRegistrar.GetEnrollmentForACourse(null,username);
            }            
            return View(crvm);
        }

        [HttpPost]
        public ActionResult CourseRegister(CourseRegisterVM crvm)
        {
            crvm.SList = BusinessRegistrar.GetSemester();
            crvm.CList = BusinessRegistrar.GetCoursesForSemester(crvm.SelectedSemester);
            string username = HttpContext.User.Identity.Name;
            if (username != null)
                crvm.EList = BusinessRegistrar.GetEnrollmentForACourse(null,username);
            return View(crvm);
        }

        public ActionResult _Register(string cnum, string semester)
        {
            string username = HttpContext.User.Identity.Name;
            CourseRegisterVM crvm = new CourseRegisterVM();
            bool add = BusinessRegistrar.SignUpCourse(username, cnum, semester);
            crvm.SList = BusinessRegistrar.GetSemester();
            crvm.SelectedCoursesNum = cnum;
            crvm.SelectedSemester = semester;
            crvm.CList = BusinessRegistrar.GetCoursesForSemester(crvm.SelectedSemester);
            crvm.EList = BusinessRegistrar.GetEnrollmentForACourse(null, username);
            crvm.AddCourse = add;
            crvm.ClickRegister = true;
            if (add)
                return RedirectToAction("CourseRegister");
            else
            {
                ViewBag.Message = "Fail.";
                return View();
            }
        }

        double GetGPA(List<StudentTranscript> list)
        {
            double gpa = 0;
            foreach (var item in list)
                gpa += item.Grade;
            gpa = gpa / (list.Count * 1.0);
            return gpa;
        }
    }
}