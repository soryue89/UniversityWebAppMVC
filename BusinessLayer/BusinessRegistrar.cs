using Park_University_MVC.DataLayer;
using Park_University_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Park_University_MVC.BusinessLayer
{
    public class BusinessRegistrar
    {
        public static List<SelectListItem> GetCourses()
        {
            return RepositoryRegistrar.GetCourses();
        }

        public static List<SelectListItem> GetSemester()
        {
            return RepositoryRegistrar.GetSemester();
        }

        public static List<SelectListItem> GetCoursesForSemester(string semester)
        {
            return RepositoryRegistrar.GetCoursesForSemester(semester);
        }

        public static List<Enrollment> GetEnrollmentForACourse(string cnum = null, string userName = null)
        {
            
            if(userName != null)
            {
                long studentId = RepositoryAuth.VerifyUser(userName);
                
                if (studentId == 0)
                    throw new Exception("Error!!");
                else
                {
                    if (cnum == null)
                        return RepositoryRegistrar.GetEnrollmentForACourse(null, studentId);
                    else
                        return RepositoryRegistrar.GetEnrollmentForACourse(cnum, studentId);
                }
            }
            else
            {
                return RepositoryRegistrar.GetEnrollmentForACourse(cnum, 0);
            }
        }

        public static bool DeleteEnrollmentForStudent(long studentId, string courseName)
        {
            return RepositoryRegistrar.DeleteEnrollmentForStudent(studentId, courseName);
        }

        public static List<StudentTranscript> GetTranscript(string username)
        {
            return RepositoryRegistrar.GetTranscript(username);
        }

        public static List<Courses> GetExistingCourses()
        {
            return RepositoryRegistrar.GetExistingCourses();
        }

        public static List<Students> GetStudents()
        {
            return RepositoryRegistrar.GetStudents();
        }

        public static Courses GetCourseDetail(string cnum)
        {
            return RepositoryRegistrar.GetCourseDetail(cnum);
        }

        public static bool CreateCourse(Courses c)
        {
            return RepositoryRegistrar.CreateCourse(c);
        }

        public static bool UpdateCourse(Courses c)
        {
            return RepositoryRegistrar.UpdateCourse(c);
        }

        public static Students GetStudentDetail(long studentId)
        {
            return RepositoryRegistrar.GetStudentDetail(studentId);
        }

        public static bool CreateStudent(Students s)
        {
            return RepositoryRegistrar.CreateStudent(s);
        }

        public static bool UpdateStudent(Students s)
        {
            return RepositoryRegistrar.UpdateStudent(s);
        }
        public static bool DeleteStudent(long studentId)
        {
            return RepositoryRegistrar.DeleteStudent(studentId);
        }

        public static bool SignUpCourse(string username, string cnum, string semester)
        {
            bool res = false;
            long studentId = RepositoryAuth.VerifyUser(username);
            try
            {
                if (studentId != 0)
                {
                    List<Enrollment> cs = RepositoryRegistrar.GetEnrollmentForACourse(cnum, studentId);
                    if (cs.Count == 0)
                    {
                        if (RepositoryRegistrar.CheckPrerequisiteCourseTaken(studentId, cnum))
                            res = RepositoryRegistrar.SignUpCourse(studentId, cnum, semester);
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return res;
        }
    }
}
