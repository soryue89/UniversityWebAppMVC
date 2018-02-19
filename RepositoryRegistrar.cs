using Park_University_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Park_University_MVC.DataLayer
{
    public class RepositoryRegistrar
    {
        public static List<SelectListItem> GetCourses()
        {
            List<SelectListItem> clist = new List<SelectListItem>();
            try
            {
                string sql = "select * from Courses";

                DataTable dt = DataAccess.GetManyRowsCols(sql, null);
                foreach (DataRow dr in dt.Rows)
                {
                    SelectListItem si = new SelectListItem();
                    si.Value = dr["CourseNum"].ToString();
                    si.Text = dr["CourseNum"].ToString();
                    clist.Add(si);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return clist;
        }

        public static List<SelectListItem> GetSemester()
        {
            List<SelectListItem> semester = new List<SelectListItem>();
            try
            {
                string sql = "select SemesterId from Semesters";
                DataTable dt = DataAccess.GetManyRowsCols(sql, null);
                foreach (DataRow dr in dt.Rows)
                {
                    SelectListItem si = new SelectListItem();
                    si.Value = dr["SemesterId"].ToString();
                    si.Text = dr["SemesterId"].ToString();
                    semester.Add(si);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return semester;
        }

        public static List<SelectListItem> GetCoursesForSemester(string semester)
        {
            List<SelectListItem> courses = new List<SelectListItem>();
            try
            {
                string sql = "select CourseNum from CoursesOffered where SemesterId=@SemesterId";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@SemesterId", SqlDbType.VarChar, semester, 50);
                DataTable dt = DataAccess.GetManyRowsCols(sql, plist);
                foreach (DataRow dr in dt.Rows)
                {
                    SelectListItem si = new SelectListItem();
                    si.Value = dr["CourseNum"].ToString();
                    si.Text = dr["CourseNum"].ToString();
                    courses.Add(si);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return courses;
        }

        public static List<Enrollment> GetEnrollmentForACourse(string cnum = null, long studentId = 0)
        {
            List<Enrollment> elist = new List<Enrollment>();
            try
            {
                string sql = "select s.StudentId, s.FirstName, s.LastName, c.CourseName, e.SemesterId as Semester, c.Credits "
                    + "from Students s inner join CourseEnrollments e on s.StudentId=e.StudentId "
                    + "inner join Courses c on e.CourseNum=c.CourseNum ";
                if(studentId == 0 && cnum != null)
                {
                    sql += "where c.CourseNum=@CourseNum";
                }
                else if(studentId != 0 && cnum == null)
                {
                    sql +=  "where e.StudentId=@StudentId";
                }
                else
                {
                    sql += "where e.StudentId=@StudentId and c.CourseNum=@CourseNum";
                }

                List<SqlParameter> plist = new List<SqlParameter>();
                
                if (studentId != 0)
                    DBHelper.addSqlParam(plist, "@StudentId", SqlDbType.BigInt, studentId);
                if (cnum != null)
                    DBHelper.addSqlParam(plist, "@CourseNum", SqlDbType.VarChar, cnum, 50);
                DataTable dt = DataAccess.GetManyRowsCols(sql, plist);
                foreach (DataRow dr in dt.Rows)
                {
                    Enrollment e1 = new Enrollment();
                    e1.StudentId = (long)dr["StudentId"];
                    e1.FirstName = dr["FirstName"].ToString();
                    e1.LastName = dr["LastName"].ToString();
                    e1.CourseName = dr["CourseName"].ToString();
                    e1.Semester = dr["Semester"].ToString();
                    e1.Credits = (int)dr["Credits"];
                    elist.Add(e1);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return elist;
        }

        public static bool DeleteEnrollmentForStudent(long studentId, string courseName)
        {
            bool res = false;
            try
            {
                string sql = "delete from CourseEnrollments "
                    +"where StudentId=@StudentId and CourseNum in (select CourseNum from Courses "
                    + "where CourseName=@CourseName)";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@StudentId", SqlDbType.BigInt, studentId);
                DBHelper.addSqlParam(plist, "@CourseName", SqlDbType.VarChar, courseName);
                int deleted = DataAccess.InsertUpdateDelete(sql, plist);
                if (deleted > 0)
                    res = true;
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }

        public static List<StudentTranscript> GetTranscript(string username)
        {
            List<StudentTranscript> stlist = new List<StudentTranscript>();
            try
            {
                string sql = "select cc.CourseNum, cc.Grade, cc.SemesterId as Semester from CoursesCompleted cc "
                    + "inner join Users u on cc.StudentId=u.StudentId where u.UserName=@UserName";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@UserName", SqlDbType.VarChar, username, 50);
                DataTable dt = DataAccess.GetManyRowsCols(sql, plist);
                foreach(DataRow dr in dt.Rows)
                {
                    StudentTranscript st = new StudentTranscript();
                    st.CourseNum = dr["CourseNum"].ToString();
                    st.Grade = (double)dr["Grade"];
                    st.Semester = dr["Semester"].ToString();
                    stlist.Add(st);
                }
            }
            catch(Exception)
            {
                throw;
            }
            return stlist;
        }

        public static List<Courses> GetExistingCourses()
        {
            List<Courses> clist = new List<Courses>();
            try
            {
                string sql = "select * from Courses";
                DataTable dt = DataAccess.GetManyRowsCols(sql, null);
                foreach (DataRow dr in dt.Rows)
                {
                    Courses c1 = new Courses();
                    c1.CourseNum = dr["CourseNum"].ToString();
                    c1.CourseName = dr["CourseName"].ToString();
                    c1.Credits = (int) dr["Credits"];
                    c1.DepartmentId = (int)dr["DepartmentId"];
                    c1.Description = dr["Description"].ToString();
                    clist.Add(c1);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return clist;
        }

        public static Courses GetCourseDetail(string cnum)
        {
            Courses c1 = new Courses();
            try
            {
                string sql = "select * from Courses where CourseNum=@CourseNum";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@CourseNum", SqlDbType.VarChar, cnum, 50);
                DataTable dt = DataAccess.GetManyRowsCols(sql, plist);
                foreach (DataRow dr in dt.Rows)
                {
                    c1.CourseNum = dr["CourseNum"].ToString();
                    c1.CourseName = dr["CourseName"].ToString();
                    c1.Credits = (int)dr["Credits"];
                    c1.DepartmentId = (int)dr["DepartmentId"];
                    c1.Description = dr["Description"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return c1;
        }

        public static bool CreateCourse(Courses c)
        {
            bool res = false;
            try
            {
                string sql = "insert into Courses(CourseNum,DepartmentId,CourseName,Description,Credits) "
                    + "values (@CourseNum,@DepartmentId,@CourseName,@Description,@Credits)";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@CourseNum", SqlDbType.VarChar, c.CourseNum, 50);
                DBHelper.addSqlParam(plist, "@DepartmentId", SqlDbType.VarChar, c.DepartmentId, 50);
                DBHelper.addSqlParam(plist, "@CourseName", SqlDbType.VarChar, c.CourseName, 50);
                DBHelper.addSqlParam(plist, "@Description", SqlDbType.Text, c.Description);
                DBHelper.addSqlParam(plist, "@Credits", SqlDbType.Int, c.Credits);
                
                int created = DataAccess.InsertUpdateDelete(sql, plist);
                if (created > 0)
                    res = true;
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }

        public static bool UpdateCourse(Courses c)
        {
            bool res = false;
            try
            {
                string sql = "update Courses set DepartmentId=@DepartmentId, "
                    + "CourseName=@CourseName, Description=@Description, Credits=@Credits where CourseNum=@CourseNum";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@CourseNum", SqlDbType.VarChar, c.CourseNum, 50);
                DBHelper.addSqlParam(plist, "@DepartmentId", SqlDbType.VarChar, c.DepartmentId, 50);
                DBHelper.addSqlParam(plist, "@CourseName", SqlDbType.VarChar, c.CourseName, 50);
                DBHelper.addSqlParam(plist, "@Description", SqlDbType.Text, c.Description);
                DBHelper.addSqlParam(plist, "@Credits", SqlDbType.Int, c.Credits);
                int updated = DataAccess.InsertUpdateDelete(sql, plist);
                if (updated > 0)
                    res = true;
            }
            catch(Exception)
            {
                throw;
            }
            return res;
        }

        public static List<Students> GetStudents()
        {
            List<Students> slist = new List<Students>();
            try
            {
                string sql = "select * from Students";
                List<SqlParameter> plist = new List<SqlParameter>();
                DataTable dt = DataAccess.GetManyRowsCols(sql, plist);
                foreach (DataRow dr in dt.Rows)
                {
                    Students s = new Students();
                    s.StudentId = (long)dr["StudentId"];
                    s.FirstName = dr["FirstName"].ToString();
                    s.LastName = dr["LastName"].ToString();
                    s.City = dr["City"].ToString();
                    s.State = dr["State"].ToString();
                    s.Email = dr["Email"].ToString();
                    s.Telephone = dr["Telephone"].ToString();
                    slist.Add(s);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return slist;
        }

        public static Students GetStudentDetail(long studentId)
        {
            Students s = new Students();
            try
            {
                string sql = "select * from Students where StudentId=@StudentId";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@StudentId", SqlDbType.BigInt, studentId);
                DataTable dt = DataAccess.GetManyRowsCols(sql, plist);
                foreach(DataRow dr in dt.Rows)
                {
                    s.StudentId = (long)dr["StudentId"];
                    s.FirstName = dr["FirstName"].ToString();
                    s.LastName = dr["LastName"].ToString();
                    s.Street = dr["Street"].ToString();
                    s.City = dr["City"].ToString();
                    s.State = dr["State"].ToString();
                    s.Email = dr["Email"].ToString();
                    s.Telephone = dr["Telephone"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return s;
        }
        public static bool CreateStudent(Students s)
        {
            bool res = false;
            try
            {
                string sql = "insert into Students values(@FirstName, @LastName, @Street, @City, "
                    + "@State, @Email, @Telephone)";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@FirstName", SqlDbType.VarChar,s.FirstName,50);
                DBHelper.addSqlParam(plist, "@LastName", SqlDbType.VarChar,s.LastName,50);
                DBHelper.addSqlParam(plist, "@Street", SqlDbType.VarChar, s.Street, 50);
                DBHelper.addSqlParam(plist, "@City", SqlDbType.VarChar,s.City,50);
                DBHelper.addSqlParam(plist, "@State", SqlDbType.VarChar,s.State,50);
                DBHelper.addSqlParam(plist, "@Email", SqlDbType.VarChar,s.Email,50);
                DBHelper.addSqlParam(plist, "@Telephone", SqlDbType.VarChar,s.Telephone,20);

                int create = DataAccess.InsertUpdateDelete(sql, plist);
                if (create > 0)
                    res = true;
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }

        public static bool UpdateStudent(Students s)
        {
            bool res = false;
            try
            {
                string sql = "Update Students set FirstName=@FirstName, LastName=@LastName, "
                    + "Street=@Street, City=@City, State=@State, Email=@Email, Telephone=@Telephone "
                    + "where StudentId=@StudentId";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@StudentId", SqlDbType.BigInt, s.StudentId);
                DBHelper.addSqlParam(plist, "@FirstName", SqlDbType.VarChar, s.FirstName, 50);
                DBHelper.addSqlParam(plist, "@LastName", SqlDbType.VarChar, s.LastName, 50);
                DBHelper.addSqlParam(plist, "@Street", SqlDbType.VarChar, s.Street, 50);
                DBHelper.addSqlParam(plist, "@City", SqlDbType.VarChar, s.City, 50);
                DBHelper.addSqlParam(plist, "@State", SqlDbType.VarChar, s.State, 50);
                DBHelper.addSqlParam(plist, "@Email", SqlDbType.VarChar, s.Email, 50);
                DBHelper.addSqlParam(plist, "@Telephone", SqlDbType.VarChar, s.Telephone, 20);

                int updated = DataAccess.InsertUpdateDelete(sql, plist);
                if (updated > 0)
                    res = true;
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }

        public static bool DeleteStudent(long studentId)
        {
            bool res = false;
            try
            {
                string sql = "delete from Students where StudentId=@StudentId";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@StudentId", SqlDbType.BigInt, studentId);
                int deleted = DataAccess.InsertUpdateDelete(sql, plist);
                if (deleted > 0)
                    res = true;
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }

        public static bool SignUpCourse(long studentId, string cnum, string semester)
        {
            bool res = false;
            try
            {
                string sql = "insert into CourseEnrollments values(@StudentId,@SemesterId,@CourseNum)";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@StudentId", SqlDbType.BigInt, studentId);
                DBHelper.addSqlParam(plist, "@SemesterId", SqlDbType.VarChar, semester, 50);
                DBHelper.addSqlParam(plist, "@CourseNum", SqlDbType.VarChar, cnum, 50);
                int added = DataAccess.InsertUpdateDelete(sql, plist);
                if (added > 0)
                    res = true;
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }

        public static bool CheckPrerequisiteCourseTaken(long studentId, string cnum)
        {
            bool res = false;
            try
            {
                string sql = "select cc.CourseNum from CoursesCompleted cc "
                    + "where cc.StudentId=@StudentId and cc.CourseNum in "
                    + "(select p.PrereqCourseNum from CoursePrerequisites p where "
                    + "p.CourseNum=@CourseNum)";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@StudentId", SqlDbType.BigInt, studentId);
                DBHelper.addSqlParam(plist, "@CourseNum", SqlDbType.VarChar, cnum, 50);
                object obj = DataAccess.GetSingleAnswer(sql, plist);
                if (obj != null)
                    res = true;
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }
    }
}
